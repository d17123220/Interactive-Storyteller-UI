namespace Interactive_Storyteller_UI.Models
{
    
    using System.Text.Json.Serialization;

    public class Session
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sessionID")]
        public string SessionID { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("sessionPassword")]
        public string Password { get; set; }
    }

}