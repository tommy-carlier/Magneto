using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace TC.Magneto
{
	/// <summary>Provides utilities that deal with reflection.</summary>
	public static class ReflectionUtilities
	{
        /// <summary>Gets the first attribute of the specified type defined on the specified provider.</summary>
        /// <typeparam name="TAttribute">The type of the attribute to get.</typeparam>
        /// <param name="provider">The <see cref="T:ICustomAttributeProvider"/> to investigate.</param>
        /// <param name="inherit">if set to <c>true</c>, look up the hierarchy chain for the inherited attribute.</param>
        /// <returns>The first attribute of the specified type, or null if not found.</returns>
        public static TAttribute GetFirstAttribute<TAttribute>(
			this ICustomAttributeProvider provider, 
			bool inherit) where TAttribute : Attribute
		{
			if (provider == null) throw new ArgumentNullException("provider");

			object[] attributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);
			return
				attributes.Length > 0
					? attributes[0] as TAttribute
					: null;
		}

        /// <summary>Gets the attributes of the specified type defined on the specified provider.</summary>
        /// <typeparam name="TAttribute">The type of the attributes to get.</typeparam>
        /// <param name="provider">The <see cref="T:ICustomAttributeProvider"/> to investigate.</param>
        /// <param name="inherit">if set to <c>true</c>, look up the hierarchy chain for the inherited attribute.</param>
        /// <returns>The collection of attributes of the specified type.</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(
			this ICustomAttributeProvider provider, 
			bool inherit) where TAttribute : Attribute
		{
			if (provider == null) throw new ArgumentNullException("provider");

			return GetAttributesCore<TAttribute>(provider, inherit);
		}

		private static IEnumerable<TAttribute> GetAttributesCore<TAttribute>(
			ICustomAttributeProvider provider,
			bool inherit) where TAttribute : Attribute
		{
            object[] attributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);
            if (attributes != null)
				for (int i = 0; i < attributes.Length; i++)
					if (attributes[i] is TAttribute attribute)
						yield return attribute;
		}
	}
}
