using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.Mobile.TestTools.UnitTesting
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class TestCleanupAttribute : Attribute
	{
	}
}
