﻿using System;

namespace Carbon.Media.Drawing
{
    public readonly struct UnboundPadding : IEquatable<UnboundPadding>
    {
        public static readonly UnboundPadding Zero = new UnboundPadding();

        public UnboundPadding(Padding value)
        {
            Top    = value.Top;
            Right  = value.Right;
            Bottom = value.Bottom;
            Left   = value.Left;
        }

        public UnboundPadding(Unit value)
        {
            if (value < 0)
            {
                throw ExceptionHelper.MustBeGreaterThanOrEqualToZero(nameof(value), value.Value);
            }

            Top = value;
            Right = value;            
            Bottom = value;
            Left = value;
        }

        public UnboundPadding(Unit top, Unit right, Unit bottom, Unit left)
        {
            if (top < 0)    throw ExceptionHelper.MustBeGreaterThanOrEqualToZero(nameof(top),    top.Value);
            if (right < 0)  throw ExceptionHelper.MustBeGreaterThanOrEqualToZero(nameof(right),  right.Value);
            if (bottom < 0) throw ExceptionHelper.MustBeGreaterThanOrEqualToZero(nameof(bottom), bottom.Value);
            if (left < 0)   throw ExceptionHelper.MustBeGreaterThanOrEqualToZero(nameof(left),   left.Value);

            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public Unit Top { get; }

        public Unit Right { get; }

        public Unit Bottom { get; }

        public Unit Left { get; }

        public override string ToString()
        {
            if (Top == Left && Top == Right && Top == Bottom)
            {
                return Top.ToString();
            }
            else if (Top == Bottom && Right == Left)
            {
                return $"{Top},{Right}";
            }

            return $"{Top},{Right},{Bottom},{Left}";
        }

        public static UnboundPadding Parse(string text)
        {
            return new UnboundPadding(Unit.Parse(text));
        }

        public bool Equals(UnboundPadding other) =>
            Left   == other.Left && 
            Right  == other.Right && 
            Top    == other.Top &&
            Bottom == other.Bottom;
    }
}