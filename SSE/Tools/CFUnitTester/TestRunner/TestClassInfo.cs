using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Practices.Mobile.TestTools.UnitTesting;

namespace Microsoft.Practices.Mobile.TestTools.TestRunner
{
	public class TestClassInfo
	{
		private Type classType;
		private MethodInfo testInitialize;
		private TestMethodInfo[] testMethods;
		private MethodInfo testCleanup;

		public TestClassInfo(Type classType)
		{
			this.classType = classType;
			LoadClassInfo();
		}

		public Type ClassType
		{
			get { return classType; }
		}

		public string Name
		{
			get { return classType.Name; }
		}

		public string FullName
		{
			get { return classType.FullName; }
		}

		public MethodInfo TestCleanup
		{
			get { return testCleanup; }
		}
	

		public MethodInfo TestInitialize
		{
			get { return testInitialize; }
		}

		public TestMethodInfo[] TestMethods
		{
			get { return testMethods; }
		}
	

		private void LoadClassInfo()
		{
			MethodInfo[] methods = classType.GetMethods();

			SortedList<TestMethodInfo, object> results = new SortedList<TestMethodInfo, object>(new TestMethodInfoComparer());

			foreach (MethodInfo method in methods)
			{
				bool isTestMethod = Attribute.IsDefined(method, typeof(TestMethodAttribute), true);
				bool isIgnored = Attribute.IsDefined(method, typeof(IgnoreAttribute), true);
				bool isInitialize = Attribute.IsDefined(method, typeof(TestInitializeAttribute), true);
				bool isCleanup = Attribute.IsDefined(method, typeof(TestCleanupAttribute), true);

				Type expectedException = null;
				string expectedExceptionMessage = null;
				object[] attributes = method.GetCustomAttributes(typeof(ExpectedExceptionAttribute), true);
				if (attributes.Length != 0)
				{
					expectedException = ((ExpectedExceptionAttribute)attributes[0]).ExceptionType;
					expectedExceptionMessage = ((ExpectedExceptionAttribute)attributes[0]).Message;
				}

				if (isInitialize)
				{
					testInitialize = method;
				}
				else if (isCleanup)
				{
					testCleanup = method;
				}
				else if (isTestMethod && !isIgnored)
				{
					results.Add(new TestMethodInfo(method, expectedException, expectedExceptionMessage), null);
				}
			}

			this.testMethods = new List<TestMethodInfo>(results.Keys).ToArray();
		}

		class TestMethodInfoComparer : IComparer<TestMethodInfo>
		{
			#region IComparer<TestMethodInfo> Members

			public int Compare(TestMethodInfo x, TestMethodInfo y)
			{
				return x.Name.CompareTo(y.Name);
			}

			#endregion
		}
}
}
