﻿using System;
using System.Drawing;

namespace Carbon.Media
{
    using Processors;

    using static ExifOrientation;

    public static class OrientationHelper
    {
        public static Size GetOrientatedSize(Size size, ExifOrientation orientation)
        {
            switch (orientation)
            {
                case Transpose:
                case Rotate90:
                case Transverse:
                case Rotate270 : return new Size(size.Height, size.Width);
                default        : return new Size(size.Width, size.Height);
            }
        }

        public static ExifOrientation Parse(string text) =>
            Enum.TryParse<ExifOrientation>(text, true, out var result) 
                ? result 
                : throw new Exception("Invalid Orientation: " + text);

        public static ITransform[] GetTransforms(this ExifOrientation orientation)
        {
            switch (orientation)
            {
                case FlipHorizontal : return new ITransform[] { FlipTransform.Horizontally };
                case Rotate180      : return new ITransform[] { new RotateTransform(180) };
                case FlipVertical   : return new ITransform[] { FlipTransform.Vertically };
                case Transpose      : return new ITransform[] { FlipTransform.Horizontally, new RotateTransform(270) };
                case Rotate90       : return new ITransform[] { new RotateTransform(90) };
                case Transverse     : return new ITransform[] { FlipTransform.Horizontally, new RotateTransform(90) };
                case Rotate270      : return new ITransform[] { new RotateTransform(270) };
                default             : return new ITransform[0];
            }
        }
    }
}
