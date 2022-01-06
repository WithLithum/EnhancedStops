// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using EnhancedStops.Util;
using LSPD_First_Response.Mod.API;
using Rage;
using System.IO;
using System.Reflection;

[assembly: Rage.Attributes.Plugin("BEPIS", Author = "BEPIS", Description = "You know what I hate? That's BEPIS. The taste, the smell, the texture.")]

namespace EnhancedStops
{
    /// <inheritdoc />
    public class Main : Plugin
    {
        /// <inheritdoc />
        public override void Finally()
        {
            StopProcess.DisposePeds();
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            if (File.Exists(@"plugins\LSPDFR\StopThePed.dll"))
            {
                Game.DisplayNotification("~r~~h~EnhancedStops Warning~n~~s~~r~Stop The Ped~s~ was detected. EnhancedStops will refrain from loading.");
                Game.DisplayNotification("~g~EnhancedStops~s~ is not compactible with ~r~Stop The Ped~s~. You must choose one.");
                Game.LogTrivial("EH: Stop the ped Detected. The plugin is not loading!");
                return;
            }

            if (File.Exists(@"plugins\LSPDFR\UltimateBackup.dll"))
            {
                Game.DisplayNotification("~r~~h~EnhancedStops Warning~n~~s~~r~UltimateBackup~s~ was detected. The latter's feature is not supported by EnhancedStops.");
                Game.DisplayNotification("~g~EnhancedStops~s~ is not compactible with ~r~Ultimate Backup~s~. You must choose one.");
                Game.LogTrivial("EH: Ultimate Backup Detected. The plugin is not loading!");
                return;
            }

            GameFiber.StartNew(Config.Init, "EH config");
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
