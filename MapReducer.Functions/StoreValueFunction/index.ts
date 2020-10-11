import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { StoredItem } from "../Models/StoredItem";
import { getCosmosDbClient } from "../Services/CosmosDb/Connector";
import { getContainer } from "../Services/CosmosDb/Database";
import { storeValueByKey } from "../Services/CosmosDb/Mutations";

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  context.res = {
    body: {
      key: ((await storeValueByKey(req.body.key,req.body.value)) as StoredItem).key,
    },
  };
};

export default httpTrigger;
