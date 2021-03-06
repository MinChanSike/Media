﻿using System.Text;

namespace Carbon.Media.Processing
{
    public sealed class VibranceFilter : IFilter, ICanonicalizable
    {
        public VibranceFilter(float amount)
        {
            if (amount < -10 || amount > 10)
            {
                throw ExceptionHelper.OutOfRange(nameof(amount), -10, 10, amount);
            }

            Amount = amount;
        }

        public float Amount { get; }

        #region ICanonicalizable

        public string Canonicalize()
        {
            var sb = StringBuilderCache.Aquire();

            WriteTo(sb);

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        public void WriteTo(StringBuilder sb)
        {
            sb.Append("vibrance(");
            sb.Append(Amount);
            sb.Append(')');
        }

        public override string ToString() => Canonicalize();

        #endregion

        public static VibranceFilter Create(in CallSyntax syntax)
        {
            return new VibranceFilter(float.Parse(syntax.Arguments[0].Value.ToString()));
        }
    }
}