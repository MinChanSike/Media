﻿using System;

namespace Carbon.Media
{
    [Flags]
    public enum MediaFeatures : byte
    {
        Unknown      = 0,
        None         = 1 << 0,  // 1
        AlphaChannel = 1 << 1,  // 2
        Animated     = 1 << 2,  // 4
        ColorProfile = 1 << 3,  // 8,

        // Color spaces
        CMYK         = 1 << 5,  // 16
        Gray         = 1 << 6
    }
}