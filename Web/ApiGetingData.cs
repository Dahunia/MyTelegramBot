
using System;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Settings;
using Newtonsoft.Json;

namespace MyTelegramBot.Web
{
    public class ApiGetingData<T>
    {
        private readonly IMyLogger _filelogger;
        private readonly ILogger _logger;
        public ApiGetingData(ILogger logger, IMyLogger filelogger)
        {
            _logger = logger;
            _filelogger = filelogger;
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
                response = await web.UploadStringTaskAsync(url, "POST", jsonData);
            } catch(Exception ex) {
                await LogInformation("Error send: " + ex.Message);
                //throw new Exception(ex.Message);
            }
            finally {
                await LogInformation($"Using proxy {proxy}");
                await LogInformation($"SEND TO USER JSON SERIALIZED DATA {jsonData}");
            }
            //await LogInformation($"Sent to url: {url} with count of parameters: {parameters.Count}");
            return response;
        }//return await web.UploadValuesTaskAsync(url, parameters);
        public Task<T> GetDataAsync(string urlApi, string[] parameters, string proxy)
        {
            throw new Exception();
        }
        public async Task LogInformation(string message) 
        {
            _logger?.LogInformation(message);
            await _filelogger?.WriteInformationAsync(message);
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
                    await LogInformation($"Parameter: name->'{property.Name}', " + 
                    $"value->'{property.GetValue(entity, null)?.ToString()}'");
                } else { 
                await LogInformation($"Parameter: name->{property.Name}, " +
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
