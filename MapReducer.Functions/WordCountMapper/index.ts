import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { forEach, keys, indexOf } from "lodash";
const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  const inputLine = req.body.inputLine as string;
  const words = inputLine.split(" ");
  const dictionary = {};
  forEach(words, (word) => {
    const dictionaryKeys = keys(dictionary);
    const containsKey = indexOf(dictionaryKeys, word) !== -1;
    if (!containsKey) dictionary[word] = 1;
    else dictionary[word]++;
  });

  context.res = {
    // status: 200, /* Defaults to 200 */
    body: dictionary,
  };
};

export default httpTrigger;
