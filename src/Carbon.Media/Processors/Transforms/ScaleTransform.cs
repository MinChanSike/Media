﻿using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text;

namespace Carbon.Media.Processors
{
    public sealed class ScaleTransform : IProcessor, ICanonicalizable
    {
        public ScaleTransform(Size size, InterpolaterMode mode)
            : this(size.Width, size.Height, mode) { }

        public ScaleTransform(int width, int height, InterpolaterMode mode)
        {
            if (width < 0 || width > Constants.MaxWidth)
                throw new ArgumentOutOfRangeException(nameof(width), width, message: "Must be between 0 and 16,384");

            if (height < 0 || height > Constants.MaxHeight)
                throw new ArgumentOutOfRangeException(nameof(height), height, message: "Must be between 0 and 16,384");

            Width = width;
            Height = height;
            Mode   = mode;
        }

        public int Width { get; }

        public int Height { get; }

        public InterpolaterMode Mode { get; }

        [IgnoreDataMember]
        public Size Size => new Size(Width, Height);

        #region ICanonicalizable

        // scale(100,100,lanczos3)
        public string Canonicalize()
        {
            var sb = StringBuilderCache.Aquire();

            WriteTo(sb);

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        public void WriteTo(StringBuilder sb)
        {
            sb.Append("scale(");

            sb.Append(Width);
            sb.Append(',');
            sb.Append(Height);

            if (Mode != default)
            {
                sb.Append(',');
                sb.Append(Mode.ToLower());
            }

            sb.Append(')');
        }

        public override string ToString() => Canonicalize();

        #endregion

        public static ScaleTransform Parse(string segment)
        {
            int argStart = segment.IndexOf('(') + 1;

            var args = ArgumentList.Parse(segment.Substring(argStart, segment.Length - argStart - 1));

            int width = 0;
            int height = 0;
            var mode = InterpolaterMode.Box;

            var i = 0;

            foreach (var (key, value) in args)
            {
                switch (i)
                {
                    case 0: width  = int.Parse(value); break;
                    case 1: height = int.Parse(value); break;
                    case 2: mode   = InterpolaterHelper.Parse(value); break;
                }

                i++;
            }

            return new ScaleTransform(width, height, mode);
        }
    }
}