using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents a literal token.</summary>
	public abstract class LiteralToken : Token
	{
		/// <summary>Initializes a new instance of the <see cref="LiteralToken"/> class.</summary>
		/// <param name="dataType">The data type of the literal.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		protected LiteralToken(DataType dataType, TextPosition startPosition, TextPosition endPosition)
			: base(TokenType.Literal, startPosition, endPosition)
		{
			this.dataType = dataType;
		}

		readonly DataType dataType;
		/// <summary>Gets the data type of the literal.</summary>
		/// <value>The data type of the literal.</value>
		public DataType DataType { get { return dataType; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "{0} {1}", dataType, base.ToString()); }
	}
}
