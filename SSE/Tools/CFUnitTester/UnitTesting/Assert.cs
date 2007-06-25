using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.Mobile.TestTools.UnitTesting
{
	public sealed class Assert
	{
        public Assert()
        {
                
        }

		public static void Fail()
		{
			throw new AssertException(Properties.Resources.AssertFail);
		}

		public static void Fail(string message)
		{
			throw new AssertException(Properties.Resources.AssertFail + ": " + message);
		}

		public static void IsTrue(bool condition)
		{
			if (!condition)
				throw new AssertException(Properties.Resources.AssertIsTrue);
		}

		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
				throw new AssertException(Properties.Resources.AssertIsTrue + " " + message);
		}

		public static void IsFalse(bool condition)
		{
			if (condition)
				throw new AssertException(Properties.Resources.AssertIsFalse);
		}

		public static void IsFalse(bool condition, string message)
		{
			if (condition)
				throw new AssertException(Properties.Resources.AssertIsFalse + " " + message);
		}

		public static void IsNull(object value)
		{
			if (value != null)
				throw new AssertException(Properties.Resources.AssertIsNull);
		}

		public static void IsNull(object value, string message)
		{
			if (value != null)
				throw new AssertException(Properties.Resources.AssertIsNull + ": " + message);
		}

		public static void IsNotNull(object value)
		{
			if (value == null)
				throw new AssertException(Properties.Resources.AssertIsNotNull);
		}

		public static void IsNotNull(object value, string message)
		{
			if (value == null)
				throw new AssertException(Properties.Resources.AssertIsNotNull + " " + message);
		}

		public static void AreEqual(object expected, object actual)
		{
			AreEqual(expected, actual, null);
		}

		public static void AreEqual<T>(T expected, T actual)
		{
			AreEqual(expected, actual, null);
		}

		public static void AreEqual(object expected, object actual, string message)
		{
			if (expected == null && actual == null)
				return;
			else if ((expected == null || actual == null) || (!Object.Equals(expected, actual)))
			{
				string msg = Properties.Resources.AssertAreEqual;
				string first = expected == null ? "(null)" : expected.ToString();
				string second = actual == null ? "(null)" : actual.ToString();
				msg = String.Format(msg, first, second);
				if (message != null)
					msg += message;

				throw new AssertException(msg);
			}
		}

		public static void AreNotEqual(object notExpected, object actual)
		{
			AreNotEqual(notExpected, actual, null);
		}

		public static void AreNotEqual(object notExpected, object actual, string message)
		{
			if ((notExpected == null && actual != null) || (notExpected != null && actual == null))
				return;
			if ((notExpected == null || actual == null) || (Object.Equals(notExpected, actual)))
			{
				string msg = Properties.Resources.AssertAreNotEqual;
				string first = notExpected == null ? "(null)" : notExpected.ToString();
				string second = actual == null ? "(null)" : actual.ToString();
				msg = String.Format(msg, first, second);
				if (message != null)
					msg += message;

				throw new AssertException(msg);
			}
		}

		public static void AreSame(object expected, object actual)
		{
			if (!Object.ReferenceEquals(expected, actual))
				throw new AssertException(Properties.Resources.AssertAreSame);
		}

		public static void AreSame(object expected, object actual, string message)
		{
			if (!Object.ReferenceEquals(expected, actual))
				throw new AssertException(Properties.Resources.AssertAreSame + " " + message);
		}

		public static void AreNotSame(object notExpected, object actual)
		{
			if (Object.ReferenceEquals(notExpected, actual))
				throw new AssertException(Properties.Resources.AssertAreNotSame);
		}

		public static void IsInstanceOfType(object section, Type type)
		{
			if (!type.IsInstanceOfType(section))
				throw new AssertException(Properties.Resources.IsInstanceOfType);
		}
	}
}
