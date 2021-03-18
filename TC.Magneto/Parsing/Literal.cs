using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a literal.</summary>
	/// <typeparam name="T">The type of the literal value.</typeparam>
	public class Literal<T> : Expression
	{
		static readonly DataType dataType = DataTypes.GetDataType(typeof(T));

		/// <summary>Initializes a new instance of the <see cref="Literal{T}"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="value">The value.</param>
		internal Literal(ParserContext context, T value)
			: base(context)
		{
			if (dataType == DataType.None) throw new ArgumentException("Invalid type of value.", "value");
			this.value = value;
		}

		readonly T value;
		/// <summary>Gets the value.</summary>
		/// <value>The value.</value>
		public T Value { get { return value; } }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public override DataType ResultType { get { return dataType; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
