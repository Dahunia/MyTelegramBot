using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyTelegramBot.Models.Settings;
using MyTelegramBot.Interface;
using Microsoft.Extensions.Options;

namespace MyTelegramBot.Log
{

    public class FileReceiver : IReceiver
    {
        public readonly string _fileName;

        public FileReceiver(IOptions<FilePaths> _appConfig) {
            _fileName = _appConfig.Value.FileLoggerName
                .Replace("date", System.DateTime.Now.ToString("yyyy-MM-dd"));
        }
        public FileReceiver(string fileName) => _fileName = fileName;
        public string Read()
        {
            throw new System.NotImplementedException();
        }

        public void Write(string info)
        {
            //Encoding.RegisterProvider(CodepPagesEncodingProvider.Instance);
            //Encoding.GetEncoding("windows-1251");
            using(var sw = new StreamWriter(_fileName, true, Encoding.UTF8))
            {
                sw.WriteLine(info);
            }
        }
        public async Task WriteAsync(string info)
        {
            //Encoding.RegisterProvider(CodepPagesEncodingProvider.Instance);
            //Encoding.GetEncoding("windows-1251");
            if (!File.Exists(_fileName) ) 
            {
                using(var sw = new StreamWriter(_fileName, true, Encoding.UTF8))
                {
                    await sw.WriteLineAsync(info);
                }
            }
            else 
            {
                string tempfile = Path.GetTempFileName();

                using(var writer = new StreamWriter(tempfile, true, Encoding.UTF8))
                {
                    using(var reader = new StreamReader(_fileName))
                    {
                        await writer.WriteAsync(info);
                        await writer.WriteAsync( await reader.ReadToEndAsync() );
                    }
                }
                
                File.Copy(tempfile, _fileName, true);
            }
        }

    }
}