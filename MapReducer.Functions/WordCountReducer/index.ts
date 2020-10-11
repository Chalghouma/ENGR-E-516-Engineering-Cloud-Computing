import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { reduce, map } from "lodash";
import { storeValueByKey } from "../Services/CosmosDb/Mutations";
import { getMergedItemByKey } from "../Services/CosmosDb/Queries";

interface KeyValuePairInput {
  Key: string;
  Value: number;
}

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {

  
  const occurences = (await getMergedItemByKey(req.body.key)).value as number[];
  const sum = reduce(occurences, (accumulated, current) => accumulated + current);
  
  const key = req.body.key;
  
  const outputKey = key+"_REDUCER_OUTPUT";
  const storedItem = await storeValueByKey(key+"_REDUCER_OUTPUT",sum ); 
  context.res = {
    // status: 200, /* Defaults to 200 */
    body: { key:outputKey  },
  };
};

export default httpTrigger;
