using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Mvp.Xml.ObjectXPathNavigator.Test
{
	[TestFixture]
	public class AnonymousTest
	{
		[Test]
		public void SimpleAnonymous()
		{
			var obj = new
			          	{
			          		Hello = "Hello",
			          		World = "World"
			          	};
		}
	}
}