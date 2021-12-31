// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Text;

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

            var info = VehicleUtil.QueryInformation(vehicle);

            _ = GameFiber.StartNew(() =>
            {
                GameFiber.Sleep(5000);

                var sb = new StringBuilder()
                    .Append("License Plate: ~b~")
                    .Append(info.LicensePlate)
                    .Append("~s~~n~Registration: ")
                    .Append(GetStatusString(info.Registration))
                    .Append("~s~~n~Insurance: ")
                    .Append(GetStatusString(info.Insurance))
                    .ToString();

                Game.DisplayNotification("commonmenu",
                    "shop_garage_icon_a",
                    "Dispatch",
                    "Vehicle Status",
                    sb);
            }, "Display Vehicle Information");
        }

        internal static void DisplayPedId(Persona persona)
        {
            _ = GameFiber.StartNew(() =>
            {
                Radio.DisplayConversation(Game.LocalPlayer.Name, $"Requesting check for ~y~{persona.FullName}, born {persona.Birthday.ToShortDateString()}");
                GameFiber.Sleep(2500);
                Radio.DisplayConversation("Dispatch", "Copy, stand by...");
                GameFiber.Sleep(MathHelper.GetRandomInteger(1500, 3000));
                Game.DisplayNotification("commonmenu", "shop_mask_icon_a", "Dispatch", "Ped Status",
                    $"~y~{persona.FullName}~s~, born {persona.Birthday.ToShortDateString()}~n~License: {Radio.GetLicenseStateString(persona.ELicenseState)}~s~~n~Wanted: {Radio.GetWantedString(persona.Wanted)}");
            }, "EnhancedStops Display Pedid");
        }

        internal static string GetStatusString(VehicleUtil.VehicleStatus status)
        {
            switch (status)
            {
                default:
                    return "~y~NOT FOUND";
                case VehicleUtil.VehicleStatus.Expired:
                    return "~r~EXPIRED";
                case VehicleUtil.VehicleStatus.Valid:
                    return "~g~VALID";
                case VehicleUtil.VehicleStatus.None:
                    return "~g~NONE";
            }
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
