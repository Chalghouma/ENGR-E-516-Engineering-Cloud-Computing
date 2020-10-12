import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { forEach, indexOf, keys } from "lodash";
import { StoredItem } from "../Models/StoredItem";
import { appendValueByKey } from "../Services/CosmosDb/Mutations";

interface BagOfWords {
  [word: string]: number;
}
export interface WordOccurence {
  occurence: number;
  index: number;
}
const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  const index = req.body.index as number;
  const textContent = req.body.content as string;
  const lines = textContent.match(/[^\r\n]+/g);
  const dictionary: BagOfWords = {};
  forEach(lines, (line) => {
    const words = line.split(" ");
    forEach(words, (word) => {
      const dictionaryKeys = keys(dictionary);
      const containsKey = indexOf(dictionaryKeys, word) !== -1;
      if (!containsKey) dictionary[word] = 1;
      else dictionary[word]++;
    });
  });

  const promises: any[] = [];
  const dictionaryKeys = keys(dictionary);
  forEach(dictionaryKeys, async (dictionaryKey) => {
    let error = null;
    do {
      try {
        promises.push( appendValueByKey(dictionaryKey,{
            occurence: dictionary[dictionaryKey],
            index,
          }));
        error = null;
      } catch (err) {
        error = err;
      }
    } while (error !== null);
  });
  await Promise.all(promises);

  context.res = {
    // status: 200, /* Defaults to 200 */
    body: dictionaryKeys,
  };
};

export default httpTrigger;
