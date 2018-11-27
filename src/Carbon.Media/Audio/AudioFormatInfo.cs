﻿using System;

namespace Carbon.Media
{
    public class AudioFormatInfo : IEquatable<AudioFormatInfo>
    {
        private readonly SampleFormatInfo sampleFormatInfo;

        public AudioFormatInfo(
            SampleFormat sampleFormat,
            ChannelLayout channelLayout,
            int sampleRate)
        {
            if (sampleFormat == SampleFormat.Unknown)
                throw new ArgumentException("Must not be Unknown", nameof(sampleFormat));

            // 8 kHz ?

            if (sampleRate <= 0 || sampleRate > 22_579_200)
            {
                throw ExceptionHelper.OutOfRange(nameof(sampleRate), 0, 22_579_200, sampleRate);
            }

            if (channelLayout == default)
            {
                throw new ArgumentException("Must not be unknown", nameof(channelLayout));
            }

            SampleFormat = sampleFormat;
            SampleRate = sampleRate;
            ChannelLayout = channelLayout;

            sampleFormatInfo = SampleFormatInfo.Get(sampleFormat);
        }

        // bitCount, isPlanar, ..
        public SampleFormat SampleFormat { get; }

        public int SampleRate { get; }

        public ChannelLayout ChannelLayout { get; }

        public int ChannelCount => ChannelLayout.GetChannelCount();

        #region Helpers

        public int BitsPerSample => sampleFormatInfo.BitCount;

        public bool IsPlanar => sampleFormatInfo.IsPlanar;

        public int LineCount => sampleFormatInfo.IsPlanar ? ChannelCount : 1; // PlaneCount?

        public int LineSize => sampleFormatInfo.IsPlanar ? (BitsPerSample >> 3) : (BitsPerSample >> 3) * ChannelCount;

        #endregion

        public override string ToString()
        {
            return string.Join(" ", SampleFormat, ChannelLayout, SampleRate);
        }

        #region Equality

        public bool Equals(AudioFormatInfo other) =>
            SampleFormat == other.SampleFormat &&
            SampleRate == other.SampleRate &&
            ChannelLayout == other.ChannelLayout;

        #endregion
    }
}

/// 8,000 Hz     : Telephone
/// 44,100 Hz    : Audio CD, most popular MPEG-1 (VCD, SVCD, MP3) sampling rate
/// 48,000 Hz    : TV, DVD, and films. 
/// 96,000 Hz    : DVD-Audio, BD-ROM (Blu-ray Disc)/HD-DVD audio tracks
/// 192,000 Hz   : DVD-Audio, BD-ROM (Blu-ray Disc)/HD-DVD audio tracks
/// 2,822,400 Hz : SACD
// Channels { L, R, C }