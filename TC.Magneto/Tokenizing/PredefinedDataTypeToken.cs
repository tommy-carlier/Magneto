using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents	a predefined data type token.</summary>
	public class PredefinedDataTypeToken : Token
	{
		/// <summary>Initializes a new instance of the <see cref="PredefinedDataTypeToken"/> class.</summary>
		/// <param name="dataType">The data type.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		internal PredefinedDataTypeToken(DataType dataType, TextPosition startPosition, TextPosition endPosition)
			: base(TokenType.DataType, startPosition, endPosition)
		{
			this.dataType = dataType;
		}

		readonly DataType dataType;
		/// <summary>Gets the data type.</summary>
		/// <value>The data type.</value>
		public DataType DataType { get { return dataType; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", base.ToString(), dataType); }
	}
}
