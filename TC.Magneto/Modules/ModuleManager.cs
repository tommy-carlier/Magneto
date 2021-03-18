using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using TC.Magneto.Parsing;

namespace TC.Magneto.Modules
{
	/// <summary>Manages the modules that contain the functions.</summary>
	public class ModuleManager
	{
		readonly List<Assembly> assemblies = new List<Assembly>();
		readonly List<Type> moduleTypes = new List<Type>();
		readonly Dictionary<string, List<FunctionSignature>> functions = new Dictionary<string, List<FunctionSignature>>(StringComparer.OrdinalIgnoreCase);
		readonly Dictionary<string, ConstantAttribute> constants = new Dictionary<string, ConstantAttribute>(StringComparer.OrdinalIgnoreCase);

		/// <summary>Initializes a new instance of the <see cref="ModuleManager"/> class.</summary>
		public ModuleManager()
		{
			Register(typeof(ModuleManager).Assembly);
		}

		/// <summary>Gets the registered module types.</summary>
		/// <value>The registered module types.</value>
		public IEnumerable<Type> ModuleTypes { get { return moduleTypes; } }

		#region Register

		/// <summary>Registers the specified assembly.</summary>
		/// <param name="assembly">The assembly to register.</param>
		public void Register(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");
			if (assemblies.Contains(assembly)) throw new ArgumentException("assembly " + assembly.ToString() + " is already registered.", "assembly");

			assemblies.Add(assembly);
			foreach (Type type in assembly.GetTypes())
				if (type.IsPublic && type.IsSubclassOf(typeof(MagnetoModule)) && type.GetConstructor(Type.EmptyTypes) != null)
					Register(type);
		}

		void Register(Type moduleType)
		{
			moduleTypes.Add(moduleType);

			foreach (ConstantAttribute constant in ReflectionUtilities.GetAttributes<ConstantAttribute>(moduleType, false))
				Register(constant);

			foreach (MethodInfo method in moduleType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				FunctionSignature function = FunctionSignature.Read(method);
				if (function != null)
					Register(function);
			}
		}

		void Register(ConstantAttribute constant)
		{
			if (constants.ContainsKey(constant.Name))
				throw new InvalidOperationException("The constant '" + constant.Name + "' is declared multiple times.");

			if (functions.ContainsKey(constant.Name))
				throw new InvalidOperationException("The constant '" + constant.Name + "' is already declared as a function.");

			constants[constant.Name] = constant;
		}

		void Register(FunctionSignature function)
		{
			if (constants.ContainsKey(function.Name))
				throw new InvalidOperationException("The function '" + function.Name + "' is already declared as a constant.");

            if (functions.TryGetValue(function.Name, out List<FunctionSignature> overloads))
            {
                foreach (FunctionSignature lOverload in overloads)
                    if (lOverload.Equals(function))
                        throw new InvalidOperationException("The following function is declared multiple times: " + function.ToString());
                overloads.Add(function);
            }
            else
            {
                overloads = new List<FunctionSignature>(3) { function };
                functions[function.Name] = overloads;
            }
        }

		#endregion

		#region constants

		/// <summary>Indicates whether a constant with the specified name exists.</summary>
		/// <param name="constantName">The name of the constant.</param>
		/// <returns>If a constant with the specified name exists, true; otherwise, false.</returns>
		public bool ConstantExists(string constantName)
		{
			if (constantName == null) throw new ArgumentNullException("constantName");
			return constants.ContainsKey(constantName);
		}

		/// <summary>Gets the <see cref="T:ConstantAttribute"/> with the specified name, or null.</summary>
		/// <param name="constantName">The name of the constant.</param>
		/// <returns>The <see cref="T:ConstantAttribute"/> with the specified name, or null if not found.</returns>
		public ConstantAttribute GetConstant(string constantName)
		{
			if (constantName == null) throw new ArgumentNullException("constantName");
            return constants.TryGetValue(constantName, out ConstantAttribute constant) ? constant : null;
        }

		#endregion

		#region functions

		/// <summary>Indicates whether a function with the specified name exists.</summary>
		/// <param name="functionName">The name of the function.</param>
		/// <returns>If a function with the specified name exists, true; otherwise, false.</returns>
		public bool FunctionExists(string functionName)
		{
			if (functionName == null) throw new ArgumentNullException("functionName");
			return functions.ContainsKey(functionName);
		}

		/// <summary>Gets the overloads of the function with the specified name.</summary>
		/// <param name="functionName">The name of the function.</param>
		/// <returns>The function overloads of the specified function.</returns>
		public IEnumerable<FunctionSignature> GetFunctionOverloads(string functionName)
		{
			if (functionName == null) throw new ArgumentNullException("functionName");
            return functions.TryGetValue(functionName, out List<FunctionSignature> overloads)
                ? overloads : null;
        }

		/// <summary>Gets the <see cref="T:FunctionSignature"/> of the specified function.</summary>
		/// <param name="functionName">The name of the function.</param>
		/// <param name="arguments">The arguments of the function call.</param>
		/// <returns>The <see cref="T:FunctionSignature"/> of the specified function.</returns>
		public FunctionSignature GetFunctionSignature(string functionName, IList<Expression> arguments)
		{
			if (functionName == null) throw new ArgumentNullException("functionName");
			if (arguments == null) throw new ArgumentNullException("arguments");

			return GetFunctionSignatureInternal(functionName, arguments);
		}

		/// <summary>Gets the <see cref="T:FunctionSignature"/> of the specified function.</summary>
		/// <param name="functionName">The name of the function.</param>
		/// <param name="argument">The argument of the function call.</param>
		/// <returns>The <see cref="T:FunctionSignature"/> of the specified function.</returns>
		public FunctionSignature GetFunctionSignature(string functionName, Expression argument)
		{
			if (functionName == null) throw new ArgumentNullException("functionName");
			if (argument == null) throw new ArgumentNullException("argument");

			return GetFunctionSignatureInternal(functionName, new List<Expression>(1) { argument });
		}

		FunctionSignature GetFunctionSignatureInternal(string functionName, IList<Expression> arguments)
		{
            if (!functions.TryGetValue(functionName, out List<FunctionSignature> overloads))
                throw new MagnetoException("Unknown function '" + functionName + "'.");

            FunctionSignature compatibleSignature = null;
			foreach (FunctionSignature signature in overloads)
				switch (Match(signature.Arguments, arguments))
				{
					case ArgumentSignatureMatch.Exact: return signature;
					case ArgumentSignatureMatch.Compatible:
						if (compatibleSignature == null)
							compatibleSignature = signature;
						else throw new MagnetoException("There are multiple possible overloads of the function '" + functionName + "'.");
						break;
				}

			if (compatibleSignature != null) return compatibleSignature;
			else throw new ArgumentException("The arguments don't match the overloads of the function '" + functionName + "'.");
		}

		static ArgumentSignatureMatch Match(IList<FunctionArgumentSignature> argumentSignatures, IList<Expression> arguments)
		{
			int count = argumentSignatures.Count;
			if (count != arguments.Count) return ArgumentSignatureMatch.None;

			bool exactMatch = true;
			for (int i = 0; i < count; i++)
			{
				FunctionArgumentSignature argumentSignature = argumentSignatures[i];
				Expression argument = arguments[i];

				if (argumentSignature.PassByReference != (argument is VariableReference))
					return ArgumentSignatureMatch.None;

				if (argumentSignature.DataType != argument.ResultType)
				{
					exactMatch = false;
					if (argumentSignature.PassByReference
						|| !(argumentSignature.DataType == DataType.Real
							&& argument.ResultType == DataType.Integer))
						return ArgumentSignatureMatch.None;
				}
			}

			return exactMatch ? ArgumentSignatureMatch.Exact : ArgumentSignatureMatch.Compatible;
		}

		enum ArgumentSignatureMatch { None = 0, Exact, Compatible }

		#endregion
	}
}
