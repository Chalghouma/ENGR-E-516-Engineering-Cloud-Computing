import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { StoredItem } from "../Models/StoredItem";
import { getCosmosDbClient } from "../Services/CosmosDb/Connector";
import { getContainer } from "../Services/CosmosDb/Database";

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  const cosmosClient = getCosmosDbClient();
  const container = await getContainer(cosmosClient);

  const storedItem = new StoredItem(req.body.key, req.body.value);
  const createdItem = await container.items.create<StoredItem>(storedItem);
  context.res = {
    body: {
      key: createdItem.resource.key,
    },
  };
};

export default httpTrigger;
