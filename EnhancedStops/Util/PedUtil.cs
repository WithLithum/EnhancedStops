// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStops.Util
{
    /// <summary>
    /// An enumeration of alcohol level abstractions.
    /// </summary>
    public enum PedAlcoholLevel
    {
        /// <summary>
        /// Absolutely no alcohol (<c>0mg/100ml</c>).
        /// </summary>
        None,
        /// <summary>
        /// Has a little amount of alcohol but not exceeding any limit (greater than zero but lesser than <c>20mg/100ml</c> per Chinese standard).
        /// </summary>
        Normal,
        /// <summary>
        /// The alcohol limit exceeds the summary offense limit
        /// (greater than / equals to <c>20mg/100ml</c> but lower than <c>80mg/100ml</c> per Chinese standard)
        /// </summary>
        SummaryOffense,
        /// <summary>
        /// Double of <see cref="SummaryOffense"/> (e.g. <c>40mg/100ml</c> per Chinese standard).
        /// </summary>
        DoubleSummaryOffense,
        /// <summary>
        /// Over <see cref="DoubleSummaryOffense"/> but lower than felony level (<c>80mg/100ml</c> per Chinese standard).
        /// </summary>
        OverDoubleSummaryOffense,
        /// <summary>
        /// Exceeds felony limit (greater than or equals to <c>80mg/100ml</c> per <a href="https://www.spp.gov.cn/flfg/sfjs/201312/t20131227_66040.shtml">Chinese standard</a>).
        /// </summary>
        Indictable,
        /// <summary>
        /// Twice times higher than felony limit.
        /// </summary>
        DoubleIndictable,
        /// <summary>
        /// Three times higher than felony limit.
        /// </summary>
        TripleIndictable,
        /// <summary>
        /// The suspect failed to provide a breath sample.
        /// </summary>
        Failed
    }

    internal static class PedUtil
    {
        private static bool _breathalyzeProgress;
        private static readonly Dictionary<Ped, float> _readings = new Dictionary<Ped, float>();
        private static readonly Dictionary<Ped, PedAlcoholLevel> _levels = new Dictionary<Ped, PedAlcoholLevel>();

        internal static PedAlcoholLevel GetPedAlcoholLevelSlient(Ped ped)
        {
            if (!_levels.ContainsKey(ped))
            {
                _levels.Add(ped, GetRandomLevel());
            }

            return _levels[ped];
        }

        internal static void OverridePedAlcoholLevel(Ped ped, PedAlcoholLevel lvl)
        {
            _levels[ped] = lvl;

            _readings[ped] = GetRandomReading(lvl);
        }

        internal static void Breathalyze(Ped ped)
        {
            if (ped == null) throw new ArgumentNullException(nameof(ped));
            if (!ped.IsValid()) throw new ArgumentException("Invalid ped", nameof(ped));

            if (_breathalyzeProgress)
            {
                return;
            }

            _breathalyzeProgress = true;
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon("WEAPON_UNARMED", 1, true);

            var level = GetPedAlcoholLevelSlient(ped);

            if (!_readings.ContainsKey(ped))
            {
                _readings.Add(ped, GetRandomReading(level));
            }

            GameFiber.StartNew(() =>
            {
                if (!ped)
                {
                    Game.DisplayNotification("There were some problems breathalyzing the suspect.~n~Is the suspect still there?");
                    return;
                }

                NativeFunction.Natives.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON("STRING");
                NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Breathalyzing");
                NativeFunction.Natives.END_TEXT_COMMAND_BUSYSPINNER_ON(0);

                if (ped.IsInAnyVehicle(false))
                {
                    Game.LocalPlayer.Character.Tasks.PlayAnimation("amb@code_human_police_investigate@idle_b", "idle_e", 2f, 0);
                    if (!ped.CurrentVehicle.IsBike)
                    {
                        ped.Tasks.PlayAnimation("amb@incar@male@smoking_low@idle_a", "idle_a", 2f, 0);
                    }
                    GameFiber.Sleep(2000);
                }
                else
                {
                    ped.Tasks.PlayAnimation("switch@michael@smoking", "michael_smoking_loop", 2f, AnimationFlags.SecondaryTask).WaitForCompletion(8000);
                    Game.LocalPlayer.Character.Tasks.Clear();
                }


                GameFiber.Sleep(3000);

                Game.LocalPlayer.Character.Tasks.Clear();
                NativeFunction.Natives.BUSYSPINNER_OFF();

                var off = "~g~was not under influence";

                if (_readings[ped] >= Config.IndictableBac)
                {
                    off = "~r~was drunk";
                }
                else if (_readings[ped] >= Config.SummaryOffenseBac)
                {
                    off = "~y~is a little dizzy";
                }

                var read = _readings[ped];

                if (read == -1)
                {
                    Game.DisplayNotification("The suspect failed to provide a valid sample.");
                }
                else
                {
                    Game.DisplayNotification("commonmenu", "mp_specitem_heroin",
                        "Breathalyzer",
                        "Test Result",
                        $"Reading: ~b~{read}~y~{Config.BacUnit}~s~~n~The suspect {off}");
                }

                _breathalyzeProgress = false;
            }, "ES Breathalyzer");
        }

        internal static float GetRandomReading(PedAlcoholLevel level)
        {
            var rnd = new Random();
            var dlb = rnd.NextDouble();
            Game.LogTrivial("ES: Normalized BAC: " + dlb);
            switch (level)
            {
                default:
                    return 0f;

                case PedAlcoholLevel.Normal:
                    return (float)MathHelper.Clamp(dlb * Config.SummaryOffenseBac, 1, Config.SummaryOffenseBac - 0.02f);

                case PedAlcoholLevel.SummaryOffense:
                    return (float)(rnd.NextDouble() * (Config.SummaryOffenseBac));

                case PedAlcoholLevel.DoubleSummaryOffense:
                    return (float)(rnd.NextDouble() * (Config.SummaryOffenseBac * 2));

                case PedAlcoholLevel.OverDoubleSummaryOffense:
                    return (float)MathHelper.Clamp(dlb * (Config.SummaryOffenseBac * 2.5f), Config.SummaryOffenseBac * 2, Config.IndictableBac - 0.02f);

                case PedAlcoholLevel.Indictable:
                    return (float)MathHelper.Clamp(dlb * (Config.IndictableBac), Config.IndictableBac, (Config.IndictableBac * 2) - 0.02f);

                case PedAlcoholLevel.DoubleIndictable:
                    return (float)MathHelper.Clamp(dlb * (Config.IndictableBac * 2.5), Config.IndictableBac * 2, (Config.IndictableBac * 4) - 0.02f);

                case PedAlcoholLevel.TripleIndictable:
                    return (float)MathHelper.Clamp(dlb * (Config.IndictableBac * 4.5), Config.IndictableBac * 4, (Config.IndictableBac * 10) - 0.02f);

                case PedAlcoholLevel.Failed:
                    return -1;
            }
        }

        internal static PedAlcoholLevel GetRandomLevel()
        {
            var rnd = new Random();
            var t = rnd.Next(80);

            Game.LogTrivial($"ES: Chance by: {t}");

            if (t <= 50)
            {
                return PedAlcoholLevel.None;
            }

            if (t >= 50 && t <= 58)
            {
                return PedAlcoholLevel.Normal;
            }

            if (t > 58 && t < 68)
            {
                var limitOne = new PedAlcoholLevel[]
                {
                    PedAlcoholLevel.SummaryOffense,
                    PedAlcoholLevel.DoubleSummaryOffense,
                    PedAlcoholLevel.OverDoubleSummaryOffense
                };

                return limitOne[rnd.Next(3)];
            }

            if (t > 68 && t < 75)
            {
                var limitTwo = new PedAlcoholLevel[]
                {
                    PedAlcoholLevel.Indictable,
                    PedAlcoholLevel.DoubleIndictable,
                    PedAlcoholLevel.TripleIndictable
                };

                return limitTwo[rnd.Next(3)];
            }

            if (t >= 75)
            {
                return PedAlcoholLevel.Failed;
            }

            return PedAlcoholLevel.None;
        }
    }
}
