﻿using Xunit;

namespace Carbon.Media.Tests
{
    using static ColorComponent;

    public class PixelFormatTests
    {
        [Theory]
        [InlineData("A8",         PixelFormat.A8)]
        [InlineData("BlackWhite", PixelFormat.BlackWhite)]
        [InlineData("Cmyk32",     PixelFormat.Cmyk32)]
        // RGB -------------------------------------------
        [InlineData("Rgb24",      PixelFormat.Rgb24)]
        [InlineData("Rgba32",     PixelFormat.Rgba32)]
        [InlineData("Rgba64",     PixelFormat.Rgba64)]
        [InlineData("rgb48be",    PixelFormat.Rgb48be)]
        // BGR(A) -------------------------------------------
        [InlineData("Bgr24",      PixelFormat.Bgr24)]
        [InlineData("Bgra32",     PixelFormat.Bgra32)]
        [InlineData("Bgra64",     PixelFormat.Bgra64)]
        // YUV
        [InlineData("yuv422p",    PixelFormat.Yuv422p)]
        public void Parse(string text, PixelFormat type)
        {
            Assert.Equal(type, PixelFormatHelper.Parse(text));
        }


        [Fact]
        public void Identity()
        {
            Assert.Equal(10, (int)PixelFormat.Bgr4);
        }

        // https://ffmpeg.org/pipermail/ffmpeg-devel/2007-May/035617.html

      

        [Fact]
        public void ComponentTests()
        {
            // RGBA
            Assert.Equal(1,  (int)R);
            Assert.Equal(2,  (int)G);
            Assert.Equal(3,  (int)B);
            Assert.Equal(4,  (int)A);

            // YUV
            Assert.Equal(5,  (int)Y);
            Assert.Equal(6,  (int)U); 
            Assert.Equal(7,  (int)V);
            
            // CMYK
            Assert.Equal(10, (int)Cyan);
            Assert.Equal(11, (int)Magenta);
            Assert.Equal(12, (int)Yellow);
            Assert.Equal(13, (int)Key);
        }

        [Fact]
        public void ColorChannelAliasTests()
        {
            // CbCr aliases
            Assert.Equal(6, (int)ColorChannel.Cb(1).Component);
            Assert.Equal(7, (int)ColorChannel.Cr(1).Component);
        }

        [Fact]
        public void General()
        {
            var bw = PixelFormat.BlackWhite.GetInfo();

            Assert.Equal(1, bw.BitsPerPixel);
            // Assert.Equal(ColorChannels.K, bw.Channels);
            Assert.Equal(ColorModel.Monochrome, bw.ColorModel);

            var rgb24 = PixelFormat.Rgb24.GetInfo();

            Assert.Equal(8, rgb24.BitsPerPixel);
            // Assert.Equal(ColorChannels.RGB, rgb24.Channels);
            Assert.Equal(ColorModel.RGB, rgb24.ColorModel);

            var rgba64 = PixelFormat.Rgba64.GetInfo();

            Assert.Equal(16, rgba64.BitsPerPixel);
            // Assert.Equal(ColorChannels.RGBA, rgba64.Channels);
            Assert.Equal(ColorModel.RGB, rgba64.ColorModel);
        }
    }
}