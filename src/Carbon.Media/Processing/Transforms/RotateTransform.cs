﻿namespace Carbon.Media.Processing
{
    public sealed class RotateTransform : IProcessor
    {
        public RotateTransform(int angle)
        {
            /*
			if (!angle.InRange(0, 360))
				throw new ArgumentOutOfRangeException($"Angle must be between 0 and 360. Was {angle}");
			*/

            Angle = angle;
        }

        public int Angle { get; }

        public string Canonicalize() => $"rotate({Angle}deg)";

        public override string ToString() => $"rotate({Angle})";

        public static RotateTransform Create(in CallSyntax syntax)
        {
            var angle = Unit.Parse(syntax.Arguments[0].Value.ToString());

            return new RotateTransform(angle: (int)angle.Value);
        }
    }
}