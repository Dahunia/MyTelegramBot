using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
    {
    public class FromDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime LastActive { get; set; }
        public FromDto() {
            Created = System.DateTime.Now;
            LastActive = System.DateTime.Now;
        }
    }
}