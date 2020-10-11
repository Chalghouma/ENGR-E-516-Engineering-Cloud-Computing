import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { forEach, keys, indexOf } from "lodash";
import { appendValueByKey } from "../Services/CosmosDb/Mutations";
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

  const dictionaryKeys = keys(dictionary);
  forEach(dictionaryKeys, async (dictionaryKey) => {
    let error = null;
    do {
      try {
        await appendValueByKey(dictionaryKey, dictionary[dictionaryKey]);
      } catch (err) {
        error = err;
      }
    } while (error !== null);
  });

  context.res = {
    // status: 200, /* Defaults to 200 */
    body: dictionaryKeys,
  };
};

export default httpTrigger;
