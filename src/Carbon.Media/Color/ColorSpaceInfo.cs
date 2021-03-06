﻿using System;

namespace Carbon.Media
{
    public class ColorSpaceInfo
    {
        public ColorSpaceInfo(string name, ColorModel model, ColorSpaceFlags flags = default)
        {
            Name  = name ?? throw new ArgumentNullException(nameof(name));
            Model = model;
            Flags = flags;
        }

        public string Name { get; }

        public ColorModel Model { get; }

        public ColorSpaceFlags Flags { get; }

        public bool IsWideGamut => (Flags & ColorSpaceFlags.WideGamut) != 0;

        public static readonly ColorSpaceInfo Cmyk     = new ColorSpaceInfo("CMYK",      ColorModel.CMYK);
        public static readonly ColorSpaceInfo Rgb      = new ColorSpaceInfo("RGB",       ColorModel.RGB);
        public static readonly ColorSpaceInfo AdobeRgb = new ColorSpaceInfo("Adobe RGB", ColorModel.RGB);
        public static readonly ColorSpaceInfo DciP3    = new ColorSpaceInfo("DCI-P3",    ColorModel.RGB, ColorSpaceFlags.WideGamut);
    }
}