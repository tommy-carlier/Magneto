using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Modules
{
	/// <summary>Contains conversion functions.</summary>
	public sealed class ConversionModule : MagnetoModule
	{
		static readonly StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

		#region ConvertToString

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		[Function("toString")]
		public string ConvertToString(string value) { return value; }

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		[Function("toString")]
		public string ConvertToString(long value) { return CoreFunctions.ConvertToString(value); }

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		[Function("toString")]
		public string ConvertToString(decimal value) { return CoreFunctions.ConvertToString(value); }

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		[Function("toString")]
		public string ConvertToString(bool value) { return CoreFunctions.ConvertToString(value); }

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		[Function("toString")]
		public string ConvertToString(Polarity value) { return CoreFunctions.ConvertToString(value); }

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		[Function("toString")]
		public string ConvertToString(Magnetic value) { return CoreFunctions.ConvertToString(value); }

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		[Function("toString")]
		public string ConvertToString(Curl value) { return CoreFunctions.ConvertToString(value); }

		/// <summary>Converts the specified value to string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		[Function("toString")]
		public string ConvertToString(Circuit value) { return CoreFunctions.ConvertToString(value); }

		#endregion

		#region ConvertToInteger

		/// <summary>Converts the specified value to integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted integer.</returns>
		[Function("toInteger")]
		public long ConvertToInteger(string value) { return long.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture); }

		/// <summary>Converts the specified value to integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted integer.</returns>
		[Function("toInteger")]
		public long ConvertToInteger(long value) { return value; }

		/// <summary>Converts the specified value to integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted integer.</returns>
		[Function("toInteger")]
		public long ConvertToInteger(decimal value) { return (long)Math.Floor(value); }

		#endregion

		#region ConvertToReal

		/// <summary>Converts the specified value to real.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted real.</returns>
		[Function("toReal")]
		public decimal ConvertToReal(string value) { return decimal.Parse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture); }

		/// <summary>Converts the specified value to real.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted real.</returns>
		[Function("toReal")]
		public decimal ConvertToReal(long value) { return (decimal)value; }

		/// <summary>Converts the specified value to real.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted real.</returns>
		[Function("toReal")]
		public decimal ConvertToReal(decimal value) { return value; }

		#endregion

		#region ConvertToLogic

		/// <summary>Converts the specified value to logic.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted logic.</returns>
		[Function("toLogic")]
		public bool ConvertToLogic(string value)
		{
			if (stringComparer.Equals(value, "true")) return true;
			if (stringComparer.Equals(value, "false")) return false;
			throw new FormatException("Cannot convert to logic (invalid format).");
		}

		/// <summary>Converts the specified value to logic.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted logic.</returns>
		[Function("toLogic")]
		public bool ConvertToLogic(bool value) { return value; }

		#endregion

		#region ConvertToPolarity

		/// <summary>Converts the specified value to polarity.</summary>
		/// <param name="value">The value to converted.</param>
		/// <returns>The converted polarity.</returns>
		[Function("toPolarity")]
		public Polarity ConvertToPolarity(string value)
		{
			if (stringComparer.Equals(value, "p") || stringComparer.Equals(value, "pos")
				|| stringComparer.Equals(value, "positive") || string.Equals(value, "+"))
				return Polarity.Positive;
			if (stringComparer.Equals(value, "n") || stringComparer.Equals(value, "neg") 
				|| stringComparer.Equals(value, "negative") || string.Equals(value, "-"))
				return Polarity.Negative;
			throw new FormatException("Cannot convert to polarity (invalid format).");
		}

		/// <summary>Converts the specified value to polarity.</summary>
		/// <param name="value">The value to converted.</param>
		/// <returns>The converted polarity.</returns>
		[Function("toPolarity")]
		public Polarity ConvertToPolarity(Polarity value) { return value; }

		#endregion

		#region ConvertToMagnetic

		/// <summary>Converts the specified value to magnetic.</summary>
		/// <param name="value">The value to converted.</param>
		/// <returns>The converted magnetic.</returns>
		[Function("toMagnetic")]
		public Magnetic ConvertToMagnetic(string value)
		{
			if (stringComparer.Equals(value, "north") || stringComparer.Equals(value, "n"))
				return Magnetic.North;
			if (stringComparer.Equals(value, "south") || stringComparer.Equals(value, "s"))
				return Magnetic.South;
			throw new FormatException("Cannot convert to magnetic (invalid format).");
		}

		/// <summary>Converts the specified value to magnetic.</summary>
		/// <param name="value">The value to converted.</param>
		/// <returns>The converted magnetic.</returns>
		[Function("toMagnetic")]
		public Magnetic ConvertToMagnetic(Magnetic value) { return value; }

		#endregion

		#region ConvertToCurl

		/// <summary>Converts the specified value to curl.</summary>
		/// <param name="value">The value to converted.</param>
		/// <returns>The converted curl.</returns>
		[Function("toCurl")]
		public Curl ConvertToCurl(string value)
		{
			if (stringComparer.Equals(value, "cw") || stringComparer.Equals(value, "clockwise"))
				return Curl.Clockwise;
			if (stringComparer.Equals(value, "ccw") || stringComparer.Equals(value, "counterclockwise"))
				return Curl.Counterclockwise;
			throw new FormatException("Cannot convert to curl (invalid format).");
		}

		/// <summary>Converts the specified value to curl.</summary>
		/// <param name="value">The value to converted.</param>
		/// <returns>The converted curl.</returns>
		[Function("toCurl")]
		public Curl ConvertToCurl(Curl value) { return value; }

		#endregion

		#region ConvertToCircuit

		/// <summary>Converts the specified value to circuit.</summary>
		/// <param name="value">The value to converted.</param>
		/// <returns>The converted circuit.</returns>
		[Function("toCircuit")]
		public Circuit ConvertToCircuit(string value)
		{
			if (stringComparer.Equals(value, "open") || stringComparer.Equals(value, "o")) return Circuit.Open;
			if (stringComparer.Equals(value, "closed") || stringComparer.Equals(value, "c")) return Circuit.Closed;
			throw new FormatException("Cannot convert to circuit (invalid format).");
		}

		/// <summary>Converts the specified value to circuit.</summary>
		/// <param name="value">The value to converted.</param>
		/// <returns>The converted circuit.</returns>
		[Function("toCircuit")]
		public Circuit ConvertToCircuit(Circuit value) { return value; }

		#endregion
	}
}
