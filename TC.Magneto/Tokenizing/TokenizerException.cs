using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents an error that occurred while tokenizing source code.</summary>
	public class TokenizerException : MagnetoException
	{
		/// <summary>Initializes a new instance of the <see cref="TokenizerException"/> class.</summary>
		/// <param name="message">The error message.</param>
		/// <param name="position">The position.</param>
		public TokenizerException(string message, TextPosition position) : base(message, position) { }
	}
}
