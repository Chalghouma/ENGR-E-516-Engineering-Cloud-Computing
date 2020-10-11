import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { reduce, map } from "lodash";

interface KeyValuePairInput {
  Key: string;
  Value: number;
}

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  const items = req.body as KeyValuePairInput[];
  const occurences = map(items, (item) => item.Value);
  const sum = reduce(occurences, (accumulated, current) => accumulated + current);
  context.res = {
    // status: 200, /* Defaults to 200 */
    body: { Key: items[0].Key, Value: sum },
  };
};

export default httpTrigger;
