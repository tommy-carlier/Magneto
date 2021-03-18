using System;
using System.Collections.Generic;
using System.Text;
using TC.Magneto.Modules;
using TC.Magneto.Tokenizing;

namespace TC.Magneto.Parsing
{
	/// <summary>Contains all the state, assembled while parsing source code.</summary>
	public class ParserContext
	{
		readonly List<Variable> variables = new List<Variable>();
		readonly List<VariableScope> scopes = new List<VariableScope>();
		bool polarityIsBoolean, magneticIsBoolean, curlIsBoolean, circuitIsBoolean;
		bool positiveIsTrue, northIsTrue, clockwiseIsTrue, openIsTrue;

		/// <summary>Initializes a new instance of the <see cref="ParserContext"/> class.</summary>
		/// <param name="moduleManager">The <see cref="T:ModuleManager"/> that is used for parsing function calls.</param>
		public ParserContext(ModuleManager moduleManager)
		{
            this.moduleManager = moduleManager ?? throw new ArgumentNullException("moduleManager");
			BeginScope();
		}

		readonly ModuleManager moduleManager;
		/// <summary>Gets the <see cref="T:ModuleManager"/>.</summary>
		/// <value>The <see cref="T:ModuleManager"/>.</value>
		public ModuleManager ModuleManager { get { return moduleManager; } }

		#region variables and variable scopes

		/// <summary>Gets a value indicating whether the parser is currently parsing top-level statements.</summary>
		/// <value><c>true</c> if parsing top-level statements; otherwise, <c>false</c>.</value>
		bool TopLevel { get { return scopes.Count <= 1; } }

		/// <summary>Begins a new scope.</summary>
		internal void BeginScope() { scopes.Add(new VariableScope()); }

		/// <summary>Ends the current scope.</summary>
		internal void EndScope()
		{
			if (scopes.Count == 0) throw new InvalidOperationException();
			scopes.RemoveAt(scopes.Count - 1);
		}

		/// <summary>Gets the variable for the current scope with the specified name.</summary>
		/// <param name="name">The name of the variable to get.</param>
		/// <returns>The variable with the specified name, or null if not found.</returns>
		internal Variable GetVariable(string name)
		{
			for (int i = scopes.Count - 1; i >= 0; i--)
			{
				Variable variable = scopes[i].GetVariable(name);
				if (variable != null) return variable;
			}
			return null;
		}

		/// <summary>Declares a new variable for the current scope.</summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="dataType">The data type of the variable.</param>
		/// <param name="currentPosition">The current position in the source code.</param>
		/// <returns>The declared variable.</returns>
		internal Variable DeclareVariable(string name, DataType dataType, TextPosition currentPosition)
		{
			if (scopes.Count == 0) throw new InvalidOperationException();
			if (GetVariable(name) != null) throw new ParserException("The variable '" + name + "' has already been declared.", currentPosition);
			if (moduleManager.FunctionExists(name)) throw new ParserException("Cannot declare variable '" + name + "': a function with that name exists.", currentPosition);
			if (moduleManager.ConstantExists(name)) throw new ParserException("Cannot declare variable '" + name + "': a constant with that name exists.", currentPosition);
			
			Variable variable = scopes[scopes.Count - 1].DeclareVariable(variables.Count, name, dataType);
			variables.Add(variable);
			return variable;
		}

		#region inner class VariableScope

		class VariableScope
		{
			readonly Dictionary<string, Variable> variables = new Dictionary<string, Variable>(StringComparer.OrdinalIgnoreCase);

			internal Variable GetVariable(string name)
			{
                return variables.TryGetValue(name, out Variable variable) ? variable : null;
            }

			internal Variable DeclareVariable(int id, string name, DataType dataType)
			{
				Variable variable = new Variable(id, name, dataType);
				variables[name] = variable;
				return variable;
			}
		}

		#endregion

		#endregion

		#region boolean declarations

		/// <summary>Declares that the specified literal represents a boolean value.</summary>
		/// <param name="literal">The literal to declare as boolean.</param>
		/// <param name="value">The boolean value of the specified literal.</param>
		/// <param name="position">The start position of the boolean declaration in the source code.</param>
		public void DeclareBoolean(PredefinedLiteral literal, bool value, TextPosition position)
		{
			if (!TopLevel) throw new ParserException("Boolean declarations are only allowed as top-level statements.", position);

			switch (literal)
			{
				case PredefinedLiteral.Positive: polarityIsBoolean = true; positiveIsTrue = value; break;
				case PredefinedLiteral.Negative: polarityIsBoolean = true; positiveIsTrue = !value; break;
				case PredefinedLiteral.North: magneticIsBoolean = true; northIsTrue = value; break;
				case PredefinedLiteral.South: magneticIsBoolean = true; northIsTrue = !value; break;
				case PredefinedLiteral.Clockwise: curlIsBoolean = true; clockwiseIsTrue = value; break;
				case PredefinedLiteral.Counterclockwise: clockwiseIsTrue = true; clockwiseIsTrue = !value; break;
				case PredefinedLiteral.Open: circuitIsBoolean = true; openIsTrue = value; break;
				case PredefinedLiteral.Closed: circuitIsBoolean = true; openIsTrue = !value; break;
			}
		}

		/// <summary>Determines whether the specified data type is boolean.</summary>
		/// <param name="dataType">The data type to check.</param>
		/// <returns><c>true</c> if the specified data type is boolean; otherwise, <c>false</c>.</returns>
		public bool IsBoolean(DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Logic: return true;
				case DataType.Polarity: return polarityIsBoolean;
				case DataType.Magnetic: return magneticIsBoolean;
				case DataType.Curl: return curlIsBoolean;
				case DataType.Circuit: return circuitIsBoolean;
				default: return false;
			}
		}

		/// <summary>Gets the <see cref="T:Polarity"/>-value that represents 'true'.</summary>
		/// <value>The <see cref="T:Polarity"/>-value that represents 'true'.</value>
		public Polarity PolarityTrueValue { get { return positiveIsTrue ? Polarity.Positive : Polarity.Negative; } }

		/// <summary>Gets the <see cref="T:Polarity"/>-value that represents 'false'.</summary>
		/// <value>The <see cref="T:Polarity"/>-value that represents 'false'.</value>
		public Polarity PolarityFalseValue { get { return positiveIsTrue ? Polarity.Negative : Polarity.Positive; } }

		/// <summary>Gets the <see cref="T:Magnetic"/>-value that represents 'true'.</summary>
		/// <value>The <see cref="T:Magnetic"/>-value that represents 'true'.</value>
		public Magnetic MagneticTrueValue { get { return northIsTrue ? Magnetic.North : Magnetic.South; } }

		/// <summary>Gets the <see cref="T:Magnetic"/>-value that represents 'false'.</summary>
		/// <value>The <see cref="T:Magnetic"/>-value that represents 'false'.</value>
		public Magnetic MagneticFalseValue { get { return positiveIsTrue ? Magnetic.South : Magnetic.North; } }

		/// <summary>Gets the <see cref="T:Curl"/>-value that represents 'true'.</summary>
		/// <value>The <see cref="T:Curl"/>-value that represents 'true'.</value>
		public Curl CurlTrueValue { get { return clockwiseIsTrue ? Curl.Clockwise : Curl.Counterclockwise; } }

		/// <summary>Gets the <see cref="T:Curl"/>-value that represents 'false'.</summary>
		/// <value>The <see cref="T:Curl"/>-value that represents 'false'.</value>
		public Curl CurlFalseValue { get { return clockwiseIsTrue ? Curl.Counterclockwise : Curl.Clockwise; } }

		/// <summary>Gets the <see cref="T:Circuit"/>-value that represents 'true'.</summary>
		/// <value>The <see cref="T:Circuit"/>-value that represents 'true'.</value>
		public Circuit CircuitTrueValue { get { return openIsTrue ? Circuit.Open : Circuit.Closed; } }

		/// <summary>Gets the <see cref="T:Circuit"/>-value that represents 'false'.</summary>
		/// <value>The <see cref="T:Circuit"/>-value that represents 'false'.</value>
		public Circuit CircuitFalseValue { get { return openIsTrue ? Circuit.Closed : Circuit.Open; } }

		#endregion
	}
}
