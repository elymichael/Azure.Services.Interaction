namespace Azure.Services.Interaction.RestClient.Helper
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Json serializer helper class.
    /// </summary>
    public static class JsonSerializerHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static StringContent SerializeRequest<T>(T content)
        {

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var stringContent = new StringContent(JsonConvert.SerializeObject(content, serializerSettings), Encoding.UTF8, "application/json");
            return stringContent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(responseStream);
            using var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();

            T content = serializer.Deserialize<T>(jsonReader);

            return content;
        }
    }
}
