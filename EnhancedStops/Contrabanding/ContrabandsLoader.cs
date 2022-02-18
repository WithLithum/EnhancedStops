// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using System.IO;
using EnhancedStops.Contrabanding.Entities;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Newtonsoft.Json;
using Rage;

namespace EnhancedStops.Contrabanding
{
    internal static class ContrabandsLoader
    {
        private static ContrabandsList list;
        internal const string DataLocation = "plugins\\LSPDFR\\EnhancedStopss\\Contrabands.json";
        internal static void ApplyToPed(Ped ped, ContrabandType type)
        {
            switch (type)
            {
                case ContrabandType.Contraband:
                    Functions.AddPedContraband(ped, type, list.Contrabands[MathHelper.GetRandomInteger(list.Contrabands.Length)]);
                    break;
                case ContrabandType.Misc:
                    Functions.AddPedContraband(ped, type, list.Items[MathHelper.GetRandomInteger(list.Items.Length)]);
                    break;
                case ContrabandType.Narcotics:
                    Functions.AddPedContraband(ped, type, list.Drugs[MathHelper.GetRandomInteger(list.Drugs.Length)]);
                    break;
                case ContrabandType.Weapon:
                    Functions.AddPedContraband(ped, type, list.Weapons[MathHelper.GetRandomInteger(list.Weapons.Length)]);
                    break;
            }
        }

        internal static void LoadIn()
        {
            if (File.Exists(DataLocation))
            {
                list = JsonConvert.DeserializeObject<ContrabandsList>(File.ReadAllText(DataLocation));
            }
            else
            {
                list = new ContrabandsList()
                {
                    Contrabands = new string[]
                    {
                        "Wire cutter",
                        "Ammunitions",
                        "Fake Police Badge",
                        "Hollow Point Rounds"
                    },
                    Drugs = new string[]
                    {
                        "LSD",
                        "Ketamine",
                        "Bag of white powder",
                        "Cocaine"
                    },
                    Weapons = new string[]
                    {
                        "Pepper Spray",
                        "Nightstick",
                        "Expandable Baton",
                        "Large Flashlight",
                        "Stun Gun",
                        "Automatic Pistol"
                    },
                    Items = new string[]
                    {
                        "Mobile Phone",
                        "Spray can",
                        "Hammer",
                        "Wrench",
                        "Dollar bill",
                        "Credit card",
                        "Postcard"
                    }
                };

                Directory.CreateDirectory("plugins\\LSPDFR\\EnhancedStops\\");
                File.WriteAllText(DataLocation, JsonConvert.SerializeObject(list));
            }
        }
    }
}
