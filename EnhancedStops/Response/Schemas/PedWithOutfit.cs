using Newtonsoft.Json;
using System.Collections.Generic;

namespace EnhancedStops.Response.Schemas
{
    public class PedWithOutfit
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("freemode")]
        public bool Freemode { get; set; }

        [JsonProperty("outfit")]
        public Dictionary<string, string> Outfit { get; set; }
    }
}
