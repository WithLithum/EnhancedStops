// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using EnhancedStops.Contrabanding;
using EnhancedStops.Util;
using LSPD_First_Response.Mod.API;
using Rage;
using System.IO;

[assembly: Rage.Attributes.Plugin("BEPIS", Author = "BEPIS", Description = "You know what I hate? That's BEPIS. The taste, the smell, the texture.")]

namespace EnhancedStops
{
    /// <inheritdoc />
    public class Main : Plugin
    {
       /// <summary>
        /// Gets a value indicating whether the plugin is finalizing.
        /// </summary>
        public static bool Finalizing { get; private set; }

        /// <inheritdoc />
        public override void Finally()
        {
            StopProcess.DisposePeds();
            Finalizing = true;
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
            ContrabandsLoader.LoadIn();
            GameFiber.StartNew(ContrabandManager.UpdateFiber, "EH contraband misc");

            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
            Events.OnPedArrested += this.Events_OnPedArrested;
            Events.OnPedStopped += this.Events_OnPedStopped;
        }

        private void Events_OnPedArrested(Ped suspect, Ped arrestingOfficer)
        {
            Events_OnPedStopped(suspect);
        }

        private void Events_OnPedStopped(Ped ped)
        {
            if (ped)
            {
                ContrabandManager.RandomApply(ped);
            }
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