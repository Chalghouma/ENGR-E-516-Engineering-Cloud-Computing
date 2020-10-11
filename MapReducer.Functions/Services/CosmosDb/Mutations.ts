import { StoredItem } from "../../Models/StoredItem";
import { getCosmosDbClient } from "./Connector";
import { getContainer } from "./Database";
import { getStoredItemByKey } from "./Queries";

export const appendValueByKey = async (key: string, value: any) => {
  const cosmosClient = getCosmosDbClient();
  const container = await getContainer(cosmosClient);

  let storedItem = await getStoredItemByKey(key, container);
  if (storedItem == null) {
    storedItem = (
      await container.items.create<StoredItem>(new StoredItem(key, [value]))
    ).resource;
  } else {
    storedItem.value = [...storedItem.value, value];
    await container.item(storedItem.id).replace(storedItem);
  }

  return storedItem;
};

export const storeValueByKey = async(key:string,value:any)=>{
    
  const cosmosClient = getCosmosDbClient();
  const container = await getContainer(cosmosClient);

  const storedItem = new StoredItem(key, value);
  const createdItem = await container.items.create<StoredItem>(storedItem);
  return createdItem.resource;
};