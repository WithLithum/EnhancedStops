using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStops.Response.Schemas
{
    public class Unit
    {
        [JsonProperty("vehicles")]
        public List<string> Vehicles { get; set; }

        [JsonProperty("peds")]
        public List<PedWithOutfit> Peds { get; set; }
    }
}
