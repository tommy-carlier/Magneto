using System;
using System.Collections.Generic;
using System.Text;
using TC.Magneto.Tokenizing;
using TC.Magneto.Modules;
using System.IO;

namespace TC.Magneto.Parsing
{
	/// <summary>Converts a sequence of tokens into an abstract syntax tree.</summary>
	public class Parser
	{
		readonly ParserContext context;
		internal ParserContext Context { get { return context; } }

		readonly Queue<Token> tokens;
		Token currentToken;
		int loopDepth;

		/// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
		/// <param name="context">The <see cref="T:ParserContext"/> to contain the state.</param>
		/// <param name="reader">The <see cref="T:TextReader"/> to read source code from.</param>
		public Parser(ParserContext context, TextReader reader)
		{
			if (context == null) throw new ArgumentNullException("context");
			if (reader == null) throw new ArgumentNullException("reader");

			this.context = context;
			tokens = new Queue<Token>(new Tokenizer(reader).ReadAllTokens(true));
			ReadToken();
		}

		/// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
		/// <param name="context">The <see cref="T:ParserContext"/> to contain the state.</param>
		/// <param name="tokenizer">The <see cref="T:Tokenizer"/> to read tokens from.</param>
		public Parser(ParserContext context, Tokenizer tokenizer)
		{
			if (context == null) throw new ArgumentNullException("context");
			if (tokenizer == null) throw new ArgumentNullException("tokenizer");

			this.context = context;
			tokens = new Queue<Token>(tokenizer.ReadAllTokens(true));
			ReadToken();
		}

		/// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
		/// <param name="context">The <see cref="T:ParserContext"/> to contain the state.</param>
		/// <param name="tokens">The collection of tokens to read from.</param>
		public Parser(ParserContext context, IEnumerable<Token> tokens)
		{
			if (context == null) throw new ArgumentNullException("context");
			if (tokens == null) throw new ArgumentNullException("tokens");

			this.context = context;
			this.tokens = new Queue<Token>(tokens);
			ReadToken();
		}

		#region helper methods

		void ReadToken() { currentToken = (tokens.Count > 0 ? tokens.Dequeue() : null); }
		bool AtEndOfSource { get { return currentToken == null; } }
		static void ThrowException(string message, TextPosition position) { throw new ParserException(message, position); }
		static void ThrowUnexpectedEndOfSource() { ThrowException("Unexpected end of source.", TextPosition.Empty); }
		void CheckForUnexpectedEndOfSource() { if (AtEndOfSource) ThrowUnexpectedEndOfSource(); }

		TextPosition CurrentTextPosition { get { return currentToken != null ? currentToken.StartPosition : TextPosition.Empty; } }

		void SkipExpected(PredefinedKeyword keyword)
		{
			CheckForUnexpectedEndOfSource();
			if (currentToken.Equals(keyword)) ReadToken();
			else ThrowException("Expected keyword '" + keyword.ToString() + "' here.", currentToken.StartPosition);
		}

		void SkipExpected(PredefinedSymbol symbol, string text)
		{
			CheckForUnexpectedEndOfSource();
			if (currentToken.Equals(symbol)) ReadToken();
			else ThrowException("Expected " + text + " here.", currentToken.StartPosition);
		}

		DataType ReadDataType()
		{
			CheckForUnexpectedEndOfSource();
			if (currentToken is PredefinedDataTypeToken token)
			{
				ReadToken();
				return token.DataType;
			}
			else
			{
				ThrowException("Expected a data type here.", currentToken.StartPosition);
				return DataType.None;
			}
		}

		Variable GetVariable(string identifier)
		{
			Variable variable = context.GetVariable(identifier);
			if (variable == null) ThrowException("Undeclared variable '" + identifier + "'.", currentToken.StartPosition);
			return variable;
		}

		void BeginLoop() { loopDepth += 1; }

		void EndLoop()
		{
			if (loopDepth == 0) throw new InvalidOperationException();
			loopDepth -= 1;
		}

		bool IsInsideLoop { get { return loopDepth > 0; } }

		#endregion

		/// <summary>Reads the top-level statements.</summary>
		/// <returns>The collection of top-level statements.</returns>
		public IEnumerable<Statement> ReadTopLevelStatements()
		{
			Statement statement;
			while ((statement = ReadTopLevelStatement()) != null)
				yield return statement;
		}

		/// <summary>Reads the top-level statements.</summary>
		/// <param name="visitor">The <see cref="T:IStatementVisitor"/> that will visit each top-level statement.</param>
		public void ReadTopLevelStatements(IStatementVisitor visitor)
		{
			if (visitor == null) throw new ArgumentNullException("visitor");

			Statement statement;
			while ((statement = ReadTopLevelStatement()) != null)
				statement.AcceptInternal(visitor);
		}

		/// <summary>Reads the next top-level <see cref="T:Statement"/>.</summary>
		/// <returns>The next top-level <see cref="T:Statement"/>, or null if the end of the source code has been reached.</returns>
		public Statement ReadTopLevelStatement() { return ParseStatement(); }

		Statement ParseStatement()
		{
			if (AtEndOfSource) return null;

			while (true)
			{
				switch (currentToken.TokenType)
				{
					case TokenType.Literal: ParseBooleanDeclaration(); continue;
					case TokenType.Keyword: return ParseKeywordStatement();
					case TokenType.Identifier: return ParseAssignmentOrFunctionCallStatement();
				}

				ThrowException("Expected a statement here.", currentToken.StartPosition);
				return null;
			}
		}

		void AddStatement(ICollection<Statement> collection)
		{
			Statement statement = ParseStatement();
			if (statement != null)
			{
				collection.Add(statement);
				CheckForUnexpectedEndOfSource();
			}
			else ThrowUnexpectedEndOfSource();
		}

		StatementCollection CreateStatementCollection(IEnumerable<Statement> statements)
		{
			return new StatementCollection(context, statements);
		}

		void ParseBooleanDeclaration()
		{
            //	boolean-declaration:
            //		[pos neg north south cw ccw open closed] := [true false]

            if (currentToken is PredefinedLiteralToken token)
                switch (token.Value)
                {
                    case PredefinedLiteral.Positive:
                    case PredefinedLiteral.Negative:
                    case PredefinedLiteral.North:
                    case PredefinedLiteral.South:
                    case PredefinedLiteral.Clockwise:
                    case PredefinedLiteral.Counterclockwise:
                    case PredefinedLiteral.Open:
                    case PredefinedLiteral.Closed:
                        ParseBooleanDeclaration(token.Value);
                        return;
                }

            ThrowException("Expected a statement here.", currentToken.StartPosition);
		}

		void ParseBooleanDeclaration(PredefinedLiteral literal)
		{
			//	boolean-declaration:
			//		[pos neg north south cw ccw open closed] := [true false]

			TextPosition position = currentToken.StartPosition;
			ReadToken(); // skip the literal
			CheckForUnexpectedEndOfSource();

			if (!currentToken.Equals(PredefinedSymbol.Assignment))
				ThrowException("Expected a boolean declaration here.", currentToken.StartPosition);

			ReadToken(); // skip ':='
			CheckForUnexpectedEndOfSource();

			if (currentToken.Equals(PredefinedLiteral.True))
			{
				context.DeclareBoolean(literal, true, position);
				ReadToken(); // skip 'true'
			}
			else if (currentToken.Equals(PredefinedLiteral.False))
			{
				context.DeclareBoolean(literal, false, position);
				ReadToken(); // skip 'false'
			}
			else ThrowException("Expected true or false here.", currentToken.StartPosition);
		}

		Statement ParseKeywordStatement()
		{
			switch ((currentToken as PredefinedKeywordToken).Keyword)
			{
				case PredefinedKeyword.If: return ParseIfStatement();
				case PredefinedKeyword.While: return ParseWhileLoopStatement();
				case PredefinedKeyword.Repeat: return ParseRepeatLoopStatement();
				case PredefinedKeyword.For: return ParseForLoopStatement();
				case PredefinedKeyword.Break: return ParseBreakStatement();
				case PredefinedKeyword.Exit: return ParseExitStatement();
				case PredefinedKeyword.Switch: return ParseSwitchStatement();
				case PredefinedKeyword.Var: return ParseVariableDeclaration();
			}

			ThrowException("Expected a statement here.", currentToken.StartPosition);
			return null;
		}

		Statement ParseIfStatement()
		{
			//	if-statement:
			//		if {condition} then {statements} end if
			//		if {condition} then {statements} else {statements} end if
			//		if {condition} then {statements} elsif {condition} then {statements} end if
			//		if {condition} then {statements} elsif {condition} then {statements} ... else {statements} end if

			ReadToken(); // skip 'if' or 'elsif'
			Expression condition = ParseCondition();
			SkipExpected(PredefinedKeyword.Then); // skip 'then'

			List<Statement> trueStatements = new List<Statement>();
			List<Statement> falseStatements = new List<Statement>();
			List<Statement> statements = trueStatements;

			CheckForUnexpectedEndOfSource();
			context.BeginScope();
			while (!currentToken.Equals(PredefinedKeyword.End))
			{
				if (currentToken.Equals(PredefinedKeyword.Else))
				{
					if (statements != falseStatements)
					{
						ReadToken(); // skip 'else'
						CheckForUnexpectedEndOfSource();
						statements = falseStatements;
						context.EndScope();
						context.BeginScope();
						continue;
					}
					else ThrowException("Double 'else' keyword.", currentToken.StartPosition);
				}
				else if (currentToken.Equals(PredefinedKeyword.Elsif))
				{
					if (statements == trueStatements)
					{
						falseStatements.Add(ParseIfStatement());
						return new IfStatement(context, condition
							, CreateStatementCollection(trueStatements)
							, CreateStatementCollection(falseStatements));
					}
					else ThrowException("'elsif' cannot occur after 'else'.", currentToken.StartPosition);
				}

				AddStatement(statements);
			}

			ReadToken(); // skip 'end'
			SkipExpected(PredefinedKeyword.If); // skip 'if' after 'end'
			context.EndScope();

			return new IfStatement(context, condition
				, CreateStatementCollection(trueStatements)
				, CreateStatementCollection(falseStatements));
		}

		Statement ParseWhileLoopStatement()
		{
			//	while-loop-statement:
			//		while {condition} do {statements} end while

			ReadToken(); // skip 'while'
			Expression condition = ParseCondition();
			SkipExpected(PredefinedKeyword.Do); // skip 'do'

			CheckForUnexpectedEndOfSource();
			context.BeginScope();
			BeginLoop();

			List<Statement> statements = new List<Statement>();
			while (!currentToken.Equals(PredefinedKeyword.End))
				AddStatement(statements);

			EndLoop();
			context.EndScope();

			ReadToken(); // skip 'end'
			SkipExpected(PredefinedKeyword.While); // skip 'while' after 'end'

			return new WhileLoopStatement(context, condition, CreateStatementCollection(statements));
		}

		Statement ParseRepeatLoopStatement()
		{
			//	repeat-loop-statement:
			//		repeat {statements} until {condition}

			ReadToken(); // skip 'repeat'

			CheckForUnexpectedEndOfSource();
			context.BeginScope();
			BeginLoop();

			List<Statement> statements = new List<Statement>();
			while (!currentToken.Equals(PredefinedKeyword.Until))
				AddStatement(statements);

			EndLoop();
			context.EndScope();

			ReadToken(); // skip 'until'
			return new RepeatLoopStatement(context, ParseCondition(), CreateStatementCollection(statements));
		}

		Statement ParseForLoopStatement()
		{
			//	for-loop-statement:
			//		for {variable} := {start-expression} to {end-expression} do {statements} end for
			//		for {variable} := {start-expression} to {end-expression} by {step-expression} do {statements} end for

			ReadToken(); // skip 'for'
			CheckForUnexpectedEndOfSource();
			context.BeginScope();

			TextPosition variableNamePosition = currentToken.StartPosition;
			if (currentToken.TokenType != TokenType.Identifier)
				ThrowException("Expected a variable name here.", variableNamePosition);
			string variableName = (currentToken as IdentifierToken).Identifier;
			ReadToken();

			SkipExpected(PredefinedSymbol.Assignment, "assignment operator :="); // skip ':='
			Expression startExpression = ParseNumericExpression();

			SkipExpected(PredefinedKeyword.To); // skip 'to'
			Expression endExpression = ParseNumericExpression();
			CheckForUnexpectedEndOfSource();

			Expression stepExpression;
			if (currentToken.Equals(PredefinedKeyword.By))
			{
				ReadToken(); // skip 'by'
				stepExpression = ParseNumericExpression();
			}
			else stepExpression = new Literal<long>(context, 1L);

			MakeNumericExpressionsSameType(ref startExpression, ref endExpression, ref stepExpression);
			Variable variable = context.DeclareVariable(variableName, stepExpression.ResultType, variableNamePosition);

			SkipExpected(PredefinedKeyword.Do); // skip 'do'
			CheckForUnexpectedEndOfSource();
			BeginLoop();

			List<Statement> statements = new List<Statement>();
			while (!currentToken.Equals(PredefinedKeyword.End))
				AddStatement(statements);

			EndLoop();

			ReadToken(); // skip 'end'
			SkipExpected(PredefinedKeyword.For); // skip 'for' after 'end'
			context.EndScope();

			return new ForLoopStatement(context, variable, startExpression, endExpression, stepExpression, CreateStatementCollection(statements));
		}

		Statement ParseBreakStatement()
		{
			//	break-statement:
			//		break

			if (!IsInsideLoop) ThrowException("'break' can only be used inside a loop.", currentToken.StartPosition);
			ReadToken(); // skip 'break'
			return new BreakStatement(context);
		}

		Statement ParseExitStatement()
		{
			//	exit-statement:
			//		exit

			ReadToken(); // skip 'exit'
			return new ExitStatement(context);
		}

		Statement ParseSwitchStatement()
		{
			//	switch-statement:
			//		switch {expression}
			//			case {expression} then {statements}
			//			case {expression} then {statements}
			//			...
			//			case else {statements}
			//		end switch

			ReadToken(); // skip 'switch'
			CheckForUnexpectedEndOfSource();

			Expression valueExpression = ParseExpression();
			CheckForUnexpectedEndOfSource();
			DataType valueType = valueExpression.ResultType;

			List<CaseStatement> cases = new List<CaseStatement>();
			StatementCollection elseStatements = null;
			while (currentToken.Equals(PredefinedKeyword.Case))
			{
				TextPosition caseStartPosition = currentToken.StartPosition;

				if (elseStatements != null)
					ThrowException("There can be no more 'case'-statements after the 'case else'-statement.", caseStartPosition);

				ReadToken(); // skip 'case'
				CheckForUnexpectedEndOfSource();

				if (currentToken.Equals(PredefinedKeyword.Else)) // 'case else'
				{
					ReadToken(); // skip 'else'
					elseStatements = ParseCaseBodyStatements();
				}
				else
				{
					Expression caseValue = ParseExpression();
					SkipExpected(PredefinedKeyword.Then); // skip 'then'
					DataType caseValueType = caseValue.ResultType;
					if (caseValueType != valueType)
					{
						if ((caseValueType == DataType.Integer || caseValueType == DataType.Real)
							&& (valueType == DataType.Integer || valueType == DataType.Real))
							MakeNumericExpressionsSameType(ref valueExpression, ref caseValue);
						else ThrowException("The type of the 'case'-expression does not match the type of the 'switch'-statement.", caseStartPosition);
					}

					cases.Add(new CaseStatement(context, caseValue, ParseCaseBodyStatements()));
				}
			}

			SkipExpected(PredefinedKeyword.End); // skip 'end'
			SkipExpected(PredefinedKeyword.Switch); // skip 'switch' after 'end'

			return new SwitchStatement(context, valueExpression, cases, elseStatements);
		}

		StatementCollection ParseCaseBodyStatements()
		{
			CheckForUnexpectedEndOfSource();
			context.BeginScope();

			List<Statement> statements = new List<Statement>();
			while (!(currentToken.Equals(PredefinedKeyword.Case) || currentToken.Equals(PredefinedKeyword.End)))
				AddStatement(statements);

			context.EndScope();
			return CreateStatementCollection(statements);
		}

		Statement ParseVariableDeclaration()
		{
			//	variable-declaration:
			//		var {single-declaration}
			//		var {single-declaration}, {single-declaration}
			//		var {single-declaration}, {single-declaration}, ...

			TextPosition position = currentToken.StartPosition;
			ReadToken(); // skip 'var'
			CheckForUnexpectedEndOfSource();

			List<Statement> singleDeclarations = new List<Statement>();
			singleDeclarations.Add(ParseSingleDeclaration(position));
			while (!AtEndOfSource && currentToken.Equals(PredefinedSymbol.Comma))
			{
				ReadToken(); // skip ','
				singleDeclarations.Add(ParseSingleDeclaration(position));
			}

			if (singleDeclarations.Count == 1) return singleDeclarations[0];
			else return CreateStatementCollection(singleDeclarations);
		}

		Statement ParseSingleDeclaration(TextPosition startPosition)
		{
			//	single-declaration:
			//		{variable} : {type}
			//		{variable}, {variable} : {type}
			//		{variable}, {variable}, ... : {type}
			//		{variable} := {expression}
			//		{variable}, {variable} := {expression}
			//		{variable}, {variable}, ... := {expression}

			List<string> variableNames = new List<string>();
			while (true)
			{
				if (currentToken.TokenType != TokenType.Identifier)
					ThrowException("Expected a variable name here.", currentToken.StartPosition);

				variableNames.Add((currentToken as IdentifierToken).Identifier);
				ReadToken(); // skip variable name
				CheckForUnexpectedEndOfSource();

				if (currentToken.Equals(PredefinedSymbol.Comma))
				{
					ReadToken(); // skip ','
					CheckForUnexpectedEndOfSource();
				}
				else
				{
					CheckForUnexpectedEndOfSource();
					break;
				}
			}

			PredefinedSymbolToken symbolToken = currentToken as PredefinedSymbolToken;
			if (symbolToken == null)
				ThrowException("Expected a type declaration or value assignment here.", currentToken.StartPosition);

			Expression value = null;
			DataType valueType = DataType.None;
			switch (symbolToken.Symbol)
			{
				case PredefinedSymbol.Assignment:
					ReadToken(); // skip ':='
					value = ParseExpression();
					valueType = value.ResultType;
					break;
				case PredefinedSymbol.Colon:
					ReadToken(); // skip ':'
					valueType = ReadDataType();
					switch (valueType)
					{
						case DataType.String: value = new Literal<string>(context, ""); break;
						case DataType.Integer: value = new Literal<long>(context, 0L); break;
						case DataType.Real: value = new Literal<decimal>(context, decimal.Zero); break;
						case DataType.Logic: value = new Literal<bool>(context, false); break;
						case DataType.Polarity: value = new Literal<Polarity>(context, Polarity.Positive); break;
						case DataType.Magnetic: value = new Literal<Magnetic>(context, Magnetic.North); break;
						case DataType.Curl: value = new Literal<Curl>(context, Curl.Clockwise); break;
						case DataType.Circuit: value = new Literal<Circuit>(context, Circuit.Open); break;
						default: ThrowException("Expected one of the predefined types here.", currentToken.StartPosition); break;
					}
					break;
				default:
					ThrowException("Expected a type declaration or value assignment here.", currentToken.StartPosition);
					break;
			}

			List<Statement> assignments = new List<Statement>(variableNames.Count * 2);
			foreach (string variableName in variableNames)
			{
				Variable variable = context.DeclareVariable(variableName, valueType, startPosition);
				assignments.Add(new VariableDeclaration(context, variable));
				assignments.Add(new Assignment(context, variable, value));
			}
			return CreateStatementCollection(assignments);
		}

		Statement ParseAssignmentOrFunctionCallStatement()
		{
			TextPosition position = currentToken.StartPosition;
			string identifier = (currentToken as IdentifierToken).Identifier;
			ReadToken(); // skip the identifier
			CheckForUnexpectedEndOfSource();

            if (currentToken is PredefinedSymbolToken symbolToken)
                switch (symbolToken.Symbol)
                {
                    case PredefinedSymbol.Assignment: return ParseAssignment(identifier, position);
                    case PredefinedSymbol.OpenParenthesis: return ParseFunctionCallStatement(identifier, position);
                }

            ThrowException("Expected an assignment or function call here.", currentToken.StartPosition);
			return null;
		}

		Statement ParseAssignment(string identifier, TextPosition position)
		{
			//	assignment:
			//		{variable} := {expression}

			Variable variable = GetVariable(identifier);

			ReadToken(); // skip ':='
			Expression value = ParseExpression();
			if (value.ResultType != variable.DataType)
				ThrowException("The data type of the expression does not match the data type of the variable '" + identifier + "'.", position);

			return new Assignment(context, variable, value);
		}

		Statement ParseFunctionCallStatement(string identifier, TextPosition position)
		{
			//	function-call-statement:
			//		{function-call}

			return new FunctionCallStatement(context, ParseFunctionCall(identifier, position));
		}

		Expression ParseCondition()
		{
			//	condition:
			//		{expression} (result-type == logic)

			TextPosition position = CurrentTextPosition;
			Expression condition = ParseExpression();
			if (condition.ResultType != DataType.Logic)
				ThrowException("Expected a condition here (of type 'logic').", position);
			return condition;
		}

		Expression ParseNumericExpression()
		{
			//	numeric-expression:
			//		{expression} (result-type == integer or real)

			TextPosition position = CurrentTextPosition;
			Expression expression = ParseExpression();
			CheckNumericExpression(expression, position);
			return expression;
		}

		void CheckBooleanExpression(Expression expression, TextPosition startPosition)
		{
			if (!context.IsBoolean(expression.ResultType))
				ThrowException("Expected a boolean expression here.", startPosition);
		}

		static void CheckNumericExpression(Expression expression, TextPosition startPosition)
		{
			DataType resultType = expression.ResultType;
			if (resultType != DataType.Integer && resultType != DataType.Real)
				ThrowException("Expected a numeric expression here (of type 'integer' or 'real').", startPosition);
		}

		static void CheckNumericOrStringExpression(Expression expression, TextPosition startPosition)
		{
			DataType resultType = expression.ResultType;
			if (resultType != DataType.Integer && resultType != DataType.Real && resultType != DataType.String)
				ThrowException("Expected a numeric or string expression here (of type 'integer', 'real' or 'string').", startPosition);
		}

		static void CheckSameDataType(Expression expression1, Expression expression2, TextPosition startPosition)
		{
			if (expression1.ResultType != expression2.ResultType)
				ThrowException("Expected 2 expressions of the same type here.", startPosition);
		}

		void MakeNumericExpressionsSameType(ref Expression expression1, ref Expression expression2)
		{
			if (expression1.ResultType != expression2.ResultType)
			{
				IList<Expression> arguments;
				if (expression1.ResultType == DataType.Integer)
				{
					arguments = new List<Expression>(1) { expression1 };
					expression1 = new FunctionCall(context, context.ModuleManager.GetFunctionSignature("toReal", arguments), arguments);
				}
				else
				{
					arguments = new List<Expression>(1) { expression2 };
					expression2 = new FunctionCall(context, context.ModuleManager.GetFunctionSignature("toReal", arguments), arguments);
				}
			}
		}

		void MakeNumericExpressionsSameType(ref Expression expression1, ref Expression expression2, ref Expression expression3)
		{
			MakeNumericExpressionsSameType(ref expression1, ref expression2);
			MakeNumericExpressionsSameType(ref expression1, ref expression3);
			MakeNumericExpressionsSameType(ref expression2, ref expression3);
		}

		Expression GetTrueValue(DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Logic: return new Literal<bool>(context, true);
				case DataType.Polarity: return new Literal<Polarity>(context, context.PolarityTrueValue);
				case DataType.Magnetic: return new Literal<Magnetic>(context, context.MagneticTrueValue);
				case DataType.Curl: return new Literal<Curl>(context, context.CurlTrueValue);
				case DataType.Circuit: return new Literal<Circuit>(context, context.CircuitTrueValue);
				default: return null;
			}
		}

		Expression GetFalseValue(DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Logic: return new Literal<bool>(context, false);
				case DataType.Polarity: return new Literal<Polarity>(context, context.PolarityFalseValue);
				case DataType.Magnetic: return new Literal<Magnetic>(context, context.MagneticFalseValue);
				case DataType.Curl: return new Literal<Curl>(context, context.CurlFalseValue);
				case DataType.Circuit: return new Literal<Circuit>(context, context.CircuitFalseValue);
				default: return null;
			}
		}

		Expression ParseExpression()
		{
			CheckForUnexpectedEndOfSource();
			return ParseAndExpression();
		}

		Expression ParseAndExpression()
		{
			//	and-expression:
			//		{or-expression}
			//		{or-expression} and {or-expression}
			//		{or-expression} and {or-expression} and ...

			TextPosition position = CurrentTextPosition;
			Expression leftNode = ParseOrExpression();

			bool checkLeftNode = true;
			while (!AtEndOfSource && currentToken.Equals(PredefinedKeyword.And))
			{
				if (checkLeftNode)
				{
					CheckBooleanExpression(leftNode, position);
					checkLeftNode = false;
				}

				ReadToken(); // skip 'and'
				position = CurrentTextPosition;
				Expression rightNode = ParseOrExpression();
				CheckBooleanExpression(rightNode, position);
				CheckSameDataType(leftNode, rightNode, position);
				leftNode = new AndExpression(context, leftNode, rightNode, GetTrueValue(leftNode.ResultType), GetFalseValue(leftNode.ResultType));
			}

			return leftNode;
		}

		Expression ParseOrExpression()
		{
			//	or-expression:
			//		{xor-expression}
			//		{xor-expression} or {xor-expression}
			//		{xor-expression} or {xor-expression} or ...

			TextPosition position = CurrentTextPosition;
			Expression leftNode = ParseXorExpression();

			bool checkLeftNode = true;
			while (!AtEndOfSource && currentToken.Equals(PredefinedKeyword.Or))
			{
				if (checkLeftNode)
				{
					CheckBooleanExpression(leftNode, position);
					checkLeftNode = false;
				}

				ReadToken(); // skip 'or'
				position = CurrentTextPosition;
				Expression rightNode = ParseXorExpression();
				CheckBooleanExpression(rightNode, position);
				CheckSameDataType(leftNode, rightNode, position);
				leftNode = new OrExpression(context, leftNode, rightNode, GetTrueValue(leftNode.ResultType), GetFalseValue(leftNode.ResultType));
			}

			return leftNode;
		}

		Expression ParseXorExpression()
		{
			//	xor-expression:
			//		{comparison}
			//		{comparison} xor {comparison}
			//		{comparison} xor {comparison} xor ...

			TextPosition position = CurrentTextPosition;
			Expression leftNode = ParseComparison();

			bool checkLeftNode = true;
			while (!AtEndOfSource && currentToken.Equals(PredefinedKeyword.Xor))
			{
				if (checkLeftNode)
				{
					CheckBooleanExpression(leftNode, position);
					checkLeftNode = false;
				}

				ReadToken(); // skip 'xor'
				position = CurrentTextPosition;
				Expression rightNode = ParseComparison();
				CheckBooleanExpression(rightNode, position);
				CheckSameDataType(leftNode, rightNode, position);
				leftNode = new XorExpression(context, leftNode, rightNode, GetTrueValue(leftNode.ResultType), GetFalseValue(leftNode.ResultType));
			}

			return leftNode;
		}

		Expression ParseComparison()
		{
			//	comparison:
			//		{concatenation}
			//		{concatenation} [== <> < > <= >=] {concatenation}

			TextPosition position = currentToken.StartPosition;
			Expression leftNode = ParseConcatenation();

			if (!AtEndOfSource && currentToken.TokenType == TokenType.Symbol)
			{
				ComparisonOperator @operator = Comparison.GetOperator((currentToken as PredefinedSymbolToken).Symbol);
				if (@operator != ComparisonOperator.None)
				{
					if (@operator != ComparisonOperator.Equal && @operator != ComparisonOperator.NotEqual)
						CheckNumericOrStringExpression(leftNode, position);

					position = currentToken.StartPosition;
					ReadToken(); // skip the operator

					TextPosition rightNodeStartPosition = currentToken.StartPosition;
					Expression rightNode = ParseConcatenation();

					if (@operator != ComparisonOperator.Equal && @operator != ComparisonOperator.NotEqual)
						CheckNumericOrStringExpression(rightNode, rightNodeStartPosition);

					DataType leftType = leftNode.ResultType;
					DataType rightType = rightNode.ResultType;

					if (leftType != rightType)
					{
						if ((leftType == DataType.Integer && rightType == DataType.Real)
							|| (leftType == DataType.Real && rightType == DataType.Integer))
							MakeNumericExpressionsSameType(ref leftNode, ref rightNode);
						else ThrowException("The child expressions of a comparison have to have the same type.", position);
					}

					return new Comparison(context, @operator, leftNode, rightNode);
				}
			}

			return leftNode;
		}

		Expression ParseConcatenation()
		{
			//	concatenation:
			//		{additive-expression}
			//		{additive-expression} & {additive-expression}
			//		{additive-expression} & {additive-expression} & ...

			List<Expression> nodes = new List<Expression>();
			nodes.Add(ParseAdditiveExpression());

			while (!AtEndOfSource && currentToken.Equals(PredefinedSymbol.Ampersand))
			{
				ReadToken(); // skip '&'
				nodes.Add(ParseAdditiveExpression());
			}

			if (nodes.Count > 1)
				return new Concatenation(context, nodes.ToArray());
			else return nodes[0];
		}

		Expression ParseAdditiveExpression()
		{
			//	additive-expression:
			//		{multiplicative-expression}
			//		{multiplicative-expression} [+ -] {multiplicative-expression}
			//		{multiplicative-expression} [+ -] {multiplicative-expression} [+ -] ...

			TextPosition position = CurrentTextPosition;
			Expression leftNode = ParseMultiplicativeExpression();
			Expression rightNode;

			while (!AtEndOfSource)
			{
				if (currentToken.Equals(PredefinedSymbol.Plus))
				{
					CheckNumericExpression(leftNode, position);
					ReadToken(); // skip '+'
					position = CurrentTextPosition;
					CheckNumericExpression(rightNode = ParseMultiplicativeExpression(), position);
					MakeNumericExpressionsSameType(ref leftNode, ref rightNode);
					leftNode = new Addition(context, leftNode, rightNode);
				}
				else if (currentToken.Equals(PredefinedSymbol.Minus))
				{
					CheckNumericExpression(leftNode, position);
					ReadToken(); // skip '-'
					position = CurrentTextPosition;
					CheckNumericExpression(rightNode = ParseMultiplicativeExpression(), position);
					MakeNumericExpressionsSameType(ref leftNode, ref rightNode);
					leftNode = new Subtraction(context, leftNode, rightNode);
				}
				else break;
			}

			return leftNode;
		}

		Expression ParseMultiplicativeExpression()
		{
			//	multiplicative-expression:
			//		{power-expression}
			//		{power-expression} [* / mod] {power-expression}
			//		{power-expression} [* / mod] {power-expression} [* / mod] ...

			TextPosition position = CurrentTextPosition;
			Expression leftNode = ParsePowerExpression();
			Expression rightNode;

			while (!AtEndOfSource)
			{
				if (currentToken.Equals(PredefinedSymbol.Asterisk))
				{
					CheckNumericExpression(leftNode, position);
					ReadToken(); // skip '*'
					position = CurrentTextPosition;
					CheckNumericExpression(rightNode = ParsePowerExpression(), position);
					MakeNumericExpressionsSameType(ref leftNode, ref rightNode);
					leftNode = new Multiplication(context, leftNode, rightNode);
				}
				else if (currentToken.Equals(PredefinedSymbol.Slash))
				{
					CheckNumericExpression(leftNode, position);
					ReadToken(); // skip '/'
					position = CurrentTextPosition;
					CheckNumericExpression(rightNode = ParsePowerExpression(), position);
					MakeNumericExpressionsSameType(ref leftNode, ref rightNode);
					leftNode = new Division(context, leftNode, rightNode);
				}
				else if (currentToken.Equals(PredefinedKeyword.Mod))
				{
					CheckNumericExpression(leftNode, position);
					ReadToken(); // skip 'mod'
					position = CurrentTextPosition;
					CheckNumericExpression(rightNode = ParsePowerExpression(), position);
					MakeNumericExpressionsSameType(ref leftNode, ref rightNode);
					leftNode = new ModuloExpression(context, leftNode, rightNode);
				}
				else break;
			}

			return leftNode;
		}

		Expression ParsePowerExpression()
		{
			//	power-expression:
			//		{unary-expression}
			//		{unary-expression} ^ {unary-expression}
			//		{unary-expression} ^ {unary-expression} ^ ...

			TextPosition position = CurrentTextPosition;
			Expression leftNode = ParseUnaryExpression();
			Expression rightNode;
			Expression realNode = new Literal<decimal>(context, decimal.Zero);

			while (!AtEndOfSource && currentToken.Equals(PredefinedSymbol.Caret))
			{
				CheckNumericExpression(leftNode, position);
				MakeNumericExpressionsSameType(ref leftNode, ref realNode);
				ReadToken(); // skip '^'
				position = CurrentTextPosition;
				rightNode = ParseUnaryExpression();
				if (rightNode.ResultType != DataType.Integer)
					ThrowException("Expected an expression of type 'integer'.", position);
				leftNode = new PowerExpression(context, leftNode, rightNode);
			}

			return leftNode;
		}

		Expression ParseUnaryExpression()
		{
			//	unary-expression:
			//		{base-expression}
			//		- {base-expression}
			//		not {base-expression}

			Expression baseExpression;
			CheckForUnexpectedEndOfSource();
			if (currentToken.Equals(PredefinedSymbol.Minus))
			{
				ReadToken(); // skip '-'
				TextPosition position = CurrentTextPosition;

				// if the next token is an integer or real literal, return the negated literal directly
				if (currentToken.TokenType == TokenType.Literal)
				{
					LiteralToken literal = currentToken as LiteralToken;
					switch (literal.DataType)
					{
						case DataType.Integer:
							ReadToken();
							return new Literal<long>(context, -(literal as IntegerLiteralToken).Value);
						case DataType.Real:
							ReadToken();
							return new Literal<decimal>(context, -(literal as RealLiteralToken).Value);
					}
				}

				CheckNumericExpression(baseExpression = ParseBaseExpression(), position);
				return new Negation(context, baseExpression);
			}
			else if (currentToken.Equals(PredefinedKeyword.Not))
			{
				ReadToken(); // skip 'not'
				TextPosition position = CurrentTextPosition;
				CheckBooleanExpression(baseExpression = ParseBaseExpression(), position);
				return new NotExpression(context, baseExpression);
			}
			else if (currentToken.Equals(PredefinedSymbol.Plus))
			{
				ReadToken(); // skip '+'
				TextPosition position = CurrentTextPosition;
				CheckNumericExpression(baseExpression = ParseBaseExpression(), position);
				return baseExpression;
			}
			else return ParseBaseExpression();
		}

		Expression ParseBaseExpression()
		{
			//	base-expression:
			//		{grouping-expression}
			//		{variable}
			//		{constant}
			//		{function-call}
			//		{literal}

			CheckForUnexpectedEndOfSource();

			switch (currentToken.TokenType)
			{
				case TokenType.Symbol:
					if (currentToken.Equals(PredefinedSymbol.OpenParenthesis))
						return ParseGroupingExpression();
					break;
				case TokenType.Identifier: return ParseVariableExpressionOrFunctionCall();
				case TokenType.Literal: return ParseLiteral();
			}

			ThrowException("Expected an expression.", currentToken.StartPosition);
			return null;
		}

		Expression ParseGroupingExpression()
		{
			//	grouping-expression:
			//		( {expression} )

			ReadToken(); // skip '('
			Expression expression = ParseExpression();
			SkipExpected(PredefinedSymbol.ClosedParenthesis, "closing parenthesis )"); // skip ')'
			return expression;
		}

		Expression ParseVariableExpressionOrFunctionCall()
		{
			TextPosition position = currentToken.StartPosition;
			string identifier = (currentToken as IdentifierToken).Identifier;

			ReadToken(); // skip the identifier
			if (!AtEndOfSource && currentToken.Equals(PredefinedSymbol.OpenParenthesis))
				return ParseFunctionCall(identifier, position);
			else
			{
				ConstantAttribute constant = context.ModuleManager.GetConstant(identifier);
				return constant != null
					? constant.CreateExpression(context)
					: ParseVariableExpression(identifier);
			}
		}

		FunctionCall ParseFunctionCall(string identifier, TextPosition position)
		{
			//	function-call:
			//		{function} ( )
			//		{function} ( {argument} )
			//		{function} ( {argument}, {argument} )
			//		{function} ( {argument}, {argument}, ... )

			ReadToken(); // skip '('
			CheckForUnexpectedEndOfSource();

			List<Expression> arguments = new List<Expression>();
			if (!currentToken.Equals(PredefinedSymbol.ClosedParenthesis))
			{
				arguments.Add(ParseFunctionArgument());
				CheckForUnexpectedEndOfSource();

				while (currentToken.Equals(PredefinedSymbol.Comma))
				{
					ReadToken(); // skip ','
					arguments.Add(ParseFunctionArgument());
					CheckForUnexpectedEndOfSource();
				}

				if (!currentToken.Equals(PredefinedSymbol.ClosedParenthesis))
					ThrowException("Expected closing parenthesis ) here.", currentToken.StartPosition);
			}

			ReadToken(); // skip ')'

			IList<Expression> convertToRealArguments;
			FunctionSignature functionSignature = GetFunctionSignature(identifier, arguments, position);
			for (int i = arguments.Count - 1; i >= 0; i--)
			{
				Expression argument = arguments[i];
				if (argument.ResultType == DataType.Integer
					&& functionSignature.Arguments[i].DataType == DataType.Real)
				{
					convertToRealArguments = new List<Expression>(1) { argument };
					arguments[i] = new FunctionCall(context, context.ModuleManager.GetFunctionSignature("toReal", convertToRealArguments), convertToRealArguments);
				}
			}

			return new FunctionCall(context, functionSignature, arguments);
		}

		FunctionSignature GetFunctionSignature(string functionName, IList<Expression> arguments, TextPosition position)
		{
			try { return context.ModuleManager.GetFunctionSignature(functionName, arguments); }
			catch (MagnetoException exception) { ThrowException(exception.Message, position); return null; }
		}

		Expression ParseFunctionArgument()
		{
			//	function-argument:
			//		{variable-reference}
			//		{expression}

			CheckForUnexpectedEndOfSource();
			if (currentToken.Equals(PredefinedKeyword.Ref))
				return ParseVariableReference();
			else return ParseExpression();
		}

		Expression ParseVariableReference()
		{
			//	variable-reference:
			//		ref {variable}

			ReadToken(); // skip 'ref'
			CheckForUnexpectedEndOfSource();
			if (currentToken.TokenType == TokenType.Identifier)
			{
				Variable variable = GetVariable((currentToken as IdentifierToken).Identifier);
				ReadToken();
				return new VariableReference(context, variable);
			}

			ThrowException("Expected a variable name here.", currentToken.StartPosition);
			return null;
		}

		Expression ParseVariableExpression(string identifier)
		{
			//	variable-expression:
			//		{variable}

			return new VariableExpression(context, GetVariable(identifier));
		}

		Expression ParseLiteral()
		{
			bool succeeded = true;
			try
			{
				switch ((currentToken as LiteralToken).DataType)
				{
					case DataType.String: return new Literal<string>(context, (currentToken as StringLiteralToken).Value);
					case DataType.Integer: return new Literal<long>(context, (currentToken as IntegerLiteralToken).Value);
					case DataType.Real: return new Literal<decimal>(context, (currentToken as RealLiteralToken).Value);
					case DataType.Logic:
					case DataType.Polarity:
					case DataType.Magnetic:
					case DataType.Curl:
					case DataType.Circuit: return ParsePredefinedLiteral();
				}
			}
			catch { succeeded = false; throw; }
			finally
			{
				if (succeeded) ReadToken(); // skip literal
			}

			ThrowException("Expected a literal.", currentToken.StartPosition);
			return null;
		}

		Expression ParsePredefinedLiteral()
		{
			switch ((currentToken as PredefinedLiteralToken).Value)
			{
				case PredefinedLiteral.True: return new Literal<bool>(context, true);
				case PredefinedLiteral.False: return new Literal<bool>(context, false);
				case PredefinedLiteral.Positive: return new Literal<Polarity>(context, Polarity.Positive);
				case PredefinedLiteral.Negative: return new Literal<Polarity>(context, Polarity.Negative);
				case PredefinedLiteral.North: return new Literal<Magnetic>(context, Magnetic.North);
				case PredefinedLiteral.South: return new Literal<Magnetic>(context, Magnetic.South);
				case PredefinedLiteral.Clockwise: return new Literal<Curl>(context, Curl.Clockwise);
				case PredefinedLiteral.Counterclockwise: return new Literal<Curl>(context, Curl.Counterclockwise);
				case PredefinedLiteral.Open: return new Literal<Circuit>(context, Circuit.Open);
				case PredefinedLiteral.Closed: return new Literal<Circuit>(context, Circuit.Closed);
			}

			ThrowException("Expected a literal.", currentToken.StartPosition);
			return null;
		}
	}
}
