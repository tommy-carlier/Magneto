using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents a comment token.</summary>
	public class CommentToken : Token
	{
		/// <summary>Initializes a new instance of the <see cref="CommentToken"/> class.</summary>
		/// <param name="comment">The comment.</param>
		/// <param name="startPosition">The start position.</param>
		/// <param name="endPosition">The end position.</param>
		public CommentToken(string comment, TextPosition startPosition, TextPosition endPosition)
			: base(TokenType.Comment, startPosition, endPosition)
		{
			this.comment = comment;
		}

		readonly string comment;
		/// <summary>Gets the comment.</summary>
		/// <value>The comment.</value>
		public string Comment { get { return comment; } }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Token"/>.</returns>
		public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", base.ToString(), comment); }
	}
}
