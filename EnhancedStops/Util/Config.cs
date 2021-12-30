using EnhancedStops.Response.Schemas;
using Newtonsoft.Json;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
