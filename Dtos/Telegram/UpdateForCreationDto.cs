using System.Collections.Generic;

namespace MyTelegramBot.Dtos.Telegram
{
    public class UpdateForCreationDto
    {
        public bool ok { get; set; }
        public List<Result> result { get; set; }

        public override string ToString() {
            string toDisplay = "Update:\n";

            result.ForEach(update => {
                toDisplay += update.ToString();
            });

            return toDisplay;
        }
    }
}