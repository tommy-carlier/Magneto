using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Modules
{
	/// <summary>Contains string functions and constants.</summary>
	[Constant("tab", "\t"), Constant("newLine", "\r\n"), Constant("quote", "\"")]
	public sealed class StringModule : MagnetoModule
	{
		/// <summary>Gets the length of the specified string.</summary>
		/// <param name="s">The string to get the length of.</param>
		/// <returns>The length of the specified string.</returns>
		[Function("length")]
		public long GetLength(string s) { return s != null ? s.Length : 0; }

		/// <summary>Gets a substring of the specified string.</summary>
		/// <param name="s">The string to get a substring of.</param>
		/// <param name="startIndex">The zero-based character position that indicates the start of the substring.</param>
		/// <returns>The substring.</returns>
		[Function("substring")]
		public string Substring(string s, long startIndex)
		{
			if (string.IsNullOrEmpty(s) || startIndex >= s.Length)
				return "";

			return startIndex > 0L ? s.Substring((int)startIndex) : s;
		}

		/// <summary>Gets a substring of the specified string.</summary>
		/// <param name="s">The string to get a substring of.</param>
		/// <param name="startIndex">The zero-based character position that indicates the start of the substring.</param>
		/// <param name="length">The length of the substring.</param>
		/// <returns>The substring.</returns>
		[Function("substring")]
		public string Substring(string s, long startIndex, long length)
		{
			if (string.IsNullOrEmpty(s) || startIndex >= s.Length || length <= 0L)
				return "";

			return startIndex > 0L
				? length < s.Length - startIndex
					? s.Substring((int)startIndex, (int)length)
					: s.Substring((int)startIndex)
				: s;
		}

		/// <summary>Gets a substring of the first characters of the specified string.</summary>
		/// <param name="s">The string to get a substring of.</param>
		/// <param name="length">The length of the substring.</param>
		/// <returns>The substring of the first characters of the specified string.</returns>
		[Function("left")]
		public string Left(string s, long length)
		{
			if (string.IsNullOrEmpty(s) || length <= 0L)
				return "";

			return length < s.Length ? s.Substring(0, (int)length) : s;
		}

		/// <summary>Gets a substring of the last characters of the specified string.</summary>
		/// <param name="s">The string to get a substring of.</param>
		/// <param name="length">The length of the substring.</param>
		/// <returns>The substring of the last characters of the specified string.</returns>
		[Function("right")]
		public string Right(string s, long length)
		{
			if (string.IsNullOrEmpty(s) || length <= 0L)
				return "";

			return length < s.Length ? s.Substring(s.Length - (int)length) : s;
		}

		/// <summary>Gets the index of the first occurrence of the specified value in the specified string.</summary>
		/// <param name="s">The string to search through.</param>
		/// <param name="value">The value to search.</param>
		/// <returns>The zero-based index of the first occurrence of the specified value in the specified string.</returns>
		[Function("find")]
		public long Find(string s, string value)
		{
			return
				string.IsNullOrEmpty(s) || string.IsNullOrEmpty(value)
				? -1L : s.IndexOf(value, StringComparison.InvariantCulture);
		}

		/// <summary>Gets the index of the first occurrence of the specified value in the specified string.</summary>
		/// <param name="s">The string to search through.</param>
		/// <param name="value">The value to search.</param>
		/// <param name="startIndex">The character position to start searching from.</param>
		/// <returns>The zero-based index of the first occurrence of the specified value in the specified string, starting from the specified position.</returns>
		[Function("find")]
		public long Find(string s, string value, long startIndex)
		{
			return
				string.IsNullOrEmpty(s) || string.IsNullOrEmpty(value)
				|| value.Length > s.Length || startIndex >= s.Length - value.Length
				? -1L : s.IndexOf(value, (int)startIndex, StringComparison.InvariantCulture);
		}

		/// <summary>Indicates whether the specified string contains the specified value.</summary>
		/// <param name="s">The string to search through.</param>
		/// <param name="value">The value to search.</param>
		/// <returns>If the specified string contains the specified value, true; otherwise, false.</returns>
		[Function("contains")]
		public bool Contains(string s, string value)
		{
			return string.IsNullOrEmpty(s)
				? string.IsNullOrEmpty(value)
				: !string.IsNullOrEmpty(value) && s.Contains(value);
		}

		/// <summary>Replaces all occurrences of a value in the specified string with another value.</summary>
		/// <param name="s">The string to replace values of.</param>
		/// <param name="oldValue">The value to replace.</param>
		/// <param name="newValue">The value that replaces the other value.</param>
		/// <returns>The specified string, with the replaced values.</returns>
		[Function("replace")]
		public string Replace(string s, string oldValue, string newValue)
		{
			return
				string.IsNullOrEmpty(s)
					? "" : string.IsNullOrEmpty(oldValue)
					? s : s.Replace(oldValue, newValue ?? "");
		}

		/// <summary>Converts the specified string to uppercase.</summary>
		/// <param name="s">The string to convert.</param>
		/// <returns>The specified string, converted to uppercase.</returns>
		[Function("upper")]
		public string Upper(string s) { return string.IsNullOrEmpty(s) ? "" : s.ToUpperInvariant(); }

		/// <summary>Converts the specified string to lowercase.</summary>
		/// <param name="s">The string to convert.</param>
		/// <returns>The specified string, converted to lowercase.</returns>
		[Function("lower")]
		public string Lower(string s) { return string.IsNullOrEmpty(s) ? "" : s.ToLowerInvariant(); }

		/// <summary>Removes all leading and trailing white-space characters from the specified string.</summary>
		/// <param name="s">The string to trim.</param>
		/// <returns>The trimmed string.</returns>
		[Function("trim")]
		public string Trim(string s) { return string.IsNullOrEmpty(s) ? "" : s.Trim(); }

		/// <summary>Gets the character with the specified code.</summary>
		/// <param name="charCode">The Unicode code of the character to get.</param>
		/// <returns>The character with the specified code.</returns>
		[Function("char")]
		public string Char(long charCode)
		{
			return charCode >= 0L && charCode <= 0x10FFFFL ? System.Char.ConvertFromUtf32((int)charCode) : "";
		}

		/// <summary>Gets the Unicode code of the first character of the specified string.</summary>
		/// <param name="s">The string to get the Unicode code of the first character of.</param>
		/// <returns>The Unicode code of the first character of the specified string.</returns>
		[Function("charCode")]
		public long CharCode(string s)
		{
			return string.IsNullOrEmpty(s) ? 0L : System.Char.ConvertToUtf32(s, 0);
		}
	}
}
