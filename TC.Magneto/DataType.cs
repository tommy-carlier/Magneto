using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto
{
	/// <summary>Represents one of the predefined data types.</summary>
	public enum DataType
	{
		/// <summary>No data type.</summary>
		None = 0,

		/// <summary>The string data type (System.String).</summary>
		String,

		/// <summary>The integer data type (System.Int64).</summary>
		Integer,

		/// <summary>The real data type (System.Decimal).</summary>
		Real,

		/// <summary>The logic data type (System.Boolean).</summary>
		Logic,

		/// <summary>The polarity data type (TC.Magneto.Polarity).</summary>
		Polarity,

		/// <summary>The magnetic data type (TC.Magneto.Magnetic).</summary>
		Magnetic,

		/// <summary>The curl data type (TC.Magneto.Curl).</summary>
		Curl,

		/// <summary>The circuit data type (TC.Magneto.Circuit).</summary>
		Circuit
	}
}
