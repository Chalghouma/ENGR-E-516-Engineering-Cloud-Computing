import { BlobServiceClient, AnonymousCredential } from "@azure/storage-blob";

const STORAGE_ACCOUNT_NAME = "engrstorageaccount";
const STORAGE_SAS_TOKEN =
  "?sv=2019-12-12&ss=bfqt&srt=sco&sp=rwdlacupx&se=2020-10-24T02:49:46Z&st=2020-10-23T18:49:46Z&spr=https,http&sig=o1RF6%2FNVSvkM3%2FWQ%2FnB3vAHAKpRmwkn4Zl0PKjVkn2Y%3D";
export class BlobStorageService {
  private client: BlobServiceClient | null;
  constructor() {
    this.client = null;
    this.init();
  }
  private init() {
    const url = `https://${STORAGE_ACCOUNT_NAME}.blob.core.windows.net/${STORAGE_SAS_TOKEN}`;
    const anonymousCredentials = new AnonymousCredential();
    this.client = new BlobServiceClient(url, anonymousCredentials);
  }
  public async acquireLeaseClient(fileName: string) {
    if (!this.client)
      throw new Error("No BlobServiceClient has been initialized yet");
    const blobClient = this.client
      .getContainerClient("leases")
      .getBlockBlobClient(fileName);
    if (!(await blobClient.exists())) await blobClient.upload("0", 1);
    return blobClient.getBlobLeaseClient();
  }
}
export default new BlobStorageService();
