﻿using System;

namespace Carbon.Media.Formats
{
    public class GifMuxerOptions
    {   
        // -1 for no loop
        // 0 for infinate
        public int Loop { get; set; }

        public TimeSpan FinalDelay { get; set; }
    }
}
