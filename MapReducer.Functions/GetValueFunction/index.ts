import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { StoredItem } from "../Models/StoredItem";
import { getCosmosDbClient } from "../Services/CosmosDb/Connector";
import { getContainer } from "../Services/CosmosDb/Database";
import { getStoredItemByKey } from "../Services/CosmosDb/Queries";

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  const cosmosClient = getCosmosDbClient();
  const container = await getContainer(cosmosClient);

  let storedItem = await getStoredItemByKey(req.body.key, container);
  context.res = {
    body: storedItem as StoredItem,
  };
};

export default httpTrigger;
