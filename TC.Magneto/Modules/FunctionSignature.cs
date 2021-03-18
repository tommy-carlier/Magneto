using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace TC.Magneto.Modules
{
	/// <summary>Describes the signature of a function.</summary>
	public sealed class FunctionSignature : IEquatable<FunctionSignature>
	{
		FunctionSignature(MethodInfo methodInfo, string name, DataType resultType, IList<FunctionArgumentSignature> arguments)
		{
			this.methodInfo = methodInfo;
			this.name = name;
			this.resultType = resultType;
			this.arguments = arguments;
		}

		readonly MethodInfo methodInfo;
		/// <summary>Gets the <see cref="T:MethodInfo"/> of the function.</summary>
		/// <value>The <see cref="T:MethodInfo"/> of the function.</value>
		public MethodInfo MethodInfo { get { return methodInfo; } }

		readonly string name;
		/// <summary>Gets the name of the function.</summary>
		/// <value>The name of the function.</value>
		public string Name { get { return name; } }

		readonly DataType resultType;
		/// <summary>Gets the data type of the result.</summary>
		/// <value>The data type of the result.</value>
		public DataType ResultType { get { return resultType; } }

		readonly IList<FunctionArgumentSignature> arguments;
		/// <summary>Gets the signatures of the arguments of the function.</summary>
		/// <value>The signatures of the arguments of the function.</value>
		public IList<FunctionArgumentSignature> Arguments { get { return arguments; } }

		internal static FunctionSignature Read(MethodInfo method)
		{
			// check whether the method has a FunctionAttribute
			FunctionAttribute attribute = ReflectionUtilities.GetFirstAttribute<FunctionAttribute>(method, false);
			if (attribute == null) return null;

			// get the result type of the function
			DataType resultType;
			Type returnType = method.ReturnType;
			if (returnType == typeof(void))
				resultType = DataType.None;
			else
			{
				resultType = DataTypes.GetDataType(returnType);
				if (resultType == DataType.None) return null;
			}

			// get the function arguments
			ParameterInfo[] parameterInfos = method.GetParameters();
			List<FunctionArgumentSignature> arguments = new List<FunctionArgumentSignature>(parameterInfos.Length);
			foreach (ParameterInfo parameterInfo in parameterInfos)
			{
				FunctionArgumentSignature argument = FunctionArgumentSignature.Read(parameterInfo);
				if (argument != null) arguments.Add(argument);
				else return null;
			}

			return new FunctionSignature(method, attribute.Name, resultType, new ReadOnlyCollection<FunctionArgumentSignature>(arguments));
		}

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Object"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Object"/>.</returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(name + "(");
			bool first = true;
			foreach (FunctionArgumentSignature argument in arguments)
			{
				if (first) first = false;
				else builder.Append(", ");
				builder.Append(argument.ToString());
			}
			builder.Append(") : " + resultType.ToString());
			return builder.ToString();
		}

		#region IEquatable<FunctionSignature> Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		public bool Equals(FunctionSignature other)
		{
			if (other != null)
				return string.Equals(name, other.name, StringComparison.OrdinalIgnoreCase)
					&& Equals(arguments, other.arguments);
			else return false;
		}

		static bool Equals(IList<FunctionArgumentSignature> args1, IList<FunctionArgumentSignature> args2)
		{
			if (args1.Count != args2.Count) return false;
			for (int i = args1.Count - 1; i >= 0; i--)
				if (!args1[i].Equals(args2[i]))
					return false;
			return true;
		}

		#endregion
	}
}
