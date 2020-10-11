import { CosmosClient } from "@azure/cosmos";

export const getContainer = async (
    cosmosClient: CosmosClient,
  ) => {
    return await cosmosClient
      .database('MAIN_DB')
      .container('MAIN_CONTAINER');
  };