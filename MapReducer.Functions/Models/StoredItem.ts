export class StoredItem {
  key: string;
  value: any;
  partitionKey: true;
  constructor(key: string, value: any) {
    this.key = key;
    this.value = value;
    this.partitionKey = true;
  }
}
