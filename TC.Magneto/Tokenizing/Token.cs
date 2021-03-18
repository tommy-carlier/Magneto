using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents a token, read by a <see cref="T:Tokenizer"/>.</summary>
	public abstract class Token
	{
		/// <summary>Initializes a new instance of the <see cref="Token"/> class.</summary>
		/// <param name="tokenType">The type of the token.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		protected Token(TokenType tokenType, TextPosition startPosition, TextPosition endPosition)
		{
			this.tokenType = tokenType;
			this.startPosition = startPosition;
			this.endPosition = endPosition;
		}

		readonly TokenType tokenType;
		/// <summary>Gets the type of the token.</summary>
		/// <value>The type of the token.</value>
		public TokenType TokenType { get { return tokenType; } }

		readonly TextPosition startPosition;
		/// <summary>Gets the start position.</summary>
		/// <value>The start position.</value>
		public TextPosition StartPosition { get { return startPosition; } }

		readonly TextPosition endPosition;
		/// <summary>Gets the end position.</summary>
		/// <value>The end position.</value>
		public TextPosition EndPosition { get { return endPosition; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", tokenType, startPosition, endPosition);
		}

		/// <summary>Checks whether this token is the specified predefined keyword token.</summary>
		/// <param name="keyword">The predefined keyword to check.</param>
		/// <returns>If this token equals the specified keyword, true; otherwise, false.</returns>
		public virtual bool Equals(PredefinedKeyword keyword) { return false; }

		/// <summary>Checks whether this token is the specified predefined symbol token.</summary>
		/// <param name="symbol">The predefined symbol to check.</param>
		/// <returns>If this token equals the specified symbol, true; otherwise, false.</returns>
		public virtual bool Equals(PredefinedSymbol symbol) { return false; }

		/// <summary>Checks whether this token is the specified predefined literal token.</summary>
		/// <param name="literal">The predefined literal to check.</param>
		/// <returns>If this token equals the specified literal, true; otherwise, false.</returns>
		public virtual bool Equals(PredefinedLiteral literal) { return false; }
	}
}
