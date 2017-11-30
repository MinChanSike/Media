﻿namespace Carbon.Media
{
    public static class ChannelLayoutHelper
    {
        private static int CountSetBits(ulong value)
        {
            value = value - ((value >> 1) & 0x5555555555555555UL);
            value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);
            return (int)(unchecked(((value + (value >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
        }

        public static int GetChannelCount(this ChannelLayout layout)
        {
            return CountSetBits((ulong)layout);
        }
    }
}