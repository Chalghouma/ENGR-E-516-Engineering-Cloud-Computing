using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    public class RestClient
    {
        public static async Task<TOutput> PostJson<TOutput>(dynamic objectToBeSent, string url)
        {
            var client = new HttpClient();
            var stringContent = new StringContent(JsonConvert.SerializeObject(objectToBeSent), Encoding.UTF8, "application/json");
            var requestResult = await client.PostAsync(url, stringContent);
            if (!requestResult.IsSuccessStatusCode)
                throw new Exception($"Unsuccessful PostJson request. StatusCode {requestResult.StatusCode}");
            var serializedResponse = await requestResult.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<TOutput>(serializedResponse);
            return deserialized;
        }
    }
}
