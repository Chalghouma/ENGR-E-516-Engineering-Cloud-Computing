import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { forEach, map } from "lodash";
import { WordOccurence } from "../InvertedIndexMapper";
import { storeValueByKey } from "../Services/CosmosDb/Mutations";
import { getMergedItemByKey } from "../Services/CosmosDb/Queries";

interface OccurenceDictionary {
  [documentIndex: number]: number;
}
const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  const occurencesData = (await getMergedItemByKey(req.body.key))
    .value as WordOccurence[];

  const dictionary: OccurenceDictionary = {};
  const indexes = map(occurencesData, (occurence) => occurence.index);
  forEach(indexes, (index) => (dictionary[index] = 0));
  forEach(
    occurencesData,
    (occurenceData) =>
      (dictionary[occurenceData.index] += occurenceData.occurence)
  );

  const key = req.body.key;
  const outputKey = key + "_REDUCER_OUTPUT";
  const storedItem = await storeValueByKey(
    key + "_REDUCER_OUTPUT",
    map(indexes, (index) => {
      return {
          index,
          occurence:dictionary[index]
      };
    })
  );
  context.res = {
    // status: 200, /* Defaults to 200 */
    body: { key: outputKey },
  };
};

export default httpTrigger;
