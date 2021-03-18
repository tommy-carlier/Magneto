using System;
using System.Collections.Generic;
using System.Text;
using TC.Magneto.Tokenizing;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents an error that occurred while parsing source code.</summary>
	public class ParserException : MagnetoException
	{
		/// <summary>Initializes a new instance of the <see cref="ParserException"/> class.</summary>
		/// <param name="message">The error message.</param>
		/// <param name="position">The position.</param>
		public ParserException(string message, TextPosition position) : base(message, position) { }
	}
}
