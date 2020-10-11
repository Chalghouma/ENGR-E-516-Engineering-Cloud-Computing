import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { StoredItem } from "../Models/StoredItem";
import { appendValueByKey } from "../Services/CosmosDb/Mutations";

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  context.res = {
    body: (await appendValueByKey(req.body.key, req.body.value)) as StoredItem,
  };
};

export default httpTrigger;
