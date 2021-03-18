using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Converts source code into a sequence of tokens.</summary>
	public class Tokenizer
	{
		readonly TextReader reader;
		readonly StringBuilder tokenBuilder;

		bool atEndOfSource;
		char currentChar;
		TextPosition previousPosition, currentPosition, startPosition;

		/// <summary>Initializes a new instance of the <see cref="Tokenizer"/> class.</summary>
		/// <param name="reader">The <see cref="T:TextReader"/> to read source code from.</param>
		public Tokenizer(TextReader reader)
		{
            this.reader = reader ?? throw new ArgumentNullException("reader");
			tokenBuilder = new StringBuilder();
			ReadChar();
		}

		#region helper methods

		void ReadChar()
		{
			previousPosition = currentPosition;

			int ch = reader.Read();
			if (ch >= char.MinValue && ch <= char.MaxValue)
			{
				currentChar = (char)ch;

				currentPosition.MoveToNextChar();
				if (currentChar == 10)
					currentPosition.MoveToNextLine();
				else if (currentChar == 13 && reader.Peek() == 10)
					ReadChar();
			}
			else atEndOfSource = true;
		}

		void SkipWhiteSpace()
		{
			while (!atEndOfSource && char.IsWhiteSpace(currentChar))
				ReadChar();
		}

		static void ThrowException(string message, TextPosition position) { throw new TokenizerException(message, position); }
		void CheckForUnexpectedEndOfSource() { if (atEndOfSource) ThrowException("Unexpected end of source.", currentPosition); }
		void CheckForExpectedChar(char expectedChar)
		{
			CheckForUnexpectedEndOfSource();
			if (currentChar != expectedChar)
				ThrowException("Unexpected character (expected '" + expectedChar + "').", currentPosition);
		}

		#endregion

		/// <summary>Reads all the tokens from the source code.</summary>
		/// <param name="ignoreComments">Indicates whether to ignore comments.</param>
		/// <returns>A collection of all the tokens.</returns>
		public IEnumerable<Token> ReadAllTokens(bool ignoreComments)
		{
			Token token;
			while ((token = ReadToken(ignoreComments)) != null)
				yield return token;
		}

		/// <summary>Reads the next <see cref="T:Token"/>.</summary>
		/// <param name="ignoreComments">Indicates whether to ignore comments.</param>
		/// <returns>The next <see cref="T:Token"/>, or null of the end of the source code has been reached.</returns>
		public Token ReadToken(bool ignoreComments)
		{
            while (true)
            {
                SkipWhiteSpace();
                if (atEndOfSource) return null;

                startPosition = currentPosition;
                tokenBuilder.Length = 0;

                Token lToken;
                if (char.IsLetter(currentChar))
                    // if the token starts with a letter, it must be an identifier or a predefined word
                    lToken = ReadIdentifierOrPredefinedWord();
                else if (char.IsDigit(currentChar))
                    // if the token starts with a digit, it must be an integer or real literal
                    lToken = ReadNumericLiteral();
                else if (currentChar == '"')
                    // if the token starts with a quote, it must be a string literal
                    lToken = ReadStringLiteral();
                else if (currentChar == '{')
                    // if the token starts with a curly brace, it must be a multi-line comment
                    lToken = ReadMultiLineComment();
                else // if the token starts with any other character, it must be a predefined symbol, a single-line comment, or an invalid token
                    lToken = ReadPredefinedSymbolOrSingleLineComment();

                if (lToken == null || !(ignoreComments && lToken is CommentToken))
                    return lToken;
            }
        }

		Token ReadIdentifierOrPredefinedWord()
		{
			// a letter followed by zero or more letters, digits or underscores
			do
			{
				tokenBuilder.Append(currentChar);
				ReadChar();
			} while (!atEndOfSource && (char.IsLetterOrDigit(currentChar) || currentChar == '_'));

			// check if it's one of the predefined keywords, data types or literals
			string word = tokenBuilder.ToString();
			switch (PredefinedWords.GetTokenType(word))
			{
				case TokenType.Keyword: return new PredefinedKeywordToken(PredefinedWords.GetPredefinedKeyword(word), startPosition, previousPosition);
				case TokenType.DataType: return new PredefinedDataTypeToken(PredefinedWords.GetPredefinedDataType(word), startPosition, previousPosition);
				case TokenType.Literal: return new PredefinedLiteralToken(PredefinedWords.GetPredefinedLiteral(word), startPosition, previousPosition);
				default: // not one of the predefined words => it's an identifier
					return new IdentifierToken(word, startPosition, previousPosition);
			}
		}

		Token ReadNumericLiteral()
		{
			// one or more digits, optionally followed by a decimal point and one or more digits
			bool hasDecimalPoint = false;
			do
			{
				tokenBuilder.Append(currentChar);
				ReadChar();

				if (!(atEndOfSource || hasDecimalPoint) && currentChar == '.')
				{
					hasDecimalPoint = true;
					tokenBuilder.Append('.');
					ReadChar(); // skip the decimal point
					CheckForUnexpectedEndOfSource();
					if (!char.IsDigit(currentChar))
						ThrowException("Unexpected character after decimal point (expected a digit).", currentPosition);
				}
			}
			while (!atEndOfSource && char.IsDigit(currentChar));

			if (hasDecimalPoint)
			{
                // it has a decimal point, so it's a real literal
                if (decimal.TryParse(tokenBuilder.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal realValue))
                    return new RealLiteralToken(realValue, startPosition, previousPosition);
                else ThrowException("Invalid real literal.", startPosition);
            }
			else
			{
                // it does not have a decimal point, so it's an integer literal
                if (long.TryParse(tokenBuilder.ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out long integerValue))
                    return new IntegerLiteralToken(integerValue, startPosition, previousPosition);
                else ThrowException("Invalid integer literal.", startPosition);
            }

			return null;
		}

		Token ReadStringLiteral()
		{
			// a sequence of zero or more characters enclosed in quotes
			ReadChar(); // skip the opening quote
			CheckForUnexpectedEndOfSource();
			while (currentChar != '"')
			{
				tokenBuilder.Append(currentChar);
				ReadChar();
				CheckForUnexpectedEndOfSource();
			}
			ReadChar(); // skip the closing quote
			return new StringLiteralToken(tokenBuilder.ToString(), startPosition, previousPosition);
		}

		Token ReadMultiLineComment()
		{
			// a sequence of zero or more characters enclosed in curly braces
			ReadChar(); // skip the opening curly brace
			CheckForUnexpectedEndOfSource();
			while (currentChar != '}')
			{
				tokenBuilder.Append(currentChar);
				ReadChar();
				CheckForUnexpectedEndOfSource();
			}
			ReadChar(); // skip the closing curly brace
			return new CommentToken(tokenBuilder.ToString(), startPosition, previousPosition);
		}

		Token ReadPredefinedSymbolOrSingleLineComment()
		{
			PredefinedSymbol symbol = PredefinedSymbol.None;
			switch (currentChar)
			{
				case '+': symbol = PredefinedSymbol.Plus; break;
				case '*': symbol = PredefinedSymbol.Asterisk; break;
				case '/': symbol = PredefinedSymbol.Slash; break;
				case '^': symbol = PredefinedSymbol.Caret; break;
				case '&': symbol = PredefinedSymbol.Ampersand; break;
				case '(': symbol = PredefinedSymbol.OpenParenthesis; break;
				case ')': symbol = PredefinedSymbol.ClosedParenthesis; break;
				case ',': symbol = PredefinedSymbol.Comma; break;
				case ':':
					if (reader.Peek() == '=') { ReadChar(); symbol = PredefinedSymbol.Assignment; }
					else symbol = PredefinedSymbol.Colon;
					break;
				case '=': ReadChar(); CheckForExpectedChar('='); symbol = PredefinedSymbol.Equal; break;
				case '<':
					switch (reader.Peek())
					{
						case '>': ReadChar(); symbol = PredefinedSymbol.NotEqual; break;
						case '=': ReadChar(); symbol = PredefinedSymbol.LessThanOrEqual; break;
						default: symbol = PredefinedSymbol.LessThan; break;
					}
					break;
				case '>':
					if (reader.Peek() == '=') { ReadChar(); symbol = PredefinedSymbol.GreaterThanOrEqual; }
					else symbol = PredefinedSymbol.GreaterThan;
					break;
				case '-':
					if (reader.Peek() == '-') return ReadSingleLineComment();
					else symbol = PredefinedSymbol.Minus;
					break;
			}

			if (symbol != PredefinedSymbol.None)
			{
				ReadChar();
				return new PredefinedSymbolToken(symbol, startPosition, previousPosition);
			}
			else ThrowException("Invalid character.", currentPosition);
			return null;
		}

		Token ReadSingleLineComment()
		{
			ReadChar(); ReadChar(); // skip the two dashes
			while (!atEndOfSource && currentChar != 10 && currentChar != 13)
			{
				tokenBuilder.Append(currentChar);
				ReadChar();
			}
			return new CommentToken(tokenBuilder.ToString(), startPosition, previousPosition);
		}
	}
}
