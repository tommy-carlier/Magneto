using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Tokenizing
{
	/// <summary>The type of a <see cref="T:Token"/>.</summary>
	public enum TokenType
	{
		/// <summary>No token type.</summary>
		None = 0,

		/// <summary>A keyword token.</summary>
		Keyword,

		/// <summary>A data type token.</summary>
		DataType,

		/// <summary>An identifier token.</summary>
		Identifier,

		/// <summary>A literal token.</summary>
		Literal,

		/// <summary>A symbol token.</summary>
		Symbol,

		/// <summary>A comment token.</summary>
		Comment
	}
}
