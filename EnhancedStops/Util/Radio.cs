using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage;
using System;

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

        internal static void DisplayVehicleInfo(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (!vehicle.IsValid()) throw new ArgumentException("Invalid vehicle!", nameof(vehicle));

            string vehicleStat = "~r~Undefined";
            string vehicleInsurance = "~r~Undefined";

            if (vehicle.Metadata.Registry == null)
            {
                vehicle.Metadata.Registry = MathHelper.GetRandomInteger(2, 12) > 5;
            }

            if (vehicle.Metadata.Insurance == null)
            {
                vehicle.Metadata.Insurance = MathHelper.GetRandomInteger(2, 12) > 5;
            }

            vehicleStat = vehicle.Metadata.Registry ? "~g~Valid" : "~r~Invalid";
            vehicleInsurance = vehicle.Metadata.Insurance ? "~g~Valid" : "~r~Invalid";

            var info = Functions.GetVehicleOwnerName(vehicle);

            Game.DisplayNotification("commonmenu", "shop_garage_icon_a", "Dispatch", "Vehicle Status", 
                $"~b~Owner: ~y~{info}~n~~b~Stolen: {GetYesNoString(vehicle.IsStolen)}~n~~b~Registration: {vehicleStat}~n~~b~Insurance: {vehicleInsurance}");
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

        internal static string GetYesNoString(bool value)
        {
            return value ? "~r~Yes" : "~g~No";
        }
    }
}
