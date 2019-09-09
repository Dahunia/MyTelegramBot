using System.Collections.Generic;

namespace MyTelegramBot.Dtos
{
    public class MessageForCreationDto
    {        
        public long update_id { get; set; }
        public Message message { get; set; }

        public override string ToString() {
            return $"\nupdate_id: {update_id}\n" +
                message.ToString();
        }
    }
}