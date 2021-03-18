using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents an identifier token.</summary>
	public class IdentifierToken : Token
	{
		/// <summary>Initializes a new instance of the <see cref="IdentifierToken"/> class.</summary>
		/// <param name="identifier">The identifier.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		internal IdentifierToken(string identifier, TextPosition startPosition, TextPosition endPosition)
			: base(TokenType.Identifier, startPosition, endPosition)
		{
			this.identifier = identifier;
		}

		readonly string identifier;
		/// <summary>Gets the identifier.</summary>
		/// <value>The identifier.</value>
		public string Identifier { get { return identifier; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", base.ToString(), identifier); }
	}
}
