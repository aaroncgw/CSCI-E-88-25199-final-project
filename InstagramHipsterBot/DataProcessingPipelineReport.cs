using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstagramHipsterBot
{
    public class DataProcessingPipelineReport
    {
        [JsonProperty("ImageAnalysis")]
        public CustomVisionResponse ImageAnalysis { get; set; }

        [JsonProperty("Hashtags")]
        public IEnumerable<string> Hashtags { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("Id")]
        public Guid Id { get; set; }
    }
}
