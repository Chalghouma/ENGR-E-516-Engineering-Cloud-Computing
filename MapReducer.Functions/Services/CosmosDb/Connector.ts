import { CosmosClient, CosmosClientOptions } from "@azure/cosmos";
import { config } from "dotenv";

export const getCosmosDbClient = () => {
  return new CosmosClient(getCosmosConnectionOptionsFromEnv());
};
export const getCosmosConnectionOptionsFromEnv = (): CosmosClientOptions => {
  config();
  return {
    endpoint: process.env.COSMOS_DB_URL as string,
    key: process.env.COSMOS_DB_KEY as string,
  };
};
