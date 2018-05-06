using System.Collections.Generic;

using Newtonsoft.Json;

namespace InstagramHipsterBot
{
    public class CustomVisionResponse
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Project")]
        public string Project { get; set; }

        [JsonProperty("Iteration")]
        public string Iteration { get; set; }

        [JsonProperty("Created")]
        public string Created { get; set; }

        [JsonProperty("Predictions")]
        public IEnumerable<Prediction> Predictions { get; set; }
    }

    public class Prediction
    {
        [JsonProperty("TagId")]
        public string TagId { get; set; }

        [JsonProperty("Tag")]
        public string Tag { get; set; }

        [JsonProperty("Probability")]
        public string Probability { get; set; }
    }
}
