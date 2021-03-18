using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents an integer literal token.</summary>
	public class IntegerLiteralToken : LiteralToken
	{
		/// <summary>Initializes a new instance of the <see cref="IntegerLiteralToken"/> class.</summary>
		/// <param name="value">The value of the literal.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		internal IntegerLiteralToken(long value, TextPosition startPosition, TextPosition endPosition)
			: base(DataType.Integer, startPosition, endPosition)
		{
			this.value = value;
		}

		readonly long value;
		/// <summary>Gets the value.</summary>
		/// <value>The value.</value>
		public long Value { get { return value; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", base.ToString(), value); }
	}
}
