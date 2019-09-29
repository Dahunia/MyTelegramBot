
using System;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using MyTelegramBot.Helpers;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Settings;
using Newtonsoft.Json;

namespace MyTelegramBot.Web
{
    public class ApiGetingData<T>
    {
        private readonly IMyLogger<ApiGetingData<T>> _logger;
        public ApiGetingData(IMyLogger<ApiGetingData<T>> logger)
        {
            _logger = logger;
        }

        public async Task<T> GetDataAsync(
            string url,
            Proxy proxy = null)
        {
            WebClient web = new WebClient();
            if (proxy != null) {
                web.Proxy = new WebProxy(proxy.Address);
                web.Proxy.Credentials = new NetworkCredential(
                    proxy.Username, 
                    proxy.Password
                );  
            }

            var data_byte = await web.DownloadDataTaskAsync(url);
            var data_str = System.Text.Encoding.UTF8.GetString(data_byte);

            return JsonConvert.DeserializeObject<T>(data_str);
        }
        public async Task<string> SendDataAsync(
            T entity,
            string url,
            Proxy proxy = null) 
        {
            WebClient web = new WebClient();  
            web.Headers[HttpRequestHeader.ContentType] = "application/json";
            //web.Encoding = System.Text.Encoding.UTF8;
            if (proxy != null)
            {
                web.Proxy = new WebProxy(proxy.Address);
                web.Proxy.Credentials = new NetworkCredential(
                    proxy.Username, 
                    proxy.Password
                );                                                          
            }
            //var parameters = await GetParameters(entity);
            var jsonData = JsonConvert.SerializeObject(
                entity 
                ,Formatting.None
                ,new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
                
            string response = "";
            try {
                await _logger.LogInformation($"Using proxy {proxy.Address.ToString()}");
                await _logger.LogInformation($"SEND TO USER JSON SERIALIZED DATA {jsonData}");
                response = await web.UploadStringTaskAsync(url, "POST", jsonData);
            } catch(Exception ex) {
                await _logger.LogInformation("Error send: " + ex.Message);
                response = ex.Message;
                //throw new Exception(ex.Message);
            }
            finally {
                await _logger.LogInformation($"SEND TO USER in the form of a OBJECT: {entity.GetDump()}");
            }
            //await LogInformation($"Sent to url: {url} with count of parameters: {parameters.Count}");
            return response;
        }//return await web.UploadValuesTaskAsync(url, parameters);
        public Task<T> GetDataAsync(string urlApi, string[] parameters, string proxy)
        {
            throw new Exception();
        }
        private async Task<NameValueCollection> GetParameters(T entity) {
            var parameters = new NameValueCollection();
            Type type = entity.GetType();

            foreach(PropertyInfo property in type.GetProperties()) 
            {
                if (//!property.GetType().Equals(typeof(object))
                    !property.PropertyType.ToString().StartsWith("System.Object")
                    &&
                    (property.PropertyType.ToString().StartsWith("System") 
                    || property.GetValue(entity, null) == null)) {
                    await _logger.LogInformation($"Parameter: name->'{property.Name}', " + 
                    $"value->'{property.GetValue(entity, null)?.ToString()}'");
                } else { 
                await _logger.LogInformation($"Parameter: name->{property.Name}, " +
                    $"value->{JsonConvert.SerializeObject(property.GetValue(entity, null))}");
                }
                parameters.Add(
                    property.Name, 
                    //property.GetValue(entity, null)?.ToString()
                    property.PropertyType.ToString().StartsWith("System") || property.GetValue(entity, null) == null ?
                    property.GetValue(entity, null)?.ToString() :
                    JsonConvert.SerializeObject(property.GetValue(entity, null))
                );
            }
            return parameters;
        }
    }
}
/* 
if (property.PropertyType.ToString().StartsWith("System") || property.GetValue(entity, null) == null) {
    _logger.LogInformation($"Added {property.Name} " +
        $"with value: {property.GetValue(entity, null)?.ToString()}");
} else {
    _logger.LogInformation($"Added {property.Name} " +
        $"with value: {JsonConvert.SerializeObject(property.GetValue(entity, null))}");
} */
