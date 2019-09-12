
using System;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using Newtonsoft.Json;

namespace MyTelegramBot.Data.Work
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
            string proxy = null)
        {
            WebClient web = new WebClient();
            web.Proxy = new WebProxy(proxy);

            var data_byte = await web.DownloadDataTaskAsync(url);
            var data_str = System.Text.Encoding.UTF8.GetString(data_byte);

            return JsonConvert.DeserializeObject<T>(data_str);
        }
        public async Task<byte[]> SendDataAsync(
            T entity,
            string url,
            string proxy = null) 
        {
            WebClient web = new WebClient();            
            await LogInformation($"Using proxy {proxy}");
            web.Proxy = new WebProxy(proxy);
            var parameters = await GetParameters(entity);
            
            await LogInformation($"Was send url: {url} with count of parameters: {parameters.Count}");
            return await web.UploadValuesTaskAsync(url, parameters);
        }
        private async Task<NameValueCollection> GetParameters(T entity) {
            var parameters = new NameValueCollection();
            Type type = entity.GetType();

            foreach(PropertyInfo property in type.GetProperties()) 
            {
                await LogInformation($"Added {property.Name} " +$"with value: {property.GetValue(entity, null)?.ToString()}");

                parameters.Add(
                    property.Name, 
                    property.GetValue(entity, null)?.ToString()
                    /*  property.PropertyType.ToString().StartsWith("System") || property.GetValue(entity, null) == null ?
                        property.GetValue(entity, null)?.ToString() :
                        JsonConvert.SerializeObject(property.GetValue(entity, null)) */
                );
            }

            return parameters;
        }
        public Task<T> GetDataAsync(string urlApi, string[] parameters, string proxy)
        {
            throw new Exception();
            //url = url.Replace("pair", pair);
            // .Replace("&limit=count", limit != "" ? $"&limit={limit}" : "");
        }

        public async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
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
