using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.Mobile.TestTools.UnitTesting
{
	public class AssertException : Exception
	{
		public AssertException(string message) : base(message)
		{
		}
	}
}
