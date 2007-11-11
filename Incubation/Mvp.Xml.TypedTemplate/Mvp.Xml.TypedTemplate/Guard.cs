using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Template
{
	/// <summary>
	/// Common guard clauses.
	/// </summary>
	/// <exclude />
	public static class Guard
	{
		/// <summary>
		/// Checks an argument to ensure it isn't null.
		/// </summary>
		/// <param name="argumentValue">The argument value to check.</param>
		/// <param name="argumentName">The name of the argument.</param>
		public static void ArgumentNotNull(object argumentValue, string argumentName)
		{
			if (argumentValue == null)
				throw new ArgumentNullException(argumentName);
		}
	}
}
