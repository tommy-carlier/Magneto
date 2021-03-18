using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TC.Magneto
{
	/// <summary>Contains the core functions.</summary>
	public sealed class CoreFunctions
	{
		private CoreFunctions() { }

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ConvertToString(long value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ConvertToString(decimal value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ConvertToString(bool value)
		{
			return value ? "true" : "false";
		}

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ConvertToString(Polarity value)
		{
			switch (value)
			{
				case Polarity.Positive: return "pos";
				case Polarity.Negative: return "neg";
				default: return "";
			}
		}

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ConvertToString(Magnetic value)
		{
			switch (value)
			{
				case Magnetic.North: return "north";
				case Magnetic.South: return "south";
				default: return "";
			}
		}

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ConvertToString(Curl value)
		{
			switch (value)
			{
				case Curl.Clockwise: return "cw";
				case Curl.Counterclockwise: return "ccw";
				default: return "";
			}
		}

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ConvertToString(Circuit value)
		{
			switch (value)
			{
				case Circuit.Open: return "open";
				case Circuit.Closed: return "closed";
				default: return "";
			}
		}

		/// <summary>Calculates base raised to the specified power.</summary>
		/// <param name="base">The base.</param>
		/// <param name="power">The power.</param>
		/// <returns>base raised to the specified power.</returns>
		public static decimal Power(decimal @base, long power)
		{
			decimal result = 1L;
			if (power > 0L)
				while ((power -= 1) >= 0L)
					result *= @base;
			else if (power < 0L)
				while ((power += 1) <= 0L)
					result /= @base;
			return result;
		}

		/// <summary>Checks whether to continue the for-loop.</summary>
		/// <param name="loopValue">The value of the loop-variable.</param>
		/// <param name="endValue">The end value of the loop.</param>
		/// <param name="stepValue">The step value.</param>
		/// <returns>If the loop should continue, true; otherwise, false.</returns>
		public static bool CheckForNext(long loopValue, long endValue, long stepValue)
		{
			return
				stepValue >= 0
					? loopValue <= endValue
					: loopValue >= endValue;
		}

		/// <summary>Checks whether to continue the for-loop.</summary>
		/// <param name="loopValue">The value of the loop-variable.</param>
		/// <param name="endValue">The end value of the loop.</param>
		/// <param name="stepValue">The step value.</param>
		/// <returns>If the loop should continue, true; otherwise, false.</returns>
		public static bool CheckForNext(decimal loopValue, decimal endValue, decimal stepValue)
		{
			return
				decimal.Compare(stepValue, decimal.Zero) >= 0
					? decimal.Compare(loopValue, endValue) <= 0
					: decimal.Compare(loopValue, endValue) >= 0;
		}
	}
}
