using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto
{
	/// <summary>Represents a position inside a text document.</summary>
	public struct TextPosition : IEquatable<TextPosition>
	{
		/// <summary>Gets an empty <see cref="T:TextPosition"/>.</summary>
		public static readonly TextPosition Empty = new TextPosition();

		int charIndex;
		/// <summary>Gets or sets the character index.</summary>
		/// <value>The character index.</value>
		public int CharIndex { get { return charIndex; } }

		int lineNumber;
		/// <summary>Gets or sets the line number.</summary>
		/// <value>The line number.</value>
		public int LineNumber { get { return lineNumber; } }

		int columnNumber;
		/// <summary>Gets or sets the column number.</summary>
		/// <value>The column number.</value>
		public int ColumnNumber { get { return columnNumber; } }

		/// <summary>Moves to next character.</summary>
		public void MoveToNextChar()
		{
			charIndex += 1;
			columnNumber += 1;
			if (lineNumber == 0) lineNumber = 1;
		}

		/// <summary>Moves to the next line.</summary>
		public void MoveToNextLine()
		{
			columnNumber = 0;
			lineNumber += 1;
		}

		/// <summary>Returns a string representing this <see cref="T:TextPosition"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents this <see cref="T:TextPosition"/>.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", lineNumber, columnNumber);
		}

		#region IEquatable<TextPosition> Members

		/// <summary>Indicates whether the current <see cref="T:TextPosition"/> is equal to another <see cref="T:TextPosition"/>.</summary>
		/// <param name="other">A <see cref="T:TextPosition"/> to compare with this <see cref="T:TextPosition"/>.</param>
		/// <returns>true if the current <see cref="T:TextPosition"/> is equal to the other <see cref="T:TextPosition"/>; otherwise, false.</returns>
		public bool Equals(TextPosition other) { return lineNumber == other.lineNumber && columnNumber == other.columnNumber; }

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
            return obj is TextPosition position && Equals(position);
        }

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode() { return lineNumber.GetHashCode() ^ columnNumber.GetHashCode(); }

		/// <summary>Indicates whether the specified <see cref="T:TextPosition"/> values are equal.</summary>
		/// <param name="position1">The first <see cref="T:TextPosition"/>.</param>
		/// <param name="position2">The second <see cref="T:TextPosition"/>.</param>
		/// <returns>If position1 and position2 are equal, true; otherwise, false.</returns>
		public static bool operator ==(TextPosition position1, TextPosition position2) { return position1.Equals(position2); }

		/// <summary>Indicates whether the specified <see cref="T:TextPosition"/> values are not equal.</summary>
		/// <param name="position1">The first <see cref="T:TextPosition"/>.</param>
		/// <param name="position2">The second <see cref="T:TextPosition"/>.</param>
		/// <returns>If position1 and position2 are not equal, true; otherwise, false.</returns>
		public static bool operator !=(TextPosition position1, TextPosition position2) { return !position1.Equals(position2); }

		#endregion
	}
}
