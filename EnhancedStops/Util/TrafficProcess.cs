using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStops.Util
{
    internal static class TrafficProcess
    {
        private static Blip _trafficBlip;
        private static Vector3 _stopArea;
        private static uint _handle;

        internal static void Dispose()
        {
            if (_trafficBlip)
            {
                _trafficBlip.Delete();
            }

            if (_handle != 0)
            {
                World.RemoveSpeedZone(_handle);
            }
        }

        internal static void CreateStopArea(Vector3 area)
        {
            _trafficBlip = new Blip(area, 85f)
            {
                Color = Color.Orange
            };

            _handle = World.AddSpeedZone(area, 85f, 11.9f);
        }
    }
}
