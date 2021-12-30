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
            SuperSprintKey = iniFile.ReadEnum("Generic", "SuperSprintKey", Keys.Enter);
            SuperSprintCooldown = iniFile.ReadInt32("SuperSprint", "SuperSprintCooldown", 10);
            SuperSpringTimeout = iniFile.ReadInt32("SuperSprint", "SuperSprintTimeout", 3);

            TransportUnit = JsonConvert.DeserializeObject<Unit>(
                    File.ReadAllText("Data\\WithLithum\\EnhancedStops\\units\\transport.json")
                );
        }

        internal static Keys MenuKey { get; }
        internal static Keys SuperSprintKey { get; }
        internal static int SuperSprintCooldown { get; }
        internal static int SuperSpringTimeout { get; }
        internal static Unit TransportUnit { get; }
    }
}
