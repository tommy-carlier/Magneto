using System;
using System.Collections.Generic;
using System.Text;
using TC.Magneto.Parsing;
using System.IO;
using TC.Magneto.Modules;
using System.Reflection.Emit;
using System.Reflection;

namespace TC.Magneto
{
	/// <summary>Compiles Magneto source code to MSIL.</summary>
	public static class MagnetoCompiler
	{
		/// <summary>Compiles the source code from the specified <see cref="T:Parser"/>.</summary>
		/// <param name="parser">The <see cref="T:Parser"/> that generates an abstract syntax tree.</param>
		/// <returns>A <see cref="T:MethodInfo"/> to execute the compiled code.</returns>
		public static MethodInfo Compile(Parser parser)
		{
			if (parser == null) throw new ArgumentNullException("parser");
			return CompileInternal(parser);
		}

		/// <summary>Compiles the source code from the specified <see cref="T:TextReader"/>.</summary>
		/// <param name="moduleManager">The <see cref="T:ModuleManager"/> that is used for parsing function calls.</param>
		/// <param name="reader">The <see cref="T:TextReader"/> to read source code from.</param>
		/// <returns>A <see cref="T:MethodInfo"/> to execute the compiled code.</returns>
		public static MethodInfo Compile(ModuleManager moduleManager, TextReader reader)
		{
			if (moduleManager == null) throw new ArgumentNullException("moduleManager");
			if (reader == null) throw new ArgumentNullException("reader");
			return CompileInternal(new Parser(new ParserContext(moduleManager), reader));
		}

		/// <summary>Compiles the source code from the specified <see cref="T:Parser"/>.</summary>
		/// <param name="parser">The <see cref="T:Parser"/> that generates an abstract syntax tree.</param>
		/// <param name="ilGenerator">The <see cref="T:ILGenerator"/> to generate MSIL.</param>
		public static void Compile(Parser parser, ILGenerator ilGenerator)
		{
			if (parser == null) throw new ArgumentNullException("parser");
			if (ilGenerator == null) throw new ArgumentNullException("ilGenerator");
			CompileInternal(parser, ilGenerator);
		}

		/// <summary>Compiles the source code from the specified <see cref="T:Parser"/>.</summary>
		/// <param name="moduleManager">The <see cref="T:ModuleManager"/> that is used for parsing function calls.</param>
		/// <param name="reader">The <see cref="T:TextReader"/> to read source code from.</param>
		/// <param name="ilGenerator">The <see cref="T:ILGenerator"/> to generate MSIL.</param>
		public static void Compile(ModuleManager moduleManager, TextReader reader, ILGenerator ilGenerator)
		{
			if (moduleManager == null) throw new ArgumentNullException("moduleManager");
			if (reader == null) throw new ArgumentNullException("reader");
			if (ilGenerator == null) throw new ArgumentNullException("ilGenerator");
			CompileInternal(new Parser(new ParserContext(moduleManager), reader), ilGenerator);
		}

		static MethodInfo CompileInternal(Parser parser)
		{
			DynamicMethod method = new DynamicMethod("Main", typeof(void), Type.EmptyTypes, null as Module);
			ILGenerator generator = method.GetILGenerator();
			CompileInternal(parser, generator);
			generator.Emit(OpCodes.Ret);
			return method;
		}

		static void CompileInternal(Parser parser, ILGenerator ilGenerator)
		{
			CompilationVisitor visitor = new CompilationVisitor(parser.Context, ilGenerator);
			parser.ReadTopLevelStatements(visitor);
			visitor.EmitFooter();
		}

		#region inner class CompilationVisitor

		sealed class CompilationVisitor : IStatementVisitor, IExpressionVisitor
		{
			/// <summary>Initializes a new instance of the <see cref="CompilationVisitor"/> class.</summary>
			public CompilationVisitor(ParserContext parserContext, ILGenerator ilGenerator)
			{
				generator = new MsilGenerator(ilGenerator);

				// begin the try { ... } block
				generator.BeginExceptionBlock();

				MethodInfo startMethod = GetMethod<MagnetoModule>("Start");

				// declare a local variable per module
				foreach (Type moduleType in parserContext.ModuleManager.ModuleTypes)
				{
					// local m = new T();
					// m.Start();
					LocalBuilder moduleLocal = generator.DeclareLocal(moduleType);
					generator.NewObject(moduleType);
					generator.Emit(OpCodes.Dup);
					generator.StoreTo(moduleLocal);
					generator.Call(startMethod);
					moduleLocals.Add(moduleLocal);
					moduleLocalsByType[moduleType] = moduleLocal;
				}
			}

			readonly MsilGenerator generator;
			readonly List<LocalBuilder> moduleLocals = new List<LocalBuilder>();
			readonly Dictionary<Type, LocalBuilder> moduleLocalsByType = new Dictionary<Type, LocalBuilder>();
			readonly Dictionary<int, LocalBuilder> variableLocals = new Dictionary<int, LocalBuilder>();
			Label currentLoopAfterLabel;

			/// <summary>Emits the footer MSIL-code.</summary>
			public void EmitFooter()
			{
				// start the finally { ... } block
				generator.BeginFinallyBlock();

				MethodInfo stopMethod = GetMethod<MagnetoModule>("Stop");

				// in the finally-block: stop the modules
				foreach (LocalBuilder moduleLocal in moduleLocals)
				{
					// if (m != null) m.Stop();
					Label labelModuleIsNull = generator.DefineLabel();
					generator.LoadFrom(moduleLocal);
					generator.IfFalseGoto(labelModuleIsNull);
					generator.Call(moduleLocal, stopMethod);
					generator.MarkLabel(labelModuleIsNull);
				}

				// end the try { ... } finally { ... } block
				generator.EndExceptionBlock();
			}

			LocalBuilder GetVariableLocal(Variable variable)
			{
                if (!variableLocals.TryGetValue(variable.ID, out LocalBuilder local))
                {
                    variableLocals[variable.ID] = local = generator.DeclareLocal(variable.DataType);
                }

                return local;
			}

			const BindingFlags
				PublicInstance = BindingFlags.Public | BindingFlags.Instance,
				PublicStatic = BindingFlags.Public | BindingFlags.Static;

			static MethodInfo GetMethod<TTarget>(string name) { return GetMethod<TTarget>(name, PublicInstance); }
			static MethodInfo GetMethod<TTarget>(string name, BindingFlags flags)
			{
				return typeof(TTarget).GetMethod(name, flags, null, Type.EmptyTypes, null);
			}

			static MethodInfo GetMethod<TTarget, TArg1>(string name) { return GetMethod<TTarget, TArg1>(name, PublicInstance); }
			static MethodInfo GetMethod<TTarget, TArg1>(string name, BindingFlags flags)
			{
				return typeof(TTarget).GetMethod(name, flags, null, new Type[] { typeof(TArg1) }, null);
			}

			static MethodInfo GetMethod<TTarget, TArg1, TArg2>(string name) { return GetMethod<TTarget, TArg1, TArg2>(name, PublicInstance); }
			static MethodInfo GetMethod<TTarget, TArg1, TArg2>(string name, BindingFlags flags)
			{
				return typeof(TTarget).GetMethod(name, flags, null, new Type[] { typeof(TArg1), typeof(TArg2) }, null);
			}

			static MethodInfo GetMethod<TTarget, TArg1, TArg2, TArg3>(string name) { return GetMethod<TTarget, TArg1, TArg2, TArg3>(name, PublicInstance); }
			static MethodInfo GetMethod<TTarget, TArg1, TArg2, TArg3>(string name, BindingFlags flags)
			{
				return typeof(TTarget).GetMethod(name, flags, null, new Type[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) }, null);
			}

			static MethodInfo GetMethod<TTarget, TArg1, TArg2, TArg3, TArg4>(string name) { return GetMethod<TTarget, TArg1, TArg2, TArg3, TArg4>(name, PublicInstance); }
			static MethodInfo GetMethod<TTarget, TArg1, TArg2, TArg3, TArg4>(string name, BindingFlags flags)
			{
				return typeof(TTarget).GetMethod(name, flags, null, new Type[] { typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4) }, null);
			}

			#region IStatementVisitor Members

			void IStatementVisitor.Visit(IfStatement statement)
			{
				Label labelFalse = generator.DefineLabel();
				Label labelEnd = generator.DefineLabel();
				statement.Condition.AcceptInternal(this);
				generator.IfFalseGoto(labelFalse);
				statement.TrueStatements.AcceptInternal(this);
				generator.Goto(labelEnd);
				generator.MarkLabel(labelFalse);
				statement.FalseStatements.AcceptInternal(this);
				generator.MarkLabel(labelEnd);
			}

			void IStatementVisitor.Visit(VariableDeclaration statement)
			{
				// variables are declared when they are needed (in method GetVariableLocal())
			}

			void IStatementVisitor.Visit(Assignment statement)
			{
				statement.Value.AcceptInternal(this);
				generator.StoreTo(GetVariableLocal(statement.Variable));
			}

			void IStatementVisitor.Visit(SwitchStatement statement)
			{
				DataType valueType = statement.Value.ResultType;
				LocalBuilder localValue = generator.DeclareLocal(valueType);
				statement.Value.AcceptInternal(this);
				generator.StoreTo(localValue);
				Label labelEnd = generator.DefineLabel();

				foreach (CaseStatement @case in statement.Cases)
				{
					Label labelEndCase = generator.DefineLabel();
					generator.LoadFrom(localValue);
					@case.Value.AcceptInternal(this);
					CompareEquality(valueType);
					generator.IfFalseGoto(labelEndCase);
					@case.Body.AcceptInternal(this);
					generator.Goto(labelEnd);
					generator.MarkLabel(labelEndCase);
				}

				if (statement.ElseBody != null)
					statement.ElseBody.AcceptInternal(this);
				generator.MarkLabel(labelEnd);
			}

			void IStatementVisitor.Visit(CaseStatement statement)
			{
				// cannot occur (compiled by switch-statement)
			}

			void IStatementVisitor.Visit(WhileLoopStatement statement)
			{
				Label previousLoopAfterLabel = currentLoopAfterLabel;
				currentLoopAfterLabel = generator.DefineLabel();
				Label labelBegin = generator.DefineAndMarkLabel();
				statement.Condition.AcceptInternal(this);
				generator.IfFalseGoto(currentLoopAfterLabel);
				statement.Body.AcceptInternal(this);
				generator.Goto(labelBegin);
				generator.MarkLabel(currentLoopAfterLabel);
				currentLoopAfterLabel = previousLoopAfterLabel;
			}

			void IStatementVisitor.Visit(RepeatLoopStatement statement)
			{
				Label previousLoopAfterLabel = currentLoopAfterLabel;
				currentLoopAfterLabel = generator.DefineLabel();
				Label labelBegin = generator.DefineAndMarkLabel();
				statement.Body.AcceptInternal(this);
				statement.Condition.AcceptInternal(this);
				generator.IfFalseGoto(labelBegin);
				generator.MarkLabel(currentLoopAfterLabel);
				currentLoopAfterLabel = previousLoopAfterLabel;
			}

			static readonly MethodInfo 
				forNextCheckInt64Method = GetMethod<CoreFunctions, long, long, long>("CheckForNext", PublicStatic),
				forNextCheckDecimalMethod = GetMethod<CoreFunctions, decimal, decimal, decimal>("CheckForNext", PublicStatic);

			void IStatementVisitor.Visit(ForLoopStatement statement)
			{
				DataType dataType = statement.Variable.DataType;
				Label previousLoopAfterLabel = currentLoopAfterLabel;
				currentLoopAfterLabel = generator.DefineLabel();

				LocalBuilder localVariable = GetVariableLocal(statement.Variable);
				statement.StartExpression.AcceptInternal(this);
				generator.StoreTo(localVariable);

				LocalBuilder localEnd = generator.DeclareLocal(dataType);
				statement.EndExpression.AcceptInternal(this);
				generator.StoreTo(localEnd);

				LocalBuilder localStep = generator.DeclareLocal(dataType);
				statement.StepExpression.AcceptInternal(this);
				generator.StoreTo(localStep);

				Label labelBegin = generator.DefineAndMarkLabel();
				generator.LoadFrom(localVariable);
				generator.LoadFrom(localEnd);
				generator.LoadFrom(localStep);
				generator.Call(
					dataType == DataType.Real
						? forNextCheckDecimalMethod
						: forNextCheckInt64Method);

				generator.IfFalseGoto(currentLoopAfterLabel);
				statement.Body.AcceptInternal(this);
				generator.LoadFrom(localVariable);
				generator.LoadFrom(localStep);
				if (dataType == DataType.Real)
					generator.Call(decimalAddMethod);
				else generator.Emit(OpCodes.Add);
				generator.StoreTo(localVariable);

				generator.Goto(labelBegin);
				generator.MarkLabel(currentLoopAfterLabel);
				currentLoopAfterLabel = previousLoopAfterLabel;
			}

			void IStatementVisitor.Visit(FunctionCallStatement statement)
			{
				statement.FunctionCall.AcceptInternal(this);
				if (statement.FunctionCall.ResultType != DataType.None)
					generator.Emit(OpCodes.Pop);
			}

			void IStatementVisitor.Visit(BreakStatement statement)
			{
				generator.Goto(currentLoopAfterLabel);
			}

			void IStatementVisitor.Visit(ExitStatement statement)
			{
				generator.Emit(OpCodes.Ret);
			}

			#endregion

			#region IExpressionVisitor Members

			static readonly MethodInfo decimalNegateMethod = GetMethod<decimal, decimal>("Negate", PublicStatic);

			void IExpressionVisitor.Visit(Negation expression)
			{
				expression.ChildExpression.AcceptInternal(this);
				switch (expression.ResultType)
				{
					case DataType.Integer: generator.Emit(OpCodes.Neg); break;
					case DataType.Real: generator.Call(decimalNegateMethod); break;
				}
			}

			void IExpressionVisitor.Visit(NotExpression expression)
			{
				expression.ChildExpression.AcceptInternal(this);
				generator.ApplyNot();
			}

			void IExpressionVisitor.Visit(VariableExpression expression)
			{
				generator.LoadFrom(GetVariableLocal(expression.Variable));
			}

			void IExpressionVisitor.Visit(VariableReference expression)
			{
				generator.LoadAddress(GetVariableLocal(expression.Variable));
			}

			static readonly MethodInfo convertIntegerToRealMethod = GetMethod<ConversionModule, long>("ConvertToReal");
			static readonly ConstructorInfo decimalInt64Constructor = typeof(decimal).GetConstructor(new Type[] { typeof(long) });

			void IExpressionVisitor.Visit(FunctionCall expression)
			{
				MethodInfo method = expression.Signature.MethodInfo;
				if (method == convertIntegerToRealMethod)
				{
					// optimize toReal(integer)
					using (IEnumerator<Expression> enumerator = expression.Arguments.GetEnumerator())
						if (enumerator.MoveNext())
						{
							enumerator.Current.AcceptInternal(this);
							generator.NewObject(decimalInt64Constructor);
						}
				}
				else EmitFunctionCall(method
					, moduleLocalsByType[expression.Signature.MethodInfo.DeclaringType]
					, expression.Arguments);
			}

			void EmitFunctionCall(MethodInfo method, LocalBuilder target, IEnumerable<Expression> arguments)
			{
				if (target != null) generator.LoadFrom(target);
				foreach (Expression argument in arguments)
					argument.AcceptInternal(this);
				generator.Call(method);
			}

			void IExpressionVisitor.Visit<T>(Literal<T> expression)
			{
				switch (expression.ResultType)
				{
					case DataType.String: generator.LoadConstant((expression as Literal<string>).Value); break;
					case DataType.Integer: generator.LoadConstant((expression as Literal<long>).Value); break;
					case DataType.Real: generator.LoadConstant((expression as Literal<decimal>).Value); break;
					case DataType.Logic: generator.LoadConstant((expression as Literal<bool>).Value); break;
					case DataType.Polarity: generator.LoadConstant((int)(expression as Literal<Polarity>).Value); break;
					case DataType.Magnetic: generator.LoadConstant((int)(expression as Literal<Magnetic>).Value); break;
					case DataType.Curl: generator.LoadConstant((int)(expression as Literal<Curl>).Value); break;
					case DataType.Circuit: generator.LoadConstant((int)(expression as Literal<Circuit>).Value); break;
				}
			}

			void IExpressionVisitor.Visit(AndExpression expression)
			{
				Label labelFalse = generator.DefineLabel();
				Label labelEnd = generator.DefineLabel();
				expression.LeftChildExpression.AcceptInternal(this);
				if (expression.ResultType != DataType.Logic)
				{
					expression.TrueValue.AcceptInternal(this);
					generator.Emit(OpCodes.Ceq);
				}
				generator.IfFalseGoto(labelFalse);
				expression.RightChildExpression.AcceptInternal(this);
				generator.Goto(labelEnd);
				generator.MarkLabel(labelFalse);
				expression.FalseValue.AcceptInternal(this);
				generator.MarkLabel(labelEnd);
			}

			void IExpressionVisitor.Visit(OrExpression expression)
			{
				Label labelTrue = generator.DefineLabel();
				Label labelEnd = generator.DefineLabel();
				expression.LeftChildExpression.AcceptInternal(this);
				if (expression.ResultType != DataType.Logic)
				{
					expression.TrueValue.AcceptInternal(this);
					generator.Emit(OpCodes.Ceq);
				}
				generator.IfTrueGoto(labelTrue);
				expression.RightChildExpression.AcceptInternal(this);
				generator.Goto(labelEnd);
				generator.MarkLabel(labelTrue);
				expression.TrueValue.AcceptInternal(this);
				generator.MarkLabel(labelEnd);
			}

			void IExpressionVisitor.Visit(XorExpression expression)
			{
				Label labelFalse = generator.DefineLabel();
				Label labelEnd = generator.DefineLabel();
				expression.LeftChildExpression.AcceptInternal(this);
				expression.RightChildExpression.AcceptInternal(this);
				generator.Emit(OpCodes.Ceq);
				generator.IfFalseGoto(labelFalse);
				expression.FalseValue.AcceptInternal(this);
				generator.Goto(labelEnd);
				generator.MarkLabel(labelFalse);
				expression.TrueValue.AcceptInternal(this);
				generator.MarkLabel(labelEnd);
			}

			static readonly MethodInfo decimalAddMethod = GetMethod<decimal, decimal, decimal>("Add", PublicStatic);
			void IExpressionVisitor.Visit(Addition expression)
			{
				expression.LeftChildExpression.AcceptInternal(this);
				expression.RightChildExpression.AcceptInternal(this);
				if (expression.ResultType == DataType.Real)
					generator.Call(decimalAddMethod);
				else generator.Emit(OpCodes.Add);
			}

			static readonly MethodInfo decimalSubtractMethod = GetMethod<decimal, decimal, decimal>("Subtract", PublicStatic);
			void IExpressionVisitor.Visit(Subtraction expression)
			{
				expression.LeftChildExpression.AcceptInternal(this);
				expression.RightChildExpression.AcceptInternal(this);
				if (expression.ResultType == DataType.Real)
					generator.Call(decimalSubtractMethod);
				else generator.Emit(OpCodes.Sub);
			}

			static readonly MethodInfo decimalMultiplyMethod = GetMethod<decimal, decimal, decimal>("Multiply", PublicStatic);
			void IExpressionVisitor.Visit(Multiplication expression)
			{
				expression.LeftChildExpression.AcceptInternal(this);
				expression.RightChildExpression.AcceptInternal(this);
				if (expression.ResultType == DataType.Real)
					generator.Call(decimalMultiplyMethod);
				else generator.Emit(OpCodes.Mul);
			}

			static readonly MethodInfo decimalDivideMethod = GetMethod<decimal, decimal, decimal>("Divide", PublicStatic);
			void IExpressionVisitor.Visit(Division expression)
			{
				expression.LeftChildExpression.AcceptInternal(this);
				expression.RightChildExpression.AcceptInternal(this);
				if (expression.ResultType == DataType.Real)
					generator.Call(decimalDivideMethod);
				else generator.Emit(OpCodes.Div);
			}

			static readonly MethodInfo decimalRemainderMethod = GetMethod<decimal, decimal, decimal>("Remainder", PublicStatic);
			void IExpressionVisitor.Visit(ModuloExpression expression)
			{
				expression.LeftChildExpression.AcceptInternal(this);
				expression.RightChildExpression.AcceptInternal(this);
				if (expression.ResultType == DataType.Real)
					generator.Call(decimalRemainderMethod);
				else generator.Emit(OpCodes.Rem);
			}

			static readonly MethodInfo powerMethod = GetMethod<CoreFunctions, decimal, long>("Power", PublicStatic);
			void IExpressionVisitor.Visit(PowerExpression expression)
			{
				expression.LeftChildExpression.AcceptInternal(this);
				expression.RightChildExpression.AcceptInternal(this);
				generator.Call(powerMethod);
			}

			void IExpressionVisitor.Visit(Comparison expression)
			{
				expression.LeftChildExpression.AcceptInternal(this);
				expression.RightChildExpression.AcceptInternal(this);
				switch (expression.Operator)
				{
					case ComparisonOperator.Equal:
						CompareEquality(expression.LeftChildExpression.ResultType);
						break;
					case ComparisonOperator.NotEqual:
						CompareEquality(expression.LeftChildExpression.ResultType);
						generator.ApplyNot();
						break;
					default:
						if (expression.LeftChildExpression.ResultType == DataType.Real)
						{
							generator.Call(decimalCompareMethod);
							generator.LoadConstant(0);
						}
						else if (expression.LeftChildExpression.ResultType == DataType.String)
						{
							generator.Call(stringCompareMethod);
							generator.LoadConstant(0);
						}
						Compare(expression.Operator);
						break;
				}
			}

			static readonly MethodInfo
				decimalCompareMethod = GetMethod<decimal, decimal, decimal>("Compare", PublicStatic),
				stringCompareMethod = GetMethod<string, string, string>("Compare", PublicStatic),
				stringEqualMethod = GetMethod<string, string, string>("Equals", PublicStatic);

			void CompareEquality(DataType dataType)
			{
				switch (dataType)
				{
					case DataType.Real:
						generator.Call(decimalCompareMethod);
						generator.ApplyNot();
						break;
					case DataType.String:
						generator.Call(stringEqualMethod);
						break;
					default:
						generator.Emit(OpCodes.Ceq);
						break;
				}
			}

			void Compare(ComparisonOperator @operator)
			{
				switch (@operator)
				{
					case ComparisonOperator.LessThan:
						generator.Emit(OpCodes.Clt);
						break;
					case ComparisonOperator.GreaterThan:
						generator.Emit(OpCodes.Cgt);
						break;
					case ComparisonOperator.LessThanOrEqual:
						generator.Emit(OpCodes.Cgt);
						generator.ApplyNot();
						break;
					case ComparisonOperator.GreaterThanOrEqual:
						generator.Emit(OpCodes.Clt);
						generator.ApplyNot();
						break;
				}
			}

			static readonly MethodInfo
				concatenate2Method = GetMethod<string, string, string>("Concat", PublicStatic),
				concatenate3Method = GetMethod<string, string, string, string>("Concat", PublicStatic),
				concatenate4Method = GetMethod<string, string, string, string, string>("Concat", PublicStatic),
				concatenateArrayMethod = GetMethod<string, string[]>("Concat", PublicStatic);

			void IExpressionVisitor.Visit(Concatenation expression)
			{
				switch (expression.ChildExpressions.Length)
				{
					case 2: Concatenate(expression.ChildExpressions, concatenate2Method); break;
					case 3: Concatenate(expression.ChildExpressions, concatenate3Method); break;
					case 4: Concatenate(expression.ChildExpressions, concatenate4Method); break;
					default: ConcatenateArray(expression.ChildExpressions); break;
				}
			}

			void Concatenate(Expression[] expressions, MethodInfo method)
			{
				foreach (Expression expression in expressions)
					EmitConcatenationExpression(expression);
				generator.Call(method);
			}

			void ConcatenateArray(Expression[] expressions)
			{
				int length = expressions.Length;
				generator.NewArray(typeof(string), length);
				for (int i = 0; i < length; i++)
					generator.Emit(OpCodes.Dup);
				for (int i = 0; i < length; i++)
				{
					generator.LoadConstant(i);
					EmitConcatenationExpression(expressions[i]);
					generator.Emit(OpCodes.Stelem_Ref);
				}
				generator.Call(concatenateArrayMethod);
			}

			static readonly MethodInfo
				convertIntegerToStringMethod = GetMethod<CoreFunctions, long>("ConvertToString", PublicStatic),
				convertRealToStringMethod = GetMethod<CoreFunctions, decimal>("ConvertToString", PublicStatic),
				convertLogicToStringMethod = GetMethod<CoreFunctions, bool>("ConvertToString", PublicStatic),
				convertPolarityToStringMethod = GetMethod<CoreFunctions, Polarity>("ConvertToString", PublicStatic),
				convertMagneticToStringMethod = GetMethod<CoreFunctions, Magnetic>("ConvertToString", PublicStatic),
				convertCurlToStringMethod = GetMethod<CoreFunctions, Curl>("ConvertToString", PublicStatic),
				convertCircuitToStringMethod = GetMethod<CoreFunctions, Circuit>("ConvertToString", PublicStatic);

			void EmitConcatenationExpression(Expression expression)
			{
				expression.AcceptInternal(this);
				switch (expression.ResultType)
				{
					case DataType.Integer: generator.Call(convertIntegerToStringMethod); break;
					case DataType.Real: generator.Call(convertRealToStringMethod); break;
					case DataType.Logic: generator.Call(convertLogicToStringMethod); break;
					case DataType.Polarity: generator.Call(convertPolarityToStringMethod); break;
					case DataType.Magnetic: generator.Call(convertMagneticToStringMethod); break;
					case DataType.Curl: generator.Call(convertCurlToStringMethod); break;
					case DataType.Circuit: generator.Call(convertCircuitToStringMethod); break;
				}
			}

			#endregion
		}

		#endregion
	}
}
