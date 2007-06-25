using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Microsoft.Practices.Mobile.TestTools.TestRunner
{
	public class TestMethodInfo
	{
		private MethodInfo methodInfo;
		private Type expectedException;
		private string expectedExceptionMessage;

		public TestMethodInfo(MethodInfo methodInfo, Type expectedException, string expectedExceptionMessage)
		{
			this.expectedException = expectedException;
			this.expectedExceptionMessage = expectedExceptionMessage;
			this.methodInfo = methodInfo;
		}

		public Type ExpectedException
		{
			get { return expectedException; }
		}

		public string ExpectedExceptionMessage
		{
			get { return expectedExceptionMessage; }
		}

		public void Invoke(object testClass)
		{
			methodInfo.Invoke(testClass, null);
		}

		public string Name
		{
			get { return methodInfo.Name; }
		}
	}
}
