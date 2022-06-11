variable "webapp_name" {
  default = "liftlogwebapp"
}

variable "db_name" {
  default = "LiftLogDb"
}

variable "seq_url" {
  type = string
}

variable "seq_api_key" {
  type = string
}

variable "commit_hash" {
  type = string
}

variable "collection_name" {
  default = "LiftLogCollection"
}

# providers
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>2.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "tfstate"
    storage_account_name = "tfstateliftlog"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "liftloggroup" {
  name     = "LiftLogGroup"
  location = "germanywestcentral"
}

resource "azurerm_cosmosdb_account" "cosmosaccount" {
  name                 = "pscosmosaccount"
  location             = azurerm_resource_group.liftloggroup.location
  resource_group_name  = azurerm_resource_group.liftloggroup.name
  kind                 = "MongoDB"
  mongo_server_version = "4.0"

  consistency_policy {
    consistency_level = "Session"
  }

  capabilities {
    name = "EnableServerless"
  }

  capabilities {
    name = "EnableMongo"
  }

  geo_location {
    location          = azurerm_resource_group.liftloggroup.location
    failover_priority = 0
  }

  backup {
    type                = "Periodic"
    interval_in_minutes = 1440
    retention_in_hours  = 48
  }

  offer_type = "Standard"
  # enable_free_tier = true
}

resource "azurerm_cosmosdb_mongo_database" "cosmosdb" {
  name                = var.db_name
  resource_group_name = azurerm_resource_group.liftloggroup.name
  account_name        = azurerm_cosmosdb_account.cosmosaccount.name
}

resource "azurerm_cosmosdb_mongo_collection" "cosmoscollection" {
  name                = var.collection_name
  resource_group_name = azurerm_resource_group.liftloggroup.name
  account_name        = azurerm_cosmosdb_account.cosmosaccount.name
  database_name       = azurerm_cosmosdb_mongo_database.cosmosdb.name

  index {
    keys   = ["_id"]
    unique = true
  }
}

resource "azurerm_application_insights" "liftlogappinsights" {
  name                = "liftlog-appinsights"
  location            = azurerm_resource_group.liftloggroup.location
  resource_group_name = azurerm_resource_group.liftloggroup.name
  application_type    = "web"
}

resource "azurerm_application_insights_web_test" "liftlogwebtest" {
  name                    = "liftlog-appinsights-webtest"
  location                = azurerm_resource_group.liftloggroup.location
  resource_group_name     = azurerm_resource_group.liftloggroup.name
  application_insights_id = azurerm_application_insights.liftlogappinsights.id
  kind                    = "ping"
  frequency               = 300
  timeout                 = 120
  enabled                 = true
  geo_locations           = ["emea-nl-ams-azr", "emea-gb-db3-azr"]

  configuration = <<XML
<WebTest Name="Health Check ping" Id="45E9A843-3A90-456C-A402-A44E94CA66C3" Enabled="True" CssProjectStructure="" CssIteration="" Timeout="120" WorkItemIds="" xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" Description="" CredentialUserName="" CredentialPassword="" PreAuthenticate="True" Proxy="default" StopOnError="False" RecordedResultFile="" ResultsLocale="">
  <Items>
    <Request Method="GET" Guid="fbb64104-508e-4494-b994-2e1f4cd8c8fd" Version="1.1" Url="https://${var.webapp_name}.azurewebsites.net/healthcheck" ThinkTime="0" Timeout="120" ParseDependentRequests="False" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="200" ExpectedResponseUrl="" ReportingName="" IgnoreHttpStatusCode="False" />
  </Items>
</WebTest>
  XML
}

resource "azurerm_app_service_plan" "liftlogapp" {
  name                = "liftlog-plan"
  location            = azurerm_resource_group.liftloggroup.location
  resource_group_name = azurerm_resource_group.liftloggroup.name
  kind                = "Windows"
  reserved            = false

  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_app_service" "liftlogapp" {
  name                = var.webapp_name
  location            = azurerm_resource_group.liftloggroup.location
  resource_group_name = azurerm_resource_group.liftloggroup.name
  app_service_plan_id = azurerm_app_service_plan.liftlogapp.id
  https_only          = true

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"       = azurerm_application_insights.liftlogappinsights.instrumentation_key
    "DeployDate"                           = timestamp()
    "AppVersion"                           = var.commit_hash
    "MongoDbServer"                        = azurerm_cosmosdb_account.cosmosaccount.connection_strings[0]
    "DatabaseName"                         = var.db_name
    "CollectionName"                       = var.collection_name
    "Serilog__WriteTo__0__Args__serverUrl" = var.seq_url
    "Serilog__WriteTo__0__Args__apiKey"    = var.seq_api_key
  }

  site_config {
    dotnet_framework_version  = "v6.0"
    use_32_bit_worker_process = true
  }
}
