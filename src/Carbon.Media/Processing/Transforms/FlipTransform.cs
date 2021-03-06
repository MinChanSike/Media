﻿using System;
using System.Text;

namespace Carbon.Media.Processing
{
    public sealed class FlipTransform : IProcessor, ICanonicalizable
    {
        public static readonly FlipTransform Horizontally = new FlipTransform(FlipAxis.X);
        public static readonly FlipTransform Vertically   = new FlipTransform(FlipAxis.Y);

        public FlipTransform(FlipAxis axis)
        {
            Axis = axis;
        }

        public FlipAxis Axis { get; }

        #region ICanonicalizable

        // flip(x | y)
        public string Canonicalize()
        {
            var sb = StringBuilderCache.Aquire();

            WriteTo(sb);

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        public void WriteTo(StringBuilder sb)
        {
            sb.Append("flip(");
            sb.Append(Axis.ToLower());
            sb.Append(')');
        }

        public override string ToString() => Canonicalize();

        #endregion

        public static FlipTransform Create(in CallSyntax syntax)
        {
            switch (syntax.Arguments[0].Value)
            {
                case "x" : return Horizontally;
                case "y" : return Vertically;
            }

            throw new ArgumentException("Invalid flip axis:" + syntax.Arguments[0].Value);
        }
    }

    internal static class FlipAxisExtensions
    {
        public static string ToLower(this FlipAxis axis)
        {
            switch (axis)
            {
                case FlipAxis.X : return "x";
                case FlipAxis.Y : return "y";
                default         : return "unknown";
            }
        }
    }

    public enum FlipAxis
    {
        X = 1, // Horizontally
        Y = 2  // Veritical
    }
}