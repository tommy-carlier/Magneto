using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents a predefined symbol token.</summary>
	public class PredefinedSymbolToken : Token
	{
		/// <summary>Initializes a new instance of the <see cref="PredefinedSymbolToken"/> class.</summary>
		/// <param name="symbol">The symbol.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		internal PredefinedSymbolToken(PredefinedSymbol symbol, TextPosition startPosition, TextPosition endPosition)
			: base(TokenType.Symbol, startPosition, endPosition)
		{
			this.symbol = symbol;
		}

		readonly PredefinedSymbol symbol;
		/// <summary>Gets the symbol.</summary>
		/// <value>The symbol.</value>
		public PredefinedSymbol Symbol { get { return symbol; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", base.ToString(), symbol); }

		/// <summary>Checks whether this token is the specified predefined symbol token.</summary>
		/// <param name="symbol">The predefined symbol to check.</param>
		/// <returns>If this token equals the specified symbol, true; otherwise, false.</returns>
		public override bool Equals(PredefinedSymbol symbol) { return this.symbol == symbol; }
	}
}
