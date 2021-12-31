using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStops.Util
{
    internal static class VehicleUtil
    {
        internal enum VehicleStatus
        {
            Valid,
            Expired,
            None
        }

        internal class Info
        {
            internal VehicleStatus Registration { get; set; }
            internal VehicleStatus Insurance { get; set; }
            internal string LicensePlate { get; set; }
        }

        private static readonly Dictionary<Vehicle, Info> _information = new Dictionary<Vehicle, Info>();
   
        internal static Info QueryInformation(Vehicle veh)
        {
            if (veh == null) throw new ArgumentNullException(nameof(veh));
            if (!veh.IsValid()) throw new ArgumentException("Vehicle invalid!", nameof(veh));

            if (!_information.ContainsKey(veh))
            {
                var result = CreateInfo(veh);
                _information.Add(veh, result);
                return result;
            }
            else
            {
                return _information[veh];
            }
        }

        internal static VehicleStatus GetInvalidStatus() => MathHelper.GetRandomInteger(100) < Config.ExpirationRatio ? VehicleStatus.Expired : VehicleStatus.None;
        internal static VehicleStatus GetInvalidStatus(float prect, bool pred) => (!pred && MathHelper.GetRandomInteger(100) < prect) ? GetInvalidStatus() : VehicleStatus.Valid;
        internal static Info CreateInfo(Vehicle veh)
        {
            var problematic = MathHelper.GetRandomInteger(100) < Config.VehicleIssueRate;
            var both = MathHelper.GetRandomInteger(100) < Config.VehicleBothIssuesPercentage;

            if (problematic)
            {
                if (both)
                {
                    return new Info
                    {
                        Registration = GetInvalidStatus(),
                        Insurance = GetInvalidStatus(),
                        LicensePlate = veh.LicensePlate,
                    };
                }
                else
                {
                    var inf = new Info
                    {
                        LicensePlate = veh.LicensePlate,
                        Insurance = GetInvalidStatus(Config.VehicleInsuranceIssuePercentage, false)
                    };
                    inf.Registration = inf.Insurance != VehicleStatus.Valid ? GetInvalidStatus() : VehicleStatus.Valid;

                    return inf;
                }
            }
            else
            {
                return new Info
                {
                    Registration = VehicleStatus.Valid,
                    Insurance = VehicleStatus.Valid,
                    LicensePlate = veh.LicensePlate
                };
            }
        }
    }
}
