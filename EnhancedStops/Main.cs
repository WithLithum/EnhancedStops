using EnhancedStops.Util;
using LSPD_First_Response.Mod.API;
using Rage;
using System.IO;

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
                Game.DisplayNotification("You may experience ~r~glitches~s~ or other errors when playing.");
                Game.DisplayNotification("We recommend you to remove ~b~UltimateBackup~s~ for better experience.");
                Game.LogTrivial("EH: Stop the ped Detected. The plugin is not loading!");
                return;
            }

            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        private void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            {
                StopProcess.Initialize();
                SprintProcess.Init();
            }
        }
    }
}
