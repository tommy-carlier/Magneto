using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TC.Magneto.Modules
{
	/// <summary>Describes the signature of a function argument.</summary>
	public sealed class FunctionArgumentSignature : IEquatable<FunctionArgumentSignature>
	{
		FunctionArgumentSignature(string name, DataType dataType, bool passByReference)
		{
			this.name = name;
			this.dataType = dataType;
			this.passByReference = passByReference;
		}

		readonly string name;
		/// <summary>Gets the name of the argument.</summary>
		/// <value>The name of the argument.</value>
		public string Name { get { return name; } }

		readonly DataType dataType;
		/// <summary>Gets the data type of the argument.</summary>
		/// <value>The data type of the argument.</value>
		public DataType DataType { get { return dataType; } }

		readonly bool passByReference;
		/// <summary>Gets a value indicating whether the argument is passed by reference.</summary>
		/// <value><c>true</c> if the argument is passed by reference; otherwise, <c>false</c>.</value>
		public bool PassByReference { get { return passByReference; } }

		internal static FunctionArgumentSignature Read(ParameterInfo parameterInfo)
		{
			Type parameterType = parameterInfo.ParameterType;
			DataType dataType = DataTypes.GetDataType(parameterType.IsByRef ? parameterType.GetElementType() : parameterType);
			if (dataType == DataType.None) return null;

			return new FunctionArgumentSignature(parameterInfo.Name, dataType, parameterType.IsByRef);
		}

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:Object"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Object"/>.</returns>
		public override string ToString()
		{
			return name + (passByReference ? " : ref " : " : ") + dataType.ToString();
		}

		#region IEquatable<FunctionArgumentSignature> Members

		/// <summary>Determines whether the specified <see cref="T:Object"/> is equal to the current <see cref="T:Object"/>.</summary>
		/// <param name="obj">The <see cref="T:Object"/> to compare with the current <see cref="T:Object"/>.</param>
		/// <returns>true if the specified <see cref="T:Object"/> is equal to the current <see cref="T:Object"/>; otherwise, false.</returns>
		public override bool Equals(object obj) { return Equals(obj as FunctionArgumentSignature); }

		/// <summary>Serves as a hash function for a particular type.</summary>
		/// <returns>A hash code for the current <see cref="T:Object"/>.</returns>
		public override int GetHashCode() { return StringComparer.OrdinalIgnoreCase.GetHashCode(name) ^ dataType.GetHashCode() ^ passByReference.GetHashCode(); }

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		public bool Equals(FunctionArgumentSignature other)
		{
			if (other != null)
				return string.Equals(name, other.name, StringComparison.OrdinalIgnoreCase)
					&& dataType == other.dataType
					&& passByReference == other.passByReference;
			else return false;
		}

		#endregion
	}
}
