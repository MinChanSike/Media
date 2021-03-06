﻿namespace Carbon.Media
{
    public enum ExifOrientation : byte
    {
        None           = 0, // None
        Horizontal     = 1, // Normal
        FlipHorizontal = 2, // Flip horizontally
        Rotate180      = 3, // Rotate 180
        FlipVertical   = 4, // Flip vertically
        Transpose      = 5, // Transpose (mirror horizontal and rotate 270)
        Rotate90       = 6, // Rotate 90
        Transverse     = 7, // Transverse (mirror horizontal and rotate 90)
        Rotate270      = 8  // Rotate 270
    }
}

// transverse|transpose
// rotate(90deg|180deg|270deg)
// flip(y)
// flip(x)