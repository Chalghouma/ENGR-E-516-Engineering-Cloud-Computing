import { BlobServiceClient } from "@azure/storage-blob";
import { StoredItem } from "../../Models/StoredItem";
import { BlobStorageService } from "../BlobStorage/BlobStorageService";
import { getCosmosDbClient } from "./Connector";
import { getContainer } from "./Database";
import { getStoredItemByKey } from "./Queries";

export const appendValueByKey = async (key: string, value: any) => {
  const cosmosClient = getCosmosDbClient();
  const container = await getContainer(cosmosClient);

  const client = await new BlobStorageService().acquireLeaseClient(key);
  const lease = await client.acquireLease(15);
  if(lease.errorCode) throw new Error("Couldn't get lease");

  let storedItem = await getStoredItemByKey(key, container);
  if (storedItem == null) {
    storedItem = (
      await container.items.create<StoredItem>(new StoredItem(key, [value]))
    ).resource;
  } else {
    storedItem.value = [...storedItem.value, value];
    await container.item(storedItem.id).replace(storedItem);
  }
  await client.releaseLease();

  return storedItem;
};

export const storeValueByKey = async (key: string, value: any) => {
  const cosmosClient = getCosmosDbClient();
  const container = await getContainer(cosmosClient);

  const storedItem = new StoredItem(key, value);
  const client = await new BlobStorageService().acquireLeaseClient(key);
  const lease = await client.acquireLease(15);
  if(lease.errorCode) throw new Error("Couldn't get lease");
  const createdItem = await container.items.create<StoredItem>(
    new StoredItem(key, [value])
  );
  await client.releaseLease();
  //   const createdItem = await container.items.create<StoredItem>(storedItem);
  return createdItem.resource;
};
