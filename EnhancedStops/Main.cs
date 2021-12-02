using EnhancedStops.Util;
using LSPD_First_Response.Mod.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Rage.Attributes.Plugin("Enhanced Stops", Author = "WithLithum", Description = "You know what I hate? That's BEPIS. The taste, the smell, the texture.")]

namespace EnhancedStops
{
    public class Main : Plugin
    {
        public override void Finally()
        {
            TrafficProcess.Dispose();
            StopProcess.DisposePeds();
        }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        private void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            {
                StopProcess.Initialize();
            }
        }
    }
}
