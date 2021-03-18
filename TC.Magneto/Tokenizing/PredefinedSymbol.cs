using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents one of the predefined symbols.</summary>
	public enum PredefinedSymbol
	{
		/// <summary>No predefined symbol.</summary>
		None = 0,

		/// <summary>The predefined symbol '+'.</summary>
		Plus,

		/// <summary>The predefined symbol '-'.</summary>
		Minus,

		/// <summary>The predefined symbol ''.</summary>
		Asterisk,

		/// <summary>The predefined symbol '/'.</summary>
		Slash,

		/// <summary>The predefined symbol '^'.</summary>
		Caret,

		/// <summary>The predefined symbol '&amp;'.</summary>
		Ampersand,

		/// <summary>The predefined symbol '('.</summary>
		OpenParenthesis,

		/// <summary>The predefined symbol ')'.</summary>
		ClosedParenthesis,

		/// <summary>The predefined symbol ','.</summary>
		Comma,

		/// <summary>The predefined symbol ':'.</summary>
		Colon,

		/// <summary>The predefined symbol ':='.</summary>
		Assignment,

		/// <summary>The predefined symbol '=='.</summary>
		Equal,

		/// <summary>The predefined symbol '&lt;&gt;'.</summary>
		NotEqual,

		/// <summary>The predefined symbol '&lt;'.</summary>
		LessThan,

		/// <summary>The predefined symbol '&gt;'.</summary>
		GreaterThan,

		/// <summary>The predefined symbol '&lt;='.</summary>
		LessThanOrEqual,

		/// <summary>The predefined symbol '&gt;='.</summary>
		GreaterThanOrEqual
	}
}
