﻿using FFmpeg.AutoGen;
using Xunit;

namespace Carbon.Media.Ffmpeg.Tests
{
    using static ChannelLayout;

    public class ChannelLayoutTests
    {
        private static readonly ChannelLayout[] layouts = {
            FrontLeft,
            FrontRight,
            FrontCenter,
            LowFrequency,
            BackLeft,
            BackRight,
            FrontLeftOfCenter,
            FrontRightOfCenter,
            BackCenter,
            SideLeft,
            SideRight,
            TopCenter,
            TopFrontLeft,
            TopFrontCenter,
            TopFrontRight,
            TopBackLeft,
            TopBackCenter,
            TopBackRight,
            DownmixLeft,
            DownmixRight,
            WideLeft,
            WideRight,
            SurroundDirectLeft,
            DirectDirectRight,
            LowFrequency2,

            Mono,
            Stereo,
            _2_1,
            _3_0,
            _3_1,
            _4_0,
            _4_1,
            _5_0,
            _5_1,
            _6_0,
            _6_1,
            _7_0,
            _7_1,
            _2_2,
            Quad,
            Hexagonal,
            Octagonal,
            StereoDownmix
        };

        [Fact]
        public void ChannelCountEquality()
        {
            foreach (var layout in layouts)
            {
                var count = layout.GetChannelCount();
                var count2 = ffmpeg.av_get_channel_layout_nb_channels((ulong)layout);

                Assert.Equal(count, count2);
            }
        }
    }
}