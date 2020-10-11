import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { StoredItem } from "../Models/StoredItem";
import { getCosmosDbClient } from "../Services/CosmosDb/Connector";
import { getContainer } from "../Services/CosmosDb/Database";
import { appendValueByKey } from "../Services/CosmosDb/Mutations";
import { getStoredItemByKey } from "../Services/CosmosDb/Queries";

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  context.res = {
    body: (await appendValueByKey(req.body.key, req.body.value)) as StoredItem,
  };
};

export default httpTrigger;
