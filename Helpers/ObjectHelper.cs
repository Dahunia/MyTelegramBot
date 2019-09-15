using Newtonsoft.Json;

namespace MyTelegramBot.Helpers
{
    public static class ObjectHelper
    {
        public static string GetDump<T>(this T x)
        {
            return JsonConvert.SerializeObject(x, Formatting.Indented);
        }
    }
}