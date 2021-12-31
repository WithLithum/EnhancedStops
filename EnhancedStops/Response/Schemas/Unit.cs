// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using Newtonsoft.Json;
using System.Collections.Generic;

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
