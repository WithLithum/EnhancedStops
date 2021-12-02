using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStops.Util
{
    internal static class Radio
    {
        internal static void DisplayConversation(string owner, string text)
        {
            Game.DisplayNotification($"~b~{owner}~s~: {text}");
        }

        internal static string GetPlayerName()
        {
            return Functions.GetPersonaForPed(Game.LocalPlayer.Character).FullName;
        }

        internal static string GetLicenseStateString(ELicenseState state)
        {
            switch (state)
            {
                default:
                    return "~r~Unknown";
                case ELicenseState.Expired:
                    return "~r~Expired";
                case ELicenseState.Suspended:
                    return "~r~Suspended";
                case ELicenseState.Unlicensed:
                    return "~r~No license";
                case ELicenseState.Valid:
                    return "~g~Valid";
            }
        }

        internal static string GetWantedString(bool wanted)
        {
            return wanted ? "~r~Wanted" : "~g~No warrants";
        }
    }
}
