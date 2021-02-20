namespace Interactive_Storyteller_UI.Models
{

    using System.Text.Json.Serialization;

    public class Context
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sessionID")]
        public string SessionID { get; set; }

        [JsonPropertyName("sessionText")]
        public string SessionText { get; set; }

        [JsonPropertyName("contextCreator")]
        public string ContextCreator { get; set; }

        [JsonPropertyName("sequenceNumber")]
        public long SequenceNumber { get; set; }

    }

}