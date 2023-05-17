using System.Text.Json;
using System.Text.Json.Serialization;

namespace RESTcontrollers.Models
{
    public class Examination
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Eye")]
        public string Eye { get; set; } = "";
        [JsonPropertyName("Dioptry")]
        public double Dioptry { get; set; }
        [JsonPropertyName("Cylinder")]
        public double Cylinder { get; set; }
        [JsonPropertyName("Axis")]
        public int Axis { get; set; }
    }
}
