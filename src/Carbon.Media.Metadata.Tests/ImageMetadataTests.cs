﻿using System;
using Carbon.Json;
using Carbon.Media.Metadata.Exif;
using Xunit;

namespace Carbon.Media.Metadata.Tests
{
    public class ImageMetadataTests
    {
        [Fact]
        public void SerializeAspect()
        {
            var aspect = new Rational(1, 2);

            var aspect2 = Helper.SerializeAndBack(aspect);

            Assert.Equal(1, aspect2.Numerator);
            Assert.Equal(2, aspect2.Denominator);
        }

        [Fact]
        public void Test1()
        {
            var image = new ImageInfo {
                Format = "gif",
                Width  = 800,
                Height = 600
            };
            
            var image2 = Helper.SerializeAndBack(image);

            Assert.Equal("gif", image2.Format);
            Assert.Equal(800, image2.Width);
            Assert.Equal(600, image2.Height);

            // TODO: format should be "aac"

            Assert.Equal(@"{
  ""format"": ""gif"",
  ""width"": 800,
  ""height"": 600
}", JsonObject.FromObject(image).ToString());
        }

        [Fact]
        public void Test3()
        {
            var image = new ImageInfo {
                Format      = "tiff",
                PixelFormat = PixelFormat.Cmyk32,
                Width       = 1_000_000,
                Height      = 1_000_000,
                ColorSpace  = ColorSpace.CMYK,
                Exif        = new ExifMetadata { Orientation = ExifOrientation.Rotate90 }
            };

            var image2 = Helper.SerializeAndBack(image);
            
            Assert.Equal("tiff",                   image2.Format);
            Assert.Equal(1_000_000,                image2.Width);
            Assert.Equal(1_000_000,                image2.Height);
            Assert.Equal(ExifOrientation.Rotate90, image2.Exif.Orientation);
            Assert.Equal(PixelFormat.Cmyk32,       image2.PixelFormat);
        }

        [Fact]
        public void TestPhotograph()
        {
            // 12MP Digital Negitive
            // 64bpp = 96MB buffer
            var image = new ImageInfo {
                Format      = "dng",
                Width       = 4000,
                Height      = 3000,
                PixelFormat = PixelFormat.Rgba64,
                ColorSpace  = ColorSpace.RGB,
                Camera      = new CameraInfo(Make.Canon, new Model("EOS 5D")),
                Copyright   = "©2018 Willy Wonka. All Rights Reserved.",
                Exposure    = new ExposureInfo { Time = TimeSpan.FromSeconds(1) },
                Lens        = new LensInfo(Make.Canon, "EF-S 35mm f/2.8 Macro IS STM"),
                Location    = new GpsData { Altitude = Unit.Meters(10.5), Latitude = 2, Longitude = 3 },
                Lighting    = new LightingInfo { Source = ExifLightSource.D50 },
                Software    = new SoftwareInfo { Name = "Photoshop" },
                Owner       = new ActorInfo {  Name = "Willy Wonka" }
            };

            var image2 = Helper.SerializeAndBack(image);

            Assert.Equal("dng",                                     image2.Format);
            Assert.Equal(4000,                                      image2.Width);
            Assert.Equal("©2018 Willy Wonka. All Rights Reserved.", image2.Copyright);
            Assert.Equal(ColorSpace.RGB,                            image2.ColorSpace);
            Assert.Equal("Canon",                                   image2.Camera.Make);
            Assert.Equal("EOS 5D",                                  image2.Camera.Model);
            Assert.Equal("Canon",                                   image2.Lens.Make);
            Assert.Equal("EF-S 35mm f/2.8 Macro IS STM",            image2.Lens.Model);
            Assert.Equal("10.5m",                                   image2.Location.Altitude.ToString());
            Assert.Equal(2,                                         image2.Location.Latitude);
            Assert.Equal(3,                                         image2.Location.Longitude);

            Assert.Null(image2.Exif);

            Assert.Equal(@"{
  ""format"": ""dng"",
  ""pixelFormat"": ""Rgba64"",
  ""width"": 4000,
  ""height"": 3000,
  ""colorSpace"": ""RGB"",
  ""copyright"": ""\u00A92018 Willy Wonka. All Rights Reserved."",
  ""owner"": {
    ""name"": ""Willy Wonka""
  },
  ""camera"": {
    ""make"": ""Canon"",
    ""model"": ""EOS 5D""
  },
  ""exposure"": {
    ""time"": ""00:00:01""
  },
  ""lens"": {
    ""make"": ""Canon"",
    ""model"": ""EF-S 35mm f\/2.8 Macro IS STM""
  },
  ""lighting"": {
    ""source"": ""D50""
  },
  ""location"": {
    ""latitude"": 2,
    ""longitude"": 3,
    ""altitude"": ""10.5m""
  },
  ""software"": {
    ""name"": ""Photoshop""
  }
}", JsonObject.FromObject(image).ToString());
        }
    }
}