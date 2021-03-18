using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TC.Magneto.Modules
{
	/// <summary>Defines a function that can be called by a Magneto application.</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class FunctionAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="FunctionAttribute"/> class.</summary>
		/// <param name="name">The name of the function.</param>
		public FunctionAttribute(string name)
		{
			if (name == null) throw new ArgumentNullException("name");
			if (!Regex.IsMatch(name, @"^[A-Za-z][A-Za-z0-9_]*$")) throw new ArgumentException("name has to start with a letter, followed by zero or more letters, digits or underscores.", "name");
			this.name = name;
		}

		readonly string name;
		/// <summary>Gets the name of the function.</summary>
		/// <value>The name of the function.</value>
		public string Name { get { return name; } }
	}
}
