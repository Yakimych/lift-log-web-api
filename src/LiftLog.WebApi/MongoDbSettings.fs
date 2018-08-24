module LiftLog.MongoDbSettings

open Microsoft.Extensions.Configuration

type Settings = {
    mondoDbServer: string
    databaseName: string
    collectionName: string
}

let toMongoDbSettings (configuration: IConfiguration): Settings =
    {
        mondoDbServer = configuration.["mongoDbServer"]
        databaseName = configuration.["databaseName"]
        collectionName = configuration.["collectionName"]
    }