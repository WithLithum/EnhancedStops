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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> was null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="vehicle"/> was invalid.</exception>
        public static VehicleStatus GetRegistrationStatus(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (!vehicle.IsValid()) throw new ArgumentException("The specified vehicle was invalid!", nameof(vehicle));

            return VehicleUtil.QueryInformation(vehicle).Registration;
        }

        /// <summary>
        /// Gets the insurance status of the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <returns>An instance of <see cref="VehicleStatus"/> representing the insurance status of the vehicle.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> was null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="vehicle"/> was invalid.</exception>
        public static VehicleStatus GetInsuranceStatus(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (!vehicle.IsValid()) throw new ArgumentException("The specified vehicle was invalid!", nameof(vehicle));
            return VehicleUtil.QueryInformation(vehicle).Insurance;
        }

        /// <summary>
        /// Sets the registration status of the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <param name="status">The status to set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> was null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="vehicle"/> was invalid.</exception>
        public static void SetRegistrationStatus(Vehicle vehicle, VehicleStatus status)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (!vehicle.IsValid()) throw new ArgumentException("The specified vehicle was invalid!", nameof(vehicle));

            var inf = VehicleUtil.QueryInformation(vehicle);
            inf.Registration = status;
            VehicleUtil.SetInfo(vehicle, inf);
        }

        /// <summary>
        /// Sets the insurance status of the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <param name="status">The status to set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> was null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="vehicle"/> was invalid.</exception>
        public static void SetInsuranceStatus(Vehicle vehicle, VehicleStatus status)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (!vehicle.IsValid()) throw new ArgumentException("The specified vehicle was invalid!", nameof(vehicle));

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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> was null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="vehicle"/> was invalid.</exception>
        public static void SetVehicleStatus(Vehicle vehicle, VehicleStatus registration, VehicleStatus insurance)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (!vehicle.IsValid()) throw new ArgumentException("The specified vehicle was invalid!", nameof(vehicle));

            var inf = VehicleUtil.QueryInformation(vehicle);
            inf.Registration = registration;
            inf.Insurance = insurance;
            VehicleUtil.SetInfo(vehicle, inf);
        }

        /// <summary>
        /// Gets the alcohol level of the specified ped.
        /// </summary>
        /// <param name="ped">The ped to check.</param>
        /// <returns>An instance of <see cref="PedAlcoholLevel"/> representing the alcohol level of the specified ped.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="ped"/> specified was <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="ped"/> specified was invalid.</exception>
        public static PedAlcoholLevel GetPedAlcoholLevel(Ped ped)
        {
            if (ped == null) throw new ArgumentNullException(nameof(ped));
            if (!ped.IsValid()) throw new ArgumentException("The specified ped was invalid!", nameof(ped));

            return PedUtil.GetPedAlcoholLevelSlient(ped);
        }

        /// <summary>
        /// Sets the alcohol level of the specified ped.
        /// </summary>
        /// <param name="ped">The ped to set the alcohol level.</param>
        /// <param name="level">The level to set.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="ped"/> specified was <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="ped"/> specified was invalid.</exception>
        public static void SetPedAlcoholLevel(Ped ped, PedAlcoholLevel level)
        {
            if (ped == null) throw new ArgumentNullException(nameof(ped));
            if (!ped.IsValid()) throw new ArgumentException("The specified ped was invalid!", nameof(ped));

            PedUtil.OverridePedAlcoholLevel(ped, level);
        }
    }
}
