using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Maps words to predefined token types.</summary>
	public static class PredefinedWords
	{
		static readonly Dictionary<string, TokenType> tokenTypes = new Dictionary<string, TokenType>(StringComparer.OrdinalIgnoreCase);
		static readonly Dictionary<string, PredefinedKeyword> keywords = new Dictionary<string, PredefinedKeyword>(StringComparer.OrdinalIgnoreCase);
		static readonly Dictionary<string, DataType> dataTypes = new Dictionary<string, DataType>(StringComparer.OrdinalIgnoreCase);
		static readonly Dictionary<string, PredefinedLiteral> literals = new Dictionary<string, PredefinedLiteral>(StringComparer.OrdinalIgnoreCase);

		static PredefinedWords()
		{
			Add("if", PredefinedKeyword.If);
			Add("then", PredefinedKeyword.Then);
			Add("else", PredefinedKeyword.Else);
			Add("elsif", PredefinedKeyword.Elsif);
			Add("end", PredefinedKeyword.End);
			Add("while", PredefinedKeyword.While);
			Add("do", PredefinedKeyword.Do);
			Add("repeat", PredefinedKeyword.Repeat);
			Add("until", PredefinedKeyword.Until);
			Add("for", PredefinedKeyword.For);
			Add("to", PredefinedKeyword.To);
			Add("by", PredefinedKeyword.By);
			Add("break", PredefinedKeyword.Break);
			Add("exit", PredefinedKeyword.Exit);
			Add("ref", PredefinedKeyword.Ref);
			Add("not", PredefinedKeyword.Not);
			Add("and", PredefinedKeyword.And);
			Add("or", PredefinedKeyword.Or);
			Add("xor", PredefinedKeyword.Xor);
			Add("mod", PredefinedKeyword.Mod);
			Add("switch", PredefinedKeyword.Switch);
			Add("case", PredefinedKeyword.Case);
			Add("var", PredefinedKeyword.Var);

			Add("string", DataType.String);
			Add("integer", DataType.Integer);
			Add("real", DataType.Real);
			Add("logic", DataType.Logic);
			Add("polarity", DataType.Polarity);
			Add("magnetic", DataType.Magnetic);
			Add("curl", DataType.Curl);
			Add("circuit", DataType.Circuit);

			Add("true", PredefinedLiteral.True);
			Add("false", PredefinedLiteral.False);
			Add("pos", PredefinedLiteral.Positive);
			Add("neg", PredefinedLiteral.Negative);
			Add("north", PredefinedLiteral.North);
			Add("south", PredefinedLiteral.South);
			Add("cw", PredefinedLiteral.Clockwise);
			Add("ccw", PredefinedLiteral.Counterclockwise);
			Add("open", PredefinedLiteral.Open);
			Add("closed", PredefinedLiteral.Closed);
		}

		static void Add(string word, TokenType tokenType)
		{
			tokenTypes[word] = tokenType;
		}

		static void Add(string word, PredefinedKeyword keyword)
		{
			Add(word, TokenType.Keyword);
			keywords[word] = keyword;
		}

		static void Add(string word, DataType dataType)
		{
			Add(word, TokenType.DataType);
			dataTypes[word] = dataType;
		}

		static void Add(string word, PredefinedLiteral literal)
		{
			Add(word, TokenType.Literal);
			literals[word] = literal;
		}

		static T GetItem<T>(string word, IDictionary<string, T> collection)
		{
            return collection.TryGetValue(word, out T item) ? item : default;
        }

		/// <summary>Gets the <see cref="T:TokenType"/> of the specified word.</summary>
		/// <param name="word">The word to get the <see cref="T:TokenType"/> of.</param>
		/// <returns>The <see cref="T:TokenType"/> of the specified word.</returns>
		public static TokenType GetTokenType(string word) { return GetItem(word, tokenTypes); }

		/// <summary>Gets the <see cref="T:PredefinedKeyword"/> of the specified word.</summary>
		/// <param name="word">The word to get the <see cref="T:PredefinedKeyword"/> of.</param>
		/// <returns>The <see cref="T:PredefinedKeyword"/> of the specified word.</returns>
		public static PredefinedKeyword GetPredefinedKeyword(string word) { return GetItem(word, keywords); }

		/// <summary>Gets the <see cref="T:DataType"/> of the specified word.</summary>
		/// <param name="word">The word to get the <see cref="T:DataType"/> of.</param>
		/// <returns>The <see cref="T:DataType"/> of the specified word.</returns>
		public static DataType GetPredefinedDataType(string word) { return GetItem(word, dataTypes); }

		/// <summary>Gets the <see cref="T:PredefinedLiteral"/> of the specified word.</summary>
		/// <param name="word">The word to get the <see cref="T:PredefinedLiteral"/> of.</param>
		/// <returns>The <see cref="T:PredefinedLiteral"/> of the specified word.</returns>
		public static PredefinedLiteral GetPredefinedLiteral(string word) { return GetItem(word, literals); }
	}
}
