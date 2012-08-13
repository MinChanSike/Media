﻿namespace Carbon.Media
{
	using System;

	using Carbon.Helpers;

	public class Rotate : ITransform
	{
		private readonly int angle;

		public Rotate(int angle)
		{
			#region Preconditions

			/*
			if (!angle.InRange(0, 360))
				throw new ArgumentOutOfRangeException("Angle must be between 0 and 360. Was {0}".FormatWith(angle));
			*/

			#endregion

			this.angle = angle;
		}

		public int Angle 
		{
			get { return angle; }
		}

		public string ToKey()
		{
			return "rotate:" + angle;
		}

		public static Rotate ParseKey(string key)
		{
			#region Normalization

			if (key.StartsWith("rotate:"))
			{
				key = key.Remove(0, 7);
			}

			#endregion

			var angle = Int32.Parse(key);

			return new Rotate(angle);
		}
	}
}