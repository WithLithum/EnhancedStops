// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using Rage;
using System.IO;
using System.Windows.Forms;

namespace EnhancedStops.Util
{
    internal static class Config
    {
        private static readonly InitializationFile iniFile = new InitializationFile(@"plugins\LSPDFR\EnhancedStops.ini");
        static Config()
        {
            // Ensure exists
            Directory.CreateDirectory(@"Data\WithLithum\EnhancedStops\");
            MenuKey = iniFile.ReadEnum("Generic", "MenuKey", Keys.G);
            MustIdentifyBeforeStatusCheck = iniFile.ReadBoolean("Generic", nameof(MustIdentifyBeforeStatusCheck), false);
            IdentifyOnStatusCheck = iniFile.ReadBoolean("Generic", nameof(IdentifyOnStatusCheck), true);

            VehicleIssueRate = iniFile.ReadSingle("Vehicles", nameof(VehicleIssueRate), 15);
            VehicleInsuranceIssuePercentage = iniFile.ReadSingle("Vehicles", nameof(VehicleInsuranceIssuePercentage), 55);
            VehicleRegistrationIssuePercentage = iniFile.ReadSingle("Vehicles", nameof(VehicleRegistrationIssuePercentage), 25);
            VehicleBothIssuesPercentage = iniFile.ReadSingle("Vehicles", nameof(VehicleBothIssuesPercentage), 15);
            ExpirationRatio = iniFile.ReadSingle("Vehicles", nameof(ExpirationRatio), 72);
            TreatUnregisteredVehiclesAsNoPlate = iniFile.ReadBoolean("Vehicles", nameof(TreatUnregisteredVehiclesAsNoPlate), false);
            BacUnit = iniFile.ReadString("Breathalyzer", nameof(BacUnit), "mg/100ml");
            SummaryOffenseBac = iniFile.ReadSingle("Breathalyzer", nameof(SummaryOffenseBac), 20f);
            IndictableBac = iniFile.ReadSingle("Breathalyzer", nameof(IndictableBac), 80f);
        }

        internal static void Init()
        {
            // nullsub for triggering static consturctor
        }

        internal static Keys MenuKey { get; }

        internal static float VehicleIssueRate { get; }
        internal static float VehicleInsuranceIssuePercentage { get; }
        internal static float VehicleRegistrationIssuePercentage { get; }
        internal static float VehicleBothIssuesPercentage { get; }
        internal static float ExpirationRatio { get; }
        internal static bool TreatUnregisteredVehiclesAsNoPlate { get; }
        internal static bool MustIdentifyBeforeStatusCheck { get; }
        internal static bool IdentifyOnStatusCheck { get; }

        internal static string BacUnit { get; }
        internal static float SummaryOffenseBac { get; }
        internal static float IndictableBac { get; }
    }
}