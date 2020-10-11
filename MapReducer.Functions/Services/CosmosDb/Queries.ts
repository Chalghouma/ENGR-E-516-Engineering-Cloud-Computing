import { Container, CosmosClient, Resource, SqlQuerySpec } from "@azure/cosmos";
import { StoredItem } from "../../Models/StoredItem";
import { getCosmosDbClient } from "./Connector";
import { getContainer } from "./Database";

export const getStoredItemByKey = async (
  key: string,
  container: Container
): Promise<(Resource & StoredItem) | null> => {
  const query: SqlQuerySpec = {
    query: `SELECT * FROM c WHERE c.key = @key`,
    parameters: [{ name: "@key", value: key }],
  };
  const queryResult = await container.items.query(query);
  const feedResponse = await queryResult.fetchAll();
  const resources = feedResponse.resources;
  return resources !== null && resources.length === 1 ? resources[0] : null;
};

export const getItemByKey = async (
  key: string
): Promise<(Resource & StoredItem) | null> => {
  const cosmosClient = getCosmosDbClient();
  const container = await getContainer(cosmosClient);

  const query: SqlQuerySpec = {
    query: `SELECT * FROM c WHERE c.key = @key`,
    parameters: [{ name: "@key", value: key }],
  };
  const queryResult = await container.items.query(query);
  const feedResponse = await queryResult.fetchAll();
  const resources = feedResponse.resources;
  return resources !== null && resources.length === 1 ? resources[0] : null;
};

export const getAllItems = async (): Promise<Resource[] | null> => {
  const cosmosClient = getCosmosDbClient();
  const container = await getContainer(cosmosClient);

  const query: SqlQuerySpec = {
    query: `SELECT * FROM c`,
    parameters: [],
  };
  const queryResult = await container.items.query(query);
  const feedResponse = await queryResult.fetchAll();
  const resources = feedResponse.resources;
  return resources;
};
