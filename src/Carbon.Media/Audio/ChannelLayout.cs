﻿using System;

namespace Carbon.Media
{
    [Flags]
    public enum ChannelLayout : long
    {
        Unknown             = 0,
        FrontLeft           = 1 << 0,
        FrontRight          = 1 << 1,
        FrontCenter         = 1 << 2,
        LowFrequency        = 1 << 3,
        BackLeft            = 1 << 4,
        BackRight           = 1 << 5,
        FrontLeftOfCenter   = 1 << 6,
        FrontRightOfCenter  = 1 << 7,
        BackCenter          = 1 << 8,
        SideLeft            = 1 << 9,
        SideRight           = 1 << 10,
        TopCenter           = 1 << 11,
        TopFrontLeft        = 1 << 12,
        TopFrontCenter      = 1 << 13,
        TopFrontRight       = 1 << 14,
        TopBackLeft         = 1 << 15,
        TopBackCenter       = 1 << 16,
        TopBackRight        = 1 << 17,
        DownmixLeft         = 0x20000000,
        DownmixRight        = 0x40000000,
        WideLeft            = 1 << 20,
        WideRight           = 1 << 21,
        SurroundDirectLeft  = 1 << 22,
        DirectDirectRight   = 1 << 23,
        LowFrequency2       = 1 << 24,

        Mono     = FrontCenter,
        Stereo   = FrontLeft | FrontRight,
        _2_1 = Stereo | LowFrequency,                       // 2.1
        _3_0 = Stereo | FrontCenter,                        // 3.0
        _3_1 = _3_0 | LowFrequency,                         // 3.1
        _4_0 = Stereo | FrontCenter | BackCenter,           // 4.0
        _4_1 = _4_0 | LowFrequency,                         // 4.1
        _5_0 = Stereo | FrontCenter | SideLeft | SideRight, // 5.0
        _5_1 = _5_0 | LowFrequency,                         // 5.1
        _7_0 = _5_0 | BackLeft | BackRight,
        _2_2     = Stereo | SideLeft | SideRight,
        Quad     = Stereo | BackLeft | BackRight,

        StereoDownmix = DownmixLeft | DownmixRight
    }
}