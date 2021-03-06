﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text;

namespace Carbon.Media.Processing
{
    public class Pipeline
    {
        public IMediaInfo Source { get; set; } // Input

        // | page(1)
        // | frame(17)
        // | timestamp(1.452)
        public ExtractFilter? Extract { get; set; }
        
        // | fff
        // | black
        // | linear-gradient(...)
        [DataMember(Name = "background")]
        public string Background { get; set; }

        // 1. Flip
        public FlipTransform Flip { get; set; }

        // 2. Rotate
        public int Rotate { get; set; }

        // 2. Crop the source
        public Rectangle? Crop { get; set; }   // The crop applied to the source

        // 3. Scale the croped source w/ the interpolator
        public ScaleTransform Scale { get; set; }

        // If the image is padded, we need to create an intermediate canvas
        
        // If the resize mode was set to pad (or padding was added after) 
        public Padding Padding { get; set; } 

        public Position Position => new Position(Padding.Left, Padding.Top);
        
        public Size FinalSize => new Size(
            width  : (Scale?.Width  ?? Crop.Value.Width)  + Padding.Left + Padding.Right,
            height : (Scale?.Height ?? Crop.Value.Height) + Padding.Top  + Padding.Bottom
        );
    
        // 4. Determine whether we need a canvas to apply filters or draw a background
        // Filters & Draws
        public List<IProcessor> Filters { get; } = new List<IProcessor>();

        public MetadataFilter Metadata { get; set; }

        public DateTime? Expires { get; set; }

        public EncodeParameters Encode { get; set; }

        public bool IsDebug { get; set; }

        // lossless?

        // blob#1 |> orient(x) |> crop(0,0,100,100) |> JPEG(quality:87)

        // blob#1 |> crop(0,0,100,100) |> JPEG(quality:87)
        // blob#1 |> scale(50,50,lancoz) |> draw(background:0xffffff,margin:10) |> pixelate(5px) |> blur(5px) |> sepia(0.5) |> WebP::encode

        // blob#1 |> crop(0,0,100,100) |> JPEG::encode(quality:87)
        // blob#1 |> JPEG::encode(quality:87)
        // blob#1 |> WebP::encode

        public ValidateResult Validate()
        {
            // Ensure the width and height are divisible by 2 when targeting the MP4 format
            if (Encode.Format == FormatId.Mp4)
            {
                if (FinalSize.Width % 2 != 0)
                {
                    return new ValidateResult(
                        new ValidationError($"The MP4 format requires a width divisible by 2. Was {FinalSize.Width}.")
                    );
                }
                else if (FinalSize.Height % 2 != 0)
                {
                    return new ValidateResult(
                      new ValidationError($"The MP4 format requires a height divisible by 2. Was {FinalSize.Height}.")
                  );
                }
            }

            return ValidateResult.Successful;
        }

        public static Pipeline From(MediaTransformation transformation)
        {
            return From(transformation.Source, transformation.GetTransforms());
        }

        public static Pipeline From(IMediaInfo source, IReadOnlyList<IProcessor> processors)
        {
            var pipeline = new Pipeline {
                Source = source
            };

            if (source.Orientation != null)
            {
                switch(source.Orientation.Value)
                {
                    case ExifOrientation.FlipHorizontal : pipeline.Flip   = FlipTransform.Horizontally;  break;
                    case ExifOrientation.Rotate180      : pipeline.Rotate = 180; break;
                    case ExifOrientation.FlipVertical   : pipeline.Flip   = FlipTransform.Vertically;    break;

                    case ExifOrientation.Transpose      : pipeline.Flip = FlipTransform.Horizontally;
                                                          pipeline.Rotate = 270; break;

                    case ExifOrientation.Rotate90       : pipeline.Rotate = 90;                 break;

                    case ExifOrientation.Transverse     : pipeline.Flip = FlipTransform.Horizontally;
                                                          pipeline.Rotate = 90; break;
                    case ExifOrientation.Rotate270      : pipeline.Rotate = 270;                break;
                }
            }

            var interpolater = InterpolaterMode.Lanczos3;
            var encodingFlags = EncodeFlags.None;

            Rectangle? crop = null;

            var box = new PaddedBox(source.Width, source.Height);

            int? quality = null;

            foreach (var transform in processors)
            {
                if (transform is PageFilter page)
                {
                    pipeline.Extract = new ExtractFilter(ExtractFilterType.Page, page.Number);
                }
                else if (transform is FrameFilter frame)
                {
                    pipeline.Extract = new ExtractFilter(frame.Number == 0 ? ExtractFilterType.Poster : ExtractFilterType.Frame, frame.Number);
                }
                else if (transform is TimeFilter timestamp)
                {
                    pipeline.Extract = new ExtractFilter(ExtractFilterType.Time, timestamp.Value);
                }
                else if (transform is BackgroundFilter background)
                {
                    pipeline.Background = background.Color;
                }
                else if (transform is FlipTransform flip)
                {
                    // Do we need to apply the operations in reverse?
                    pipeline.Flip = flip;
                }
                else if (transform is CropTransform ct)
                {
                    // Note: We may have applied a scaling operating before the crop

                    double xScale = (double)pipeline.Source.Width / box.Width;
                    double yScale = (double)pipeline.Source.Height / box.Height;

                    var c = ct.GetRectangle(box.Size);

                    box.Width = c.Width;
                    box.Height = c.Height;

                    if (xScale != 1d || yScale != 1d)
                    {
                        c = c.Scale(xScale, yScale);
                    }

                    crop = c;
                }
                else if (transform is QualityFilter q)
                {
                    quality = q.Value;
                }
                else if (transform is ResizeTransform resize)
                {
                    var bounds = resize.CalcuateSize(box.Size);

                    bool upscale = resize.Upscale;

                    switch (resize.Mode)
                    {
                        case ResizeFlags.Crop:
                            crop = ResizeHelper.CalculateCropRectangle(box.Size, bounds.ToRational(), resize.Anchor ?? CropAnchor.Center);

                            box.Width = bounds.Width;
                            box.Height = bounds.Height;

                            break;

                        case ResizeFlags.Fit:
                            ResizeHelper.Fit(ref box, bounds, resize.Upscale);
                            break;

                        case ResizeFlags.Pad:
                            box = ResizeHelper.Pad(box.Size, bounds, resize.Anchor ?? CropAnchor.Center, resize.Upscale); break;

                        default: // Exact
                            box.Width = bounds.Width;
                            box.Height = bounds.Height;

                            break;
                    }
                }
                else if (transform is ScaleTransform scale)
                {
                    box.Width = scale.Width;
                    box.Height = scale.Height;

                    if (scale.Mode != InterpolaterMode.None)
                    {
                        interpolater = scale.Mode;
                    }
                }
                else if (transform is PadTransform pad)
                {
                    box.Padding = new Padding(
                        top: box.Padding.Top + pad.Top,
                        right: box.Padding.Right + pad.Right,
                        bottom: box.Padding.Bottom + pad.Bottom,
                        left: box.Padding.Left + pad.Left
                    );
                }
                else if (transform is RotateTransform rotate)
                {
                    if (rotate.Angle == 90 || rotate.Angle == 270)
                    {
                        var oldSize = box.Size;

                        // flip the height & width
                        box.Width = oldSize.Height;
                        box.Height = oldSize.Width;

                        if (crop != null)
                        {
                            var oldCrop = crop.Value;

                            crop = new Rectangle(oldCrop.Y, oldCrop.X, oldCrop.Height, oldCrop.Width);
                        }
                    }

                    pipeline.Rotate = rotate.Angle;
                }
                else if (transform is MetadataFilter metadata)
                {
                    pipeline.Metadata = metadata;
                }
                else if (transform is LosslessFlag)
                {
                    quality = 100;
                    encodingFlags |= EncodeFlags.Lossless;
                }
                else if (transform is ExpiresFilter expires)
                {
                    pipeline.Expires = expires.Timestamp;
                }

                else if (transform is EncodeParameters encode)
                {
                    pipeline.Encode = quality != null || encodingFlags != default
                        ? new EncodeParameters(encode.Format, quality, flags: encode.Flags | encodingFlags) // set the quality
                        : encode;
                }
                else if (transform is DebugFilter)
                {
                    pipeline.IsDebug = true;
                }
                else
                {
                    pipeline.Filters.Add(transform);
                }
            }

            pipeline.Crop = crop;

            if ((crop is null || crop.Value.Size != box.Size) 
                && box.OuterWidth > 0 && box.OuterHeight > 0)
            {
                pipeline.Scale = new ScaleTransform(box.Size, interpolater);
            }

            pipeline.Padding = box.Padding;
            
            return pipeline;
        }

        private static readonly string[] splitOn = new[] { "|>" };

        public static Pipeline Parse(string text)
        {
            string[] segments = text.Split(splitOn, count: 50, options: StringSplitOptions.None);

            /*
               blob#1 
            |> scale(50,50,lancoz)
            |> draw(background:0xffffff,margin:10) 
            |> pixelate(5px) 
            |> blur(5px) 
            |> sepia(0.5) 
            |> WebP::encode
            */

            var result = new Pipeline();

            foreach (var segment in segments)
            {
                int poundIndex = segment.IndexOf('#');

                if (poundIndex > -1)
                {
                    string id = segment.Substring(poundIndex + 1); // '#'

                    result.Source = new MediaInfo(id, 100, 100);

                    continue;
                }

                var transform = Processor.Parse(segment);

                switch (transform)
                {
                    case PageFilter page:
                        result.Extract = new ExtractFilter(ExtractFilterType.Page, page.Number);
                        break;
                    case FrameFilter frame:
                        result.Extract = new ExtractFilter(frame.Number == 0 ? ExtractFilterType.Poster : ExtractFilterType.Frame, frame.Number);

                        break;
                    case TimeFilter timestamp:
                        result.Extract = new ExtractFilter(ExtractFilterType.Time, timestamp.Value);
                        break;

                    case BackgroundFilter background:
                        result.Background = background.Color;
                        break;
                    case RotateTransform rotate:
                        result.Rotate = rotate.Angle;
                        break;
                    case FlipTransform flip:
                        result.Flip = flip;
                        break;
                    case ScaleTransform scale:
                        result.Scale = scale;
                        break;
                    case CropTransform crop:
                        result.Crop = crop.GetRectangle();
                        break;
                    case EncodeParameters encode:
                        result.Encode = encode;
                        break;
                    case DebugFilter _:
                        result.IsDebug = true;
                        break;
                    default:
                        result.Filters.Add(transform);
                        break;
                }
            }

            return result;
        }

        public string ToTransformPath()
        {
            if (Encode == null)
            {
                throw new Exception("Missing Encode");
            }

            var sb = StringBuilderCache.Aquire();

            if (Expires != null)
            {
                sb.Append('/');
                sb.Append("expires(");
                sb.Append(new DateTimeOffset(Expires.Value).ToUnixTimeSeconds());
                sb.Append(')');
            }

            if (Encode.Format == FormatId.Json && Metadata != null)
            {
                sb.Append('/');
                Metadata.WriteTo(sb);
                sb.Append(".json");

                return sb.ToString();
            }

            if (Extract is ExtractFilter extract)
            {
                sb.Append('/');
                
               // page | timestamp | frame | poster
               sb.Append(extract.ToString());
            }

            if (Background != null)
            {
                sb.Append('/');
                sb.Append("background(");
                sb.Append(Background);
                sb.Append(')');
            }

            if (Flip != null)
            {
                sb.Append('/');

                Flip.WriteTo(sb);
            }

            // TODO: Orient...

            if (Rotate != 0)
            {
                sb.Append('/');
                sb.Append("rotate(");
                sb.Append(Rotate);
                sb.Append("deg)");
            }

            if (Crop is Rectangle crop)
            {
                sb.Append('/');
                sb.Append($"crop({crop.X},{crop.Y},{crop.Width},{crop.Height})");
            }

            if (Scale != null)
            {
                sb.Append('/');

                sb.Append(Scale.Width);
                sb.Append('x');
                sb.Append(Scale.Height);
            }

            // pad(0,0)

            if (!Padding.Equals(Padding.Zero))
            {
                sb.Append('/');
                sb.Append("pad(");
                sb.Append(Padding.ToString());
                sb.Append(")");
            }

            foreach (var filter in Filters)
            {
                sb.Append('/');

                if (filter is ICanonicalizable canonicalizable)
                {
                    canonicalizable.WriteTo(sb);
                }
                else
                {
                    sb.Append(filter.Canonicalize());
                }
            }

            if (Encode.Flags.HasFlag(EncodeFlags.Lossless))
            {
                sb.Append('/');
                sb.Append("lossless");
            }
            else if (Encode.Quality != null)
            {
                sb.Append('/');
                sb.Append("quality(");
                sb.Append(Encode.Quality.Value);
                sb.Append(')');
            }

            sb.Append('.');
            sb.Append(Encode.Format.ToString().ToLower());

            return StringBuilderCache.ExtractAndRelease(sb).Substring(1);
        }

        public string Canonicalize()
        {
            var sb = StringBuilderCache.Aquire();

            sb.Append("blob#" + Source.Key);

            if (Expires != null)
            {
                sb.Append("|>expires(");
                sb.Append(new DateTimeOffset(Expires.Value).ToUnixTimeSeconds());
                sb.Append(')');
            }

            if (Encode.Format == FormatId.Json && Metadata != null)
            {
                sb.Append("|>");
                Metadata.WriteTo(sb);
                sb.Append("|>");
                Encode.WriteTo(sb);

                return sb.ToString();
            }

            if (Extract is ExtractFilter extract)
            {
                sb.Append("|>");

                // frame | page | timestamp
                sb.Append(extract.ToString());
            }
            
            if (Background != null)
            {
                sb.Append("|>background(");
                sb.Append(Background);
                sb.Append(')');
            }

            if (Flip != null)
            {
                sb.Append("|>");

                Flip.WriteTo(sb);
            }

            if (Rotate != 0)
            {
                sb.Append("|>rotate(");
                sb.Append(Rotate);
                sb.Append("deg)");
            }

            if (Crop is Rectangle crop)
            {
                sb.Append("|>");
                sb.Append($"crop({crop.X},{crop.Y},{crop.Width},{crop.Height})");
            }

            if (Scale != null)
            {
                sb.Append("|>");

                Scale.WriteTo(sb);
            }

            // pad(0,0)

            if (!Padding.Equals(Padding.Zero))
            {
                sb.Append("|>");
                sb.Append("pad(");
                sb.Append(Padding.ToString());
                sb.Append(")");
            }

            foreach (var filter in Filters)
            {
                sb.Append("|>");

                if (filter is ICanonicalizable canonicalizable)
                {
                    canonicalizable.WriteTo(sb);
                }
                else
                {
                    sb.Append(filter.Canonicalize());
                }
            }

            sb.Append("|>");

            Encode.WriteTo(sb);

            if (IsDebug)
            {
                sb.Append("|>debug");
            }

            return StringBuilderCache.ExtractAndRelease(sb);
        }
    }
}

// Canvas(500x500,color:red)
// Padding is applied to the scale & becomes a margin in draw phase...

// Image Pipeline Steps
// - Orient (flip, rotate, etc)
// - Crop source
// - Determine canvas size
// - Determine target pixel format  (does the source have an alpha channel? do any of the filters apply an alpha?)
// - Determine color model
// - Determine target color space