using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents a predefined keyword token.</summary>
	public class PredefinedKeywordToken : Token
	{
		/// <summary>Initializes a new instance of the <see cref="PredefinedKeywordToken"/> class.</summary>
		/// <param name="keyword">The keyword.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		internal PredefinedKeywordToken(PredefinedKeyword keyword, TextPosition startPosition, TextPosition endPosition)
			: base(TokenType.Keyword, startPosition, endPosition)
		{
			this.keyword = keyword;
		}

		readonly PredefinedKeyword keyword;
		/// <summary>Gets the keyword.</summary>
		/// <value>The keyword.</value>
		public PredefinedKeyword Keyword { get { return keyword; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", base.ToString(), keyword); }

		/// <summary>Checks whether this token is the specified predefined keyword token.</summary>
		/// <param name="keyword">The predefined keyword to check.</param>
		/// <returns>If this token equals the specified keyword, true; otherwise, false.</returns>
		public override bool Equals(PredefinedKeyword keyword) { return this.keyword == keyword; }
	}
}
