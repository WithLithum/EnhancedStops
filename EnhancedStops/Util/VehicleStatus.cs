namespace EnhancedStops.Util
{
    /// <summary>
    /// Represents the status of an entry of the vehicle.
    /// </summary>
    public enum VehicleStatus
    {
        /// <summary>
        /// The entry is valid.
        /// </summary>
        Valid,

        /// <summary>
        /// The entry has expired.
        /// </summary>
        Expired,

        /// <summary>
        /// There is no such entry, or the entry was invalid.
        /// </summary>
        None
    }
}