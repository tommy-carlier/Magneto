using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto
{
	static class DataTypes
	{
		static readonly IDictionary<Type, DataType> 
			typeToDataType = new Dictionary<Type, DataType>
            {
                [typeof(string)] = DataType.String,
                [typeof(long)] = DataType.Integer,
                [typeof(decimal)] = DataType.Real,
                [typeof(bool)] = DataType.Logic,
                [typeof(Polarity)] = DataType.Polarity,
                [typeof(Magnetic)] = DataType.Magnetic,
                [typeof(Curl)] = DataType.Curl,
                [typeof(Circuit)] = DataType.Circuit
            };

		/// <summary>Converts the specified <see cref="T:Type"/> to a <see cref="T:DataType"/>.</summary>
		/// <param name="type">The <see cref="T:Type"/> to convert.</param>
		/// <returns>The converted <see cref="T:DataType"/>.</returns>
		public static DataType GetDataType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
            return typeToDataType.TryGetValue(type, out DataType dataType) ? dataType : DataType.None;
        }

		/// <summary>Converts the specified <see cref="T:DataType"/> to a <see cref="T:Type"/>.</summary>
		/// <param name="dataType">The <see cref="T:DataType"/> to convert.</param>
		/// <returns>The converted <see cref="T:Type"/>.</returns>
		public static Type GetType(DataType dataType)
		{
			switch (dataType)
			{
				case DataType.String: return typeof(string);
				case DataType.Integer: return typeof(long);
				case DataType.Real: return typeof(decimal);
				case DataType.Logic: return typeof(bool);
				case DataType.Polarity: return typeof(Polarity);
				case DataType.Magnetic: return typeof(Magnetic);
				case DataType.Curl: return typeof(Curl);
				case DataType.Circuit: return typeof(Circuit);
				default: throw new ArgumentException("Invalid DataType", "dataType");
			}
		}
	}
}
