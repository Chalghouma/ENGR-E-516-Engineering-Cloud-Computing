import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import { getCosmosDbClient } from "../Services/CosmosDb/Connector";
import { getContainer } from "../Services/CosmosDb/Database";
import { forEach } from "lodash";
import { getAllItems } from "../Services/CosmosDb/Queries";
import { Container } from "@azure/cosmos";
const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  const container = await getContainer(getCosmosDbClient());
  const resources = await getAllItems();
  const promises: any[] = [];
  forEach(resources, async (resource) => {
    let error = null;
    do {
      try {
        promises.push(deleteItem(container, resource.id));
        error = null;
      } catch (err) {
        error = err;
      }
    } while (error !== null);
  });
  await Promise.all(promises);
  context.res = {
    // status: 200, /* Defaults to 200 */
  };
};
const deleteItem = async (container: Container, id: string) => {
  const item = await container.item(id as string, true);
  await item.delete();
};
export default httpTrigger;
