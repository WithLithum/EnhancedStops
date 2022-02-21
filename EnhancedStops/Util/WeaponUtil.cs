using Rage;

namespace EnhancedStops.Util
{
    internal static class WeaponUtil
    {
        internal static void DisplayGunPermit(Ped ped)
        {
            var has = AcquireGunLicense(ped, out var license, out var permit);

            GameFiber.StartNew(() =>
            {
                GameFiber.Sleep(MathHelper.GetRandomInteger(3500, 5500));

                if (!has)
                {
                    Game.DisplayNotification("~r~No license nor permit");
                }
                else
                {
                    Game.DisplayNotification($"~b~License: ~s~{license}~n~~b~Permit: ~s~{permit}");
                }
            });
        }

        internal static string RandomPermit()
        {
            var rc = MathHelper.GetRandomInteger(0, 10);

            return rc >= 8 ? "Public Carry" : "Concealed Carry";
        }

        internal static string RandomGunLicense()
        {
            switch (MathHelper.GetRandomInteger(0, 3))
            {
                case 0:
                    return "Handguns";

                case 1:
                    return "Long guns";

                case 2:
                    return "Handguns and Long guns";

                case 3:
                    return "Automatic weapons";

                default:
                    return "BEPIS";
            }
        }

        internal static bool AcquireGunLicense(Ped ped, out string license, out string permit)
        {
            if (!ped)
            {
                license = "~y~No Data";
                permit = "~y~No Data";
                return false;
            }

            if (!(ped.Metadata.hasGunPermit is bool hasPermit) || !hasPermit)
            {
                license = "~r~None";
                permit = "~r~None";
                return false;
            }

            if (!(ped.Metadata.gunLicense is string gunLicense))
            {
                var lc = RandomGunLicense();
                gunLicense = lc;
                ped.Metadata.gunLicense = lc;
            }

            if (!(ped.Metadata.gunPermit is string gunPermit))
            {
                var gp = RandomPermit();
                gunPermit = gp;
                ped.Metadata.gunPermit = gp;
            }

            license = gunLicense;
            permit = gunPermit;
            return true;
        }
    }
}
