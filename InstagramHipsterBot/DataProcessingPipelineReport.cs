using System;
using System.Collections.Generic;

namespace InstagramHipsterBot
{
    public class DataProcessingPipelineReport
    {
        public CustomVisionResponse ImageAnalysis { get; set; }

        public IEnumerable<string> Hashtags { get; set; }

        public string Username { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid Id { get; set; }
    }
}
