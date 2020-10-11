using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    public class RemoteKeystoreExecuter
    {
        public async static Task<T> GetKey<T>(string key, string baseUrl)
        {
            string url = $"{baseUrl}/GetValueFunction";
            return await RestClient.PostJson<T>(new { key = key }, baseUrl);
        }
        public async static Task<string> SetKey<T>(string key, object objectToBeStored, string baseUrl)
        {
            string url = $"{baseUrl}/StoreValueFunction";
            return (await RestClient.PostJson<dynamic>(new { key = key, value = objectToBeStored }, url)).key;
        }
        public async static Task<string> AppendKey<T>(string key, object objectToBeStored, string baseUrl)
        {
            string url = $"{baseUrl}/AppendValueFunction";
            return (await RestClient.PostJson<dynamic>(new { key = key, value = objectToBeStored }, url)).key;
        }
    }
}
