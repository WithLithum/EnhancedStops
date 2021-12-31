// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using Rage;
using System.Drawing;

namespace EnhancedStops.Util
{
    internal static class TrafficProcess
    {
        private static Blip _trafficBlip;
        private static uint _handle;

        internal static void CleanUp()
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
