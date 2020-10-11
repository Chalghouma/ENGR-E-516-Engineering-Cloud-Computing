import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { StoredItem } from "../Models/StoredItem";
import { getCosmosDbClient } from "../Services/CosmosDb/Connector";
import { getContainer } from "../Services/CosmosDb/Database";
import { getMergedItemByKey } from "../Services/CosmosDb/Queries";

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  let storedItem = await getMergedItemByKey(req.body.key);
  context.res = {
    body: storedItem as StoredItem,
  };
};

export default httpTrigger;
