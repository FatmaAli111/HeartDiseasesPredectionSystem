using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class FlaskResponseDto
    {
        [JsonPropertyName("metrics_analysis")]
        public Dictionary<string, MetricInfo> MetricsAnalysis { get; set; }

        [JsonPropertyName("predictions")]
        public PredictionValues Predictions { get; set; }
    }
}
