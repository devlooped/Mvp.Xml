using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
using System.Diagnostics;

namespace Microsoft.Practices.Mobile.TestTools.TestRunner
{
	public class TestClassRunner
	{
		private object testClass;
		private bool onDesktop = false;
		private TestClassInfo testClassInfo;

		public TestClassRunner(TestClassInfo classInfo)
		{
			testClassInfo = classInfo;
			testClass = Activator.CreateInstance(classInfo.ClassType);
		}

		public bool OnDesktop
		{
			set { onDesktop = value; }
		}

		public string RunMethod(TestMethodInfo testMethodInfo)
		{
			bool startedMethod = false;

			try
			{
				if (testClassInfo.TestInitialize != null)
					testClassInfo.TestInitialize.Invoke(testClass, null);

				startedMethod = true;
				testMethodInfo.Invoke(testClass);

				if (testMethodInfo.ExpectedException != null)
					return testMethodInfo.ExpectedExceptionMessage;
				else
					return null;
			}
			catch (Exception ex)
			{
				//
				// Exceptions are reported differently on the Pocket PC than on the desktop.
				// As a result, this code will run correct only on the Pocket PC.
				//
				if (onDesktop)
					ex = ex.InnerException;

				if (ex is AssertException)
				{
					Debug.WriteLine(ex);
					return ex.Message;
				}
				else
				{
					if (testMethodInfo.ExpectedException == ex.GetType() && startedMethod)
						return null;

					Debug.WriteLine(ex);
					StringBuilder message = new StringBuilder();
					message.Append(ex.GetType().FullName);
					message.Append(": ");
					message.Append(ex.Message);
					return message.ToString();
				}
			}
			finally
			{
				try
				{
					if (testClassInfo.TestCleanup != null)
						testClassInfo.TestCleanup.Invoke(testClass, null);
				}
				catch {}
			}
		}
	}
}
