﻿using System;

using FFmpeg.AutoGen;

namespace Carbon.Media
{
    public unsafe readonly struct Filter
    {
        public Filter(AVFilter* pointer)
        {
            Pointer = pointer;
        }

        public readonly AVFilter* Pointer;

        public static Filter FromName(string name)
        {
            var pointer = ffmpeg.avfilter_get_by_name(name);

            if (pointer == null)
            {
                throw new Exception("Filter named '" + name + "' was not found");
            }

            return new Filter(pointer);
        }
    }
}