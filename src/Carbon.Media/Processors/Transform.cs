﻿namespace Carbon.Media.Processors
{
    public static class Transform
    {
        // TODO: Remove colon once crop has been migrated to new syntax

        private static readonly char[] argStartChars = { '(', ':' };

        public static ITransform Parse(string segment)
        {
            var indexOfArg = segment.IndexOfAny(argStartChars);

            var name = indexOfArg > -1
                ? segment.Substring(0, indexOfArg)
                : segment;

            switch (name)
            {
                case "resize"       : return Resize.Parse(segment);
                case "scale"        : return Scale.Parse(segment);
                case "crop"         : return Crop.Parse(segment);
                case "rotate"       : return Rotate.Parse(segment);
                case "flip"         : return Flip.Parse(segment);
                case "pad"          : return Pad.Parse(segment);

                case "page"         : return Page.Parse(segment);
                    
                // drawing            
                case "text"         : return DrawText.Parse(segment);
                case "overlay"      : return DrawColor.Parse(segment);
                case "gradient"     : return DrawGradient.Parse(segment);
                case "path"         : return DrawPath.Parse(segment);
                case "circle"       : return DrawCircle.Parse(segment);

                // filters           
                case "background"   : return Background.Parse(segment);
                case "bg"           : return Background.Parse(segment);
                case "blur"         : return BlurFilter.Parse(segment);
                case "brightness"   : return BrightnessFilter.Parse(segment);

                // web filters
                case "hue-rotate"   : return HueRotateFilter.Parse(segment);
                case "saturate"     : return SaturateFilter.Parse(segment);
                case "sepia"        : return SepiaFilter.Parse(segment);
                case "grayscale"    : return GrayscaleFilter.Parse(segment);
                case "invert"       : return InvertFilter.Parse(segment);
                case "contrast"     : return ContrastFilter.Parse(segment);
                case "opacity"      : return OpacityFilter.Parse(segment);
                case "quality"      : return Quality.Parse(segment);
                default             :

                    // JPEG::encode

                    if (segment.Contains("encode"))
                    {
                        return ImageEncode.Parse(segment);
                    }
                    else
                    {
                        return CustomFilter.Parse(segment);
                    }
            }
        }
    }
}
