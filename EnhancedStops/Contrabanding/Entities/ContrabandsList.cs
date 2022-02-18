// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

namespace EnhancedStops.Contrabanding.Entities
{
    /// <summary>
    /// Represents a list of contrabands.
    /// </summary>
    public struct ContrabandsList
    {
#pragma warning disable S1104 // Fields should not have public accessibility
        /// <summary>
        /// Drugs.
        /// </summary>
        public string[] Drugs;
        /// <summary>
        /// Contrabands.
        /// </summary>
        public string[] Contrabands;
        /// <summary>
        /// Items.
        /// </summary>
        public string[] Items;
        /// <summary>
        /// Weapons.
        /// </summary>
        public string[] Weapons;
#pragma warning restore S1104 // Fields should not have public accessibility
    }
}
