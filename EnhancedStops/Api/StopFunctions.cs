using EnhancedStops.Util;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStops.Api
{
    /// <summary>
    /// The public API function for EnhancedStops.
    /// </summary>
    /// <remarks>
    /// You must call this class in a "wrapper", otherwise, your plugin would let
    /// <b>EnhancedStops</b> from <b>best to have</b> to a <b>must have</b>.
    /// </remarks>
    public static class StopFunctions
    {
        /// <summary>
        /// Gets the registration status of the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <returns>An instance of <see cref="VehicleStatus"/> representing the registration status of the vehicle.</returns>
        public static VehicleStatus GetRegistrationStatus(Vehicle vehicle)
        {
            return VehicleUtil.QueryInformation(vehicle).Registration;
        }

        /// <summary>
        /// Gets the insurance status of the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <returns>An instance of <see cref="VehicleStatus"/> representing the insurance status of the vehicle.</returns>
        public static VehicleStatus GetInsuranceStatus(Vehicle vehicle)
        {
            return VehicleUtil.QueryInformation(vehicle).Insurance;
        }

        /// <summary>
        /// Sets the registration status of the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <param name="status">The status to set.</param>
        public static void SetRegistrationStatus(Vehicle vehicle, VehicleStatus status)
        {
            var inf = VehicleUtil.QueryInformation(vehicle);
            inf.Registration = status;
            VehicleUtil.SetInfo(vehicle, inf);
        }

        /// <summary>
        /// Sets the insurance status of the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <param name="status">The status to set.</param>
        public static void SetInsuranceStatus(Vehicle vehicle, VehicleStatus status)
        {
            var inf = VehicleUtil.QueryInformation(vehicle);
            inf.Insurance = status;
            VehicleUtil.SetInfo(vehicle, inf);
        }

        /// <summary>
        /// Sets the status of the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <param name="registration">The registration status.</param>
        /// <param name="insurance">The insurance status.</param>
        public static void SetVehicleStatus(Vehicle vehicle, VehicleStatus registration, VehicleStatus insurance)
        {
            var inf = VehicleUtil.QueryInformation(vehicle);
            inf.Registration = registration;
            inf.Insurance = insurance;
            VehicleUtil.SetInfo(vehicle, inf);
        }
    }
}
