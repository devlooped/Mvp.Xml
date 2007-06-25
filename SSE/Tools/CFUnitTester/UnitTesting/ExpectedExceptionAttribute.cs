using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.Mobile.TestTools.UnitTesting
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class ExpectedExceptionAttribute : Attribute
	{
		private Type exceptionType;
		private string message;

		public ExpectedExceptionAttribute(Type exceptionType)
		{
			this.exceptionType = exceptionType;
		}

		public ExpectedExceptionAttribute(Type exceptionType, string message)
		{
			this.exceptionType = exceptionType;
			this.message = message;
		}

		public Type ExceptionType
		{
			get { return exceptionType; }
		}

		public string Message
		{
			get 
			{
				if (message == null)
				{
					return "ExpectedException fails: " + exceptionType.ToString();
				}
				else
					return message; 
			}
		}
	}
}
