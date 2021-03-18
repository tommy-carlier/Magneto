using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents a predefined literal token.</summary>
	public class PredefinedLiteralToken : LiteralToken
	{
		/// <summary>Initializes a new instance of the <see cref="PredefinedLiteralToken"/> class.</summary>
		/// <param name="value">The value of the literal.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		internal PredefinedLiteralToken(PredefinedLiteral value, TextPosition startPosition, TextPosition endPosition)
			: base(GetType(value), startPosition, endPosition)
		{
			this.value = value;
		}

		readonly PredefinedLiteral value;
		/// <summary>Gets the value.</summary>
		/// <value>The value.</value>
		public PredefinedLiteral Value { get { return value; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", base.ToString(), value); }

		/// <summary>Checks whether this token is the specified predefined literal token.</summary>
		/// <param name="literal">The predefined literal to check.</param>
		/// <returns>If this token equals the specified literal, true; otherwise, false.</returns>
		public override bool Equals(PredefinedLiteral literal) { return value == literal; }

		static DataType GetType(PredefinedLiteral literal)
		{
			switch (literal)
			{
				case PredefinedLiteral.True:
				case PredefinedLiteral.False: return DataType.Logic;
				case PredefinedLiteral.Positive:
				case PredefinedLiteral.Negative: return DataType.Polarity;
				case PredefinedLiteral.North:
				case PredefinedLiteral.South: return DataType.Magnetic;
				case PredefinedLiteral.Clockwise:
				case PredefinedLiteral.Counterclockwise: return DataType.Curl;
				case PredefinedLiteral.Open:
				case PredefinedLiteral.Closed: return DataType.Circuit;
				default: return DataType.None;
			}
		}
	}
}
