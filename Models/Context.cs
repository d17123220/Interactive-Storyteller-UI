namespace Interactive_Storyteller_UI.Models
{

    using System.Text.Json.Serialization;

    public class Context
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sessionID")]
        public long SessionID { get; set; }

        [JsonPropertyName("context")]
        public string SessionText { get; set; }

        [JsonPropertyName("contextCreator")]
        public string Creator { get; set; }

        [JsonPropertyName("contextSequence")]
        public long SequenceNumber { get; set; }

    }

}