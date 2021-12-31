// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using LemonUI.Elements;
using System.Drawing;

namespace EnhancedStops
{
    internal static class Globals
    {
        internal const string ModName = "EnhancedStops";

        internal const string ModIconDictionary = "commonmenu";
        internal const string ModIconTexture = "shop_makeup_icon_a";

        internal static readonly ScaledRectangle BackgroundRect = new ScaledRectangle(new PointF(), new SizeF(512f, 128f))
        {
            Color = Color.FromArgb(121, Color.Black)
        };
    }
}
