// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using System.Collections.Generic;
using LSPD_First_Response.Engine.Scripting.Entities;
using Rage;

namespace EnhancedStops.Contrabanding
{
    internal static class ContrabandManager
    {
        private static readonly List<Ped> AlreadyApplied = new List<Ped>();

        internal static void Initialize()
        {
            GameFiber.StartNew(UpdateFiber);
        }

        internal static void RandomApply(Ped ped)
        {
            if (AlreadyApplied.Contains(ped) || !ped) return;

            if (MathHelper.GetRandomInteger(8) == 5)
            {
                var length = MathHelper.GetRandomInteger(2);
                for (int i = 0; i < length; i++)
                {
                    var type = MathHelper.GetRandomInteger(3) == 1 ? ContrabandType.Narcotics : ContrabandType.Weapon;
                    ContrabandsLoader.ApplyToPed(ped, type);
                }
            }

            if (MathHelper.GetRandomInteger(5) > 2)
            {
                var length = MathHelper.GetRandomInteger(2);
                for (int i = 0; i < length; i++)
                {
                    var type = MathHelper.GetRandomInteger(5) == 1 ? ContrabandType.Weapon : ContrabandType.Misc;
                    ContrabandsLoader.ApplyToPed(ped, type);
                }
            }
        }

        internal static void UpdateFiber()
        {
            while (!Main.Finalizing)
            {
                GameFiber.Sleep(100);
                for (int i = 0; i < AlreadyApplied.Count; i++)
                {
                    var ped = AlreadyApplied[i];
                    if (!ped)
                    {
                        AlreadyApplied.RemoveAt(i);
                    }
                }
            }
        }
    }
}
