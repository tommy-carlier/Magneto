using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Modules
{
	/// <summary>Contains mathematical functions.</summary>
	public class MathModule : MagnetoModule
	{
		/// <summary>Gets the absolute value of the specified value.</summary>
		/// <param name="value">The value to get the absolute value of.</param>
		/// <returns>The absolute value of the specified value.</returns>
		[Function("abs")]
		public long Abs(long value) { return Math.Abs(value); }

		/// <summary>Gets the absolute value of the specified value.</summary>
		/// <param name="value">The value to get the absolute value of.</param>
		/// <returns>The absolute value of the specified value.</returns>
		[Function("abs")]
		public decimal Abs(decimal value) { return Math.Abs(value); }

		/// <summary>Gets the largest integer less than or equal to the specified value.</summary>
		/// <param name="value">The value to floor.</param>
		/// <returns>The largest integer less than or equal to the specified value.</returns>
		[Function("floor")]
		public decimal Floor(decimal value) { return Math.Floor(value); }

		/// <summary>Rounds the specified value to the nearest integer.</summary>
		/// <param name="value">The value to round.</param>
		/// <returns>The integer nearest the specified value.</returns>
		[Function("round")]
		public decimal Round(decimal value) { return Math.Round(value, MidpointRounding.AwayFromZero); }

		/// <summary>Rounds the specified value to the specified precision.</summary>
		/// <param name="value">The value to round.</param>
		/// <param name="decimals">The number of decimal places in the return value.</param>
		/// <returns>The decimal number nearest the specified value with the specified precision.</returns>
		[Function("round")]
		public decimal Round(decimal value, long decimals)
		{
			return decimals <= 0L ? Math.Round(value, MidpointRounding.AwayFromZero)
				: decimals > 28L ? value
				: Math.Round(value, (int)decimals, MidpointRounding.AwayFromZero);
		}

		/// <summary>Gets the sign of the specified value.</summary>
		/// <param name="value">The value to get the sign of.</param>
		/// <returns>If value is negative, -1; if value is positive, 1; if value is zero, 0.</returns>
		[Function("sign")]
		public long Sign(long value) { return Math.Sign(value); }

		/// <summary>Gets the sign of the specified value.</summary>
		/// <param name="value">The value to get the sign of.</param>
		/// <returns>If value is negative, -1; if value is positive, 1; if value is zero, 0.</returns>
		[Function("sign")]
		public long Sign(decimal value) { return Math.Sign(value); }
	}
}
