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
        private static readonly InitializationFile iniFile;

        static Config()
        {
            Directory.CreateDirectory(@"Data\WithLithum\EnhancedStops\");
            iniFile = new InitializationFile(@"plugins\LSPDFR\EnhancedStops.ini");

            MenuKey = iniFile.ReadEnum("Generic", "MenuKey", Keys.G);
            SuperSprintKey = iniFile.ReadEnum("Generic", "SuperSprintKey", Keys.Enter);
            SuperSprintCooldown = iniFile.ReadInt32("SuperSprint", "SuperSprintCooldown", 10);
            SuperSpringTimeout = iniFile.ReadInt32("SuperSprint", "SuperSprintTimeout", 3);

            TransportUnit = JsonConvert.DeserializeObject<Unit>(
                    File.ReadAllText("Data\\WithLithum\\EnhancedStops\\units\\transport.json")
                );
        }

        internal static Keys MenuKey { get; private set; }
        internal static Keys SuperSprintKey { get; private set; }
        internal static int SuperSprintCooldown { get; private set; }
        internal static int SuperSpringTimeout { get; private set; }
        internal static Unit TransportUnit { get; }
    }
}
