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
            var resultStringContent = await requestResult.Content.ReadAsStringAsync();
            if (!requestResult.IsSuccessStatusCode)
            {
                throw new Exception($"Unsuccessful PostJson request. StatusCode {requestResult.StatusCode}. Result:{resultStringContent}");
            }
            var deserialized = JsonConvert.DeserializeObject<TOutput>(resultStringContent);
            return deserialized;
        }
    }
}
