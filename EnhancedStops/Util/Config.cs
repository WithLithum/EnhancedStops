// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using EnhancedStops.Response.Schemas;
using Newtonsoft.Json;
using Rage;
using System.IO;
using System.Windows.Forms;

namespace EnhancedStops.Util
{
    internal static class Config
    {
        private static readonly InitializationFile iniFile = new InitializationFile(@"plugins\LSPDFR\EnhancedStops.ini");

        static Config()
        {
            Directory.CreateDirectory(@"Data\WithLithum\EnhancedStops\");
            MenuKey = iniFile.ReadEnum("Generic", "MenuKey", Keys.G);

            TransportUnit = JsonConvert.DeserializeObject<Unit>(
                    File.ReadAllText("Data\\WithLithum\\EnhancedStops\\units\\transport.json")
                );
        }

        internal static Keys MenuKey { get; }
        internal static Unit TransportUnit { get; }
    }
}
