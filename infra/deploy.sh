#!/bin/bash

# TODO: Can this be moved into a pipeline step?
export ARM_CLIENT_ID=$servicePrincipalId
export ARM_CLIENT_SECRET=$servicePrincipalKey
export ARM_SUBSCRIPTION_ID=$(az account show --query id | xargs)
export ARM_TENANT_ID=$(az account show --query tenantId | xargs)

PUBLISH_FILE=$1
SEQ_URL=$2
SEQ_API_KEY=$3
COMMIT_HASH=$4

RESOURCE_GROUP_NAME=tfstate
STORAGE_ACCOUNT_NAME=tfstateliftlog
CONTAINER_NAME=tfstate

ACCOUNT_KEY=$(az storage account keys list --resource-group $RESOURCE_GROUP_NAME --account-name $STORAGE_ACCOUNT_NAME --query '[0].value' -o tsv)
export ARM_ACCESS_KEY=$ACCOUNT_KEY

terraform plan \
    -var "seq_url=$SEQ_URL" \
    -var "seq_api_key=$SEQ_API_KEY" \
    -var "commit_hash=$COMMIT_HASH" \
    --out=main.tfplan

terraform apply main.tfplan

az webapp deploy \
    --resource-group "LiftLogGroup" \
    --name "liftlogwebapp" \
    --type=zip \
    --src-path $PUBLISH_FILE