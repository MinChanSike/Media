﻿using System.Drawing;
using System.Security.Cryptography;
using System.Text;

using Carbon.Extensions;
using Carbon.Media.Drawing;

using Xunit;

namespace Carbon.Media.Tests
{
    using Processors;

    public sealed class Signer : IUrlSigner
    {
        private readonly string name;

        public Signer(string name)
        {
            this.name = name;
        }

        public string Sign(string path)
        {
            return name;
        }
    }

    public sealed class MD5Signer : IUrlSigner
    {
        public string Sign(string path)
        {
            using (var md5 = MD5.Create())
            {
                return HexString.FromBytes(md5.ComputeHash(Encoding.UTF8.GetBytes(path)));
            }
        }
    }

    public class TransformTests
    {
        private static readonly MediaSource jpeg_50x50 = new MediaSource("1045645", 100, 50);
        private static readonly MediaSource jpeg_85x20 = new MediaSource("a", 85, 20);

        [Fact]
        public void ResizePercent()
        {

            var rendition = new MediaTransformation(jpeg_85x20)
                .Resize(Unit.Parse("300％"), Unit.Parse("200％"))
                .Encode(ImageFormat.Jpeg);

            Assert.Equal(85 * 3, rendition.Width);
            Assert.Equal(20 * 2, rendition.Height);
        }

        [Fact]
        public void Scale1()
        {
            var rendition = new MediaTransformation(jpeg_85x20)
                .Apply(new ScaleTransform(100, 200, InterpolaterMode.Cubic))
                .Encode(ImageFormat.Jpeg);

            Assert.Equal(100, rendition.Width);
            Assert.Equal(200, rendition.Height);
        }

        [Fact]
        public void Pad1()
        {
            var rendition = new MediaTransformation(jpeg_85x20)
                .Apply(new PadTransform(10))
                .Encode(ImageFormat.Jpeg);


            Assert.Equal(105, rendition.Width);
            Assert.Equal(40, rendition.Height);
        }


        [Fact]
        public void ResizeAuto()
        {
            var rendition = new MediaTransformation(jpeg_85x20)
                .Resize(Unit.Parse("_"), Unit.Parse("40"))
                .Encode(ImageFormat.Jpeg);

            Assert.Equal(170, rendition.Width);
            Assert.Equal(40, rendition.Height);

            rendition = new MediaTransformation(jpeg_85x20)
                .Resize(Unit.Parse("170px"), Unit.Parse("_"))
                .Encode(ImageFormat.Jpeg);

            Assert.Equal(170, rendition.Width);
            Assert.Equal(40, rendition.Height);
        }
       
        [Fact]
        public void DrawTextTests()
        {
            var transformation = MediaTransformation.ParsePath("1045645/draw(text(Hello World,font:14px Helvetica))/100x100.jpeg");

            var draw = (DrawFilter)transformation.GetTransforms()[0];

            var text = (DrawTextCommand)draw.Commands[0];

            Assert.Equal("Hello World", text.Content);
            Assert.Equal(14,            text.Font?.Size.Value);
            Assert.Equal(UnitType.Px,   text.Font?.Size.Type);
            Assert.Equal("Helvetica",   text.Font?.Name);
        }

        /*
        [Fact]
        public void ClipTest1()
        {
            MediaRenditionInfo.Host = "google.com";

            var rendition = new MediaRenditionInfo(85, 20, "1045645/1:00/100x100.jpeg");

            var transform = rendition.TransformString[0];

            Assert.Equal("1:00/100x100.jpeg", rendition.TransformString);

            // 1045645/1:00/100x100.jpeg
            var transformation = MediaTransformation.ParsePath("1045645/00:01:00/100x100.jpeg");

            var clip = (Clip)transformation.GetTransforms()[0];

            Assert.Equal(TimeSpan.FromSeconds(60), clip.Start);
            Assert.Equal(TimeSpan.FromSeconds(60), clip.End);
        }
        */

        [Fact]
        public void ScaleTest2()
        {
            var rendition = new MediaRenditionInfo(
                host          : "google.com",
                sourcePath    : "1045645",
                transformPath : "100x100/crop(0,0,85,20).png",
                width         : 85,
                height        : 20
            );

            var b = rendition.Scale(2f);

            Assert.Equal("1045645",                      b.SourcePath);
            Assert.Equal("200x200/crop(0,0,170,40).png", b.TransformPath);

            var c = b.Scale(2f);

            Assert.Equal("1045645",                      c.SourcePath);
            Assert.Equal("400x400/crop(0,0,340,80).png", c.TransformPath);

            var d = c.WithFormat("gif");

            Assert.Equal("1045645",                      d.SourcePath);
            Assert.Equal("400x400/crop(0,0,340,80).gif", d.TransformPath);
        }

        [Fact]
        public void ScaleTest3()
        {
            var rendition = new MediaRenditionInfo(null, "1045645", "100x100/crop(0,0,85,20).png", 85, 20);

            var b = rendition.Scale(2.7f);

            Assert.Equal("270x270/crop(0,0,229,54).png", b.TransformPath);
        }



       

        [Fact]
        public void FormatOnlyTest1()
        {
            var rendition = new MediaRenditionInfo("test.com", "1045645.png", null, 85, 20);
            
            Assert.Null(rendition.TransformPath);
            Assert.Equal("1045645.png",  rendition.Path);
            Assert.Equal("https://test.com/1045645.png", rendition.Url);

            Assert.Equal(85, rendition.Width);
            Assert.Equal(20, rendition.Height);

            var rendition2 = new MediaRenditionInfo("test.com", "1045645.png", null, 85, 20, signer: new Signer("hi"));
            
            Assert.Equal("https://test.com/hi/1045645.png", rendition2.Url);

        }

        [Fact]
        public void ScaleTest4()
        {
            var rendition = new MediaRenditionInfo(
                host          :  null, 
                sourcePath    : "1045645", 
                transformPath : "100x100/crop(0,0,85,20).png",
                width         : 85,
                height        : 20
            );

            var b = rendition.Scale(2.7f);

            Assert.Equal(229, b.Width);
            Assert.Equal(54,  b.Height);

            Assert.Equal("270x270/crop(0,0,229,54).png", b.TransformPath);
            Assert.Equal("/1045645/270x270/crop(0,0,229,54).png", b.Url);
        }
        
        [Fact]
        public void Sign1()
        {
            var signer = new Signer("sig");

            var rendition = new MediaRenditionInfo("google.com", "1045645", "100x100/crop(0,0,85,20).png", 85, 20, signer: signer);

            Assert.Equal("https://google.com/sig/1045645/100x100/crop(0,0,85,20).png", rendition.Url);

            Assert.Equal("https://google.com/sig/1045645/200x200/crop(0,0,170,40).png", rendition.Scale(2f).Url);
            Assert.Equal("https://google.com/sig/1045645/200x200/crop(0,0,170,40).jpeg", rendition.Scale(2f).WithFormat("jpeg").Url);

        }

        [Fact]
        public void Sign2()
        {
            var signer = new MD5Signer();

            var rendition = new MediaRenditionInfo("google.com", "1045645", "100x100/crop(0,0,85,20).png", 85, 20, signer: signer);
            
            Assert.Equal("https://google.com/6f875abf9ec9623c955b69c8de53b890/1045645/100x100/crop(0,0,85,20).png", rendition.Url);
            Assert.Equal("https://google.com/8e6ade4ef8005a8c1eb4321e1e1ab861/1045645/200x200/crop(0,0,170,40).png", rendition.Scale(2f).Url);
            Assert.Equal("https://google.com/e4e842003975bee77a70d5952e2c3f6c/1045645/200x200/crop(0,0,170,40).jpeg", rendition.Scale(2f).WithFormat("jpeg").Url);
        }

        [Fact]
        public void ScaleTest()
        {
            var rendition = new MediaRenditionInfo("google.com", "1045645", "100x100/crop(0,0,85,20).png", 85, 20);

            var b = rendition.Scale(2f);

            Assert.Equal("https://google.com/1045645/200x200/crop(0,0,170,40).png", b.Url);
        }
        
        [Fact]
        public void WithFormatTests()
        {
            var a = new MediaRenditionInfo("google.com", "1045645", "100x100/crop(0,0,85,20).png", 85, 20);
            var b = new MediaRenditionInfo("google.com", "1045645", "100x100/crop(0,0,85,20).png", 85, 20);

            Assert.Equal("https://google.com/1045645/200x200/crop(0,0,170,40).jpeg", a.Scale(2).WithFormat("jpeg").Url);
        }

        [Fact]
        public void OrientedPhotoHasCorrectThings()
        {
            var source = new MediaSource("1", 100, 50, ExifOrientation.Rotate90);

            var rendition = new MediaTransformation(source).Encode(ImageFormat.Jpeg);

            Assert.Equal(50, rendition.Width);
            Assert.Equal(100, rendition.Height);

            Assert.Equal("rotate(90)", rendition.GetTransforms()[0].ToString());

            Assert.Equal("1/rotate(90).jpeg", rendition.GetPath());
        }

        [Fact]
        public void RotatedSizeIsCorrect()
        {
            var source = new MediaSource("1", 100, 50);

            var transform = new MediaTransformation(source).Rotate(90).Encode(ImageFormat.Jpeg);

            Assert.Equal(50, transform.Width);
            Assert.Equal(100, transform.Height);

            Assert.Equal("1/rotate(90).jpeg", transform.GetPath());
        }

        [Fact]
        public void ResizeJpeg()
        {
            var source = new MediaSource("64a5e3944d4f11c05cde1a60efa");

            var rendition = new MediaTransformation(source).Resize(85, 20).Encode(ImageFormat.Jpeg);

            Assert.Equal("85x20.jpeg", rendition.GetFullName());
            Assert.Equal("64a5e3944d4f11c05cde1a60efa/85x20.jpeg", rendition.GetPath());

            var img = MediaTransformation.ParsePath(rendition.GetPath());

            Assert.Equal(FormatId.Jpeg, img.Encoder.Format);
            Assert.Equal("85x20", img.GetTransforms()[0].ToString());
        }

        [Fact]
        public void CropPng()
        {
            var rendition = new MediaTransformation(new MediaSource("1045645"))
                .Resize(100, 100)
                .Crop(0, 0, 85, 20)
                .Encode(ImageFormat.Png);

            Assert.Equal("100x100/crop(0,0,85,20).png",         rendition.GetFullName());
            Assert.Equal("1045645/100x100/crop(0,0,85,20).png", rendition.GetPath());

            Assert.Equal("100x100|crop(0,0,85,20).png", rendition.GetFullName("|"));

            var result = MediaTransformation.ParsePath(rendition.GetPath());

            var transforms = result.GetTransforms();

            Assert.Equal(3, transforms.Count);

            Assert.Equal("1045645"          , result.Source.Key);
            Assert.Equal(FormatId.Png       , result.Encoder.Format);
            Assert.Equal("100x100"          , transforms[0].ToString());
            Assert.Equal("crop(0,0,85,20)"  , transforms[1].ToString());
            Assert.Equal("PNG::encode"      , transforms[2].ToString());
        }

        [Fact]
        public void CenterAnchoredResizePng()
        {
            var rendition = new MediaTransformation(new MediaSource("1"))
                .Apply(new ResizeTransform(new Size(100, 50), CropAnchor.Center))
                .Encode(ImageFormat.Png);

            Assert.Equal(100, rendition.Width);
            Assert.Equal(50, rendition.Height);

            Assert.Equal("100x50-c.png", rendition.GetFullName());
        }

        [Fact]
        public void ResizeAndRotated90()
        {
            var rendition = new MediaTransformation(new MediaSource("1"))
                .Resize(50, 50)
                .Crop(0, 0, 85, 20)
                .Rotate(90)
                .Encode(ImageFormat.Gif);

            Assert.Equal("50x50/crop(0,0,85,20)/rotate(90).gif", rendition.GetFullName());

            var r2 = MediaTransformation.ParsePath(rendition.GetPath());

            Assert.Equal(20, rendition.Width);
            Assert.Equal(85, rendition.Height);

            var transforms = r2.GetTransforms();

            Assert.Equal(4, transforms.Count);

            Assert.Equal(90, (transforms[2] as RotateTransform).Angle);
        }

        [Fact]
        public void LeftAnchoredResizeAndRotate90()
        {
            var transformation = new MediaTransformation(new MediaSource("1"))
                .Apply(new ResizeTransform(new Size(150, 50), anchor: CropAnchor.Left))
                .Rotate(90)
                .Encode(ImageFormat.Jpeg);

            Assert.Equal("150x50-l/rotate(90).jpeg", transformation.GetFullName());
        }

        [Fact]
        public void LeftAnchoredResizeAndRotatationTiff()
        {
            var rendition = new MediaTransformation(new MediaSource("1045645"))
                .Apply(new ResizeTransform(new Size(50, 50), anchor: CropAnchor.Left))
                .Rotate(180)
                .Encode(ImageFormat.Tiff);

            Assert.Equal("50x50-l/rotate(180).tiff", rendition.GetFullName());
        }

        [Fact]
        public void AnchoredResizeHasValidFileName()
        {
            var rendition = new MediaTransformation(new MediaSource("100"))
                .Apply(new ResizeTransform(new Size(100, 100), anchor: CropAnchor.Center))
                .Encode(ImageFormat.Jpeg);

            Assert.Equal(@"100x100-c.jpeg", rendition.GetFullName());

            rendition = new MediaTransformation(new MediaSource("254565"))
                .Apply(new ResizeTransform(new Size(500, 100), anchor: CropAnchor.Center))
                .Encode(ImageFormat.Jpeg);

            Assert.Equal(@"500x100-c.jpeg", rendition.GetFullName());
        }

        [Fact]
        public void Filters()
        {
            var transformation = new MediaTransformation(jpeg_50x50)
                .Apply(new ContrastFilter(2f))
                .Apply(new GrayscaleFilter(1f))
                .Apply(new QuantizeFilter(256))
                .Apply(new SepiaFilter(1f))
                .Apply(new OpacityFilter(1f))
                .Apply(new SaturateFilter(1f))
                .Apply(new HueRotateFilter(90))
                .Encode(ImageFormat.Heif);

            Assert.Equal(100, transformation.Width);
            Assert.Equal(50, transformation.Height);

            Assert.Equal("contrast(2)/grayscale(1)/quantize(256)/sepia(1)/opacity(1)/saturate(1)/hueRotate(90deg).heif", transformation.GetFullName());

            var img = MediaTransformation.ParsePath(transformation.GetPath());

            var transforms = img.GetTransforms();

            Assert.Equal(8, transforms.Count);

            Assert.Equal(2f,  (transforms[0] as ContrastFilter).Amount);
            Assert.Equal(1f,  (transforms[1] as GrayscaleFilter).Amount);
            Assert.Equal(256, (transforms[2] as QuantizeFilter).MaxColors);
            Assert.Equal(1f,  (transforms[3] as SepiaFilter).Amount);
            Assert.Equal(1f,  (transforms[4] as OpacityFilter).Amount);
            Assert.Equal(1f,  (transforms[5] as SaturateFilter).Amount);
            Assert.Equal(90,  (transforms[6] as HueRotateFilter).Degrees);

            Assert.Equal(FormatId.Heif, (transforms[7] as Encode).Format);
        }

        [Fact]
        public void BuilderTests()
        {
            var transformation = new MediaTransformation(jpeg_50x50)
                .Resize(100, 100)
                .Rotate(90)
                .Apply(new SepiaFilter(1))
                .Encode(ImageFormat.Jpeg);

            Assert.Equal("1045645", transformation.Source.Key);
            Assert.Equal(100, transformation.Width);
            Assert.Equal(100, transformation.Height);

            Assert.Equal("100x100/rotate(90)/sepia(1).jpeg", transformation.GetFullName());
        }
    }

    public class MediaSource : IMediaInfo
    {
        public MediaSource(string key, int width = 0, int height = 0, ExifOrientation? orientation = null)
        {
            Key = key;
            Width = width;
            Height = height;
            Orientation = orientation;
        }

        public string Key { get; }

        public ExifOrientation? Orientation { get; }

        public int Width { get; }

        public int Height { get; }
    }
}