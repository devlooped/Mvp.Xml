using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Practices.Mobile.TestTools.UnitTesting;

namespace Microsoft.Practices.Mobile.TestTools.TestRunner
{
	public class TestAssemblyInfo
	{
		private Assembly assembly;

		public TestAssemblyInfo(string assemblyFileName)
		{
			assembly = Assembly.LoadFrom(assemblyFileName);

		}

		public TestClassInfo[] GetTestClasses()
		{
			SortedList<TestClassInfo, object> classNames = new SortedList<TestClassInfo, object>(new TestClassInfoComparer());

			Type[] types = assembly.GetTypes();
			foreach (Type type in assembly.GetTypes())
			{
				if (!type.IsClass)
					continue;

				if (Attribute.IsDefined(type, typeof(TestClassAttribute), true))
				{
					TestClassInfo info = new TestClassInfo(type);
					classNames.Add(info, null);
				}
			}

			return new List<TestClassInfo>(classNames.Keys).ToArray();
		}

		class TestClassInfoComparer : IComparer<TestClassInfo>
		{
			#region IComparer<TestClassInfo> Members

			public int Compare(TestClassInfo x, TestClassInfo y)
			{
				return x.FullName.CompareTo(y.FullName);
			}

			#endregion
		}
	}
}
