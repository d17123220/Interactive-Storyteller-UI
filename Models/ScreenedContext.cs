namespace Interactive_Storyteller_UI.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ScreenedContext
    {
        [JsonPropertyName("originalText")]
        public string OriginalText { get; set; }

        [JsonPropertyName("correctedText")]
        public string CorrectedText { get; set; }
        
        [JsonPropertyName("offensiveTerms")]
        public ISet<string> OffensiveTerms { get; set; }

        public bool IsBounced { get; set; }

    }
}