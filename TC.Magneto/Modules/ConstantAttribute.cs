using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TC.Magneto.Parsing;

namespace TC.Magneto.Modules
{
	/// <summary>Defines a constant that can be used by a Magneto application.</summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class ConstantAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="ConstantAttribute"/> class.</summary>
		/// <param name="name">The name of the constant.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantAttribute(string name, string value) : this(name, new Constant<string>(value ?? "")) { }

		/// <summary>Initializes a new instance of the <see cref="ConstantAttribute"/> class.</summary>
		/// <param name="name">The name of the constant.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantAttribute(string name, long value) : this(name, new Constant<long>(value)) { }

		/// <summary>Initializes a new instance of the <see cref="ConstantAttribute"/> class.</summary>
		/// <param name="name">The name of the constant.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantAttribute(string name, decimal value) : this(name, new Constant<decimal>(value)) { }

		/// <summary>Initializes a new instance of the <see cref="ConstantAttribute"/> class.</summary>
		/// <param name="name">The name of the constant.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantAttribute(string name, bool value) : this(name, new Constant<bool>(value)) { }

		/// <summary>Initializes a new instance of the <see cref="ConstantAttribute"/> class.</summary>
		/// <param name="name">The name of the constant.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantAttribute(string name, Polarity value) : this(name, new Constant<Polarity>(value)) { }

		/// <summary>Initializes a new instance of the <see cref="ConstantAttribute"/> class.</summary>
		/// <param name="name">The name of the constant.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantAttribute(string name, Magnetic value) : this(name, new Constant<Magnetic>(value)) { }

		/// <summary>Initializes a new instance of the <see cref="ConstantAttribute"/> class.</summary>
		/// <param name="name">The name of the constant.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantAttribute(string name, Curl value) : this(name, new Constant<Curl>(value)) { }

		/// <summary>Initializes a new instance of the <see cref="ConstantAttribute"/> class.</summary>
		/// <param name="name">The name of the constant.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantAttribute(string name, Circuit value) : this(name, new Constant<Circuit>(value)) { }

		ConstantAttribute(string name, Constant constant)
		{
			if (name == null) throw new ArgumentNullException("name");
			if (!Regex.IsMatch(name, @"^[A-Za-z][A-Za-z0-9_]*$")) throw new ArgumentException("name has to start with a letter, followed by zero or more letters, digits or underscores.", "name");
			this.name = name;
			this.constant = constant;
		}

		readonly string name;
		/// <summary>Gets the name of the constant.</summary>
		/// <value>The name of the constant.</value>
		public string Name { get { return name; } }

		readonly Constant constant;
		/// <summary>Creates an expression from this constant.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <returns>The created expression.</returns>
		internal Expression CreateExpression(ParserContext context) { return constant.CreateExpression(context); }

		#region inner class Constant and Constant<T>

		abstract class Constant
		{
			/// <summary>Creates an expression from this constant.</summary>
			/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
			/// <returns>The created expression.</returns>
			public abstract Expression CreateExpression(ParserContext context);
		}

		class Constant<T> : Constant
		{
			readonly T value;
			/// <summary>Initializes a new instance of the <see cref="Constant{T}"/> class.</summary>
			/// <param name="value">The value.</param>
			public Constant(T value) { this.value = value; }

			/// <summary>Creates an expression from this constant.</summary>
			/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
			/// <returns>The created expression.</returns>
			public override Expression CreateExpression(ParserContext context) { return new Literal<T>(context, value); }
		}

		#endregion
	}
}
