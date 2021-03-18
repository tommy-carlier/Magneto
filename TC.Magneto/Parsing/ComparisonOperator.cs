using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents the operator of a comparison.</summary>
	public enum ComparisonOperator
	{
		/// <summary>No operator.</summary>
		None = 0,

		/// <summary>The 'equal'-operator '=='.</summary>
		Equal,

		/// <summary>The 'not equal'-operator '&lt;&gt;'.</summary>
		NotEqual,

		/// <summary>The 'less than'-operator '&lt;'.</summary>
		LessThan,

		/// <summary>The 'greater than'-operator '&gt;'.</summary>
		GreaterThan,

		/// <summary>The 'less than or equal'-operator '&lt;='.</summary>
		LessThanOrEqual,

		/// <summary>The 'greater than or equal'-operator '&gt;='.</summary>
		GreaterThanOrEqual
	}
}
