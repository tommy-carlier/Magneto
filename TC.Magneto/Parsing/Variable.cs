using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a variable.</summary>
	public class Variable
	{
		/// <summary>Initializes a new instance of the <see cref="Variable"/> class.</summary>
		/// <param name="id">An integer that identifies this variable.</param>
		/// <param name="name">The name of the variable.</param>
		/// <param name="dataType">The data type of the variable.</param>
		internal Variable(int id, string name, DataType dataType)
		{
			this.id = id;
			this.name = name;
			this.dataType = dataType;
		}

		readonly int id;
		/// <summary>Gets the ID of the variable.</summary>
		/// <value>The ID of the variable.</value>
		public int ID { get { return id; } }

		readonly string name;
		/// <summary>Gets the name of the variable.</summary>
		/// <value>The name of the variable.</value>
		public string Name { get { return name; } }

		readonly DataType dataType;
		/// <summary>Gets the data type of the variable.</summary>
		/// <value>The data type of the variable.</value>
		public DataType DataType { get { return dataType; } }

		/// <summary>Returns a <see cref="T:"/> that represents the current <see cref="T:Variable"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:Variable"/>.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} : {1} ({2})", name, dataType, id);
		}
	}
}
