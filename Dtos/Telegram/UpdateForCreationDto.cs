using System.Collections.Generic;

namespace MyTelegramBot.Dtos.Telegram
{
    public class UpdateForCreationDto
    {
        public bool ok { get; set; }
        public List<ResultDto> result { get; set; }
    }
}