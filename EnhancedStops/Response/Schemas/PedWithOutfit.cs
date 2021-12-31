// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

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
