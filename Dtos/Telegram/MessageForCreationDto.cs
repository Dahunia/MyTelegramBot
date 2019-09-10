using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace MyTelegramBot.Dtos.Telegram
{
    public class MessageForCreationDto// : IOutboundParameterTransformer
    {
        public long update_id { get; set; }
        public Message message { get; set; }

        public override string ToString()
        {
            return $"\nupdate_id: {update_id}\n" +
                message.ToString();
        }

/*         public string TransformOutbound(object value)
        {
            var result = value == null ? 
                null :
                Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
            return result;
        } */
    }
}