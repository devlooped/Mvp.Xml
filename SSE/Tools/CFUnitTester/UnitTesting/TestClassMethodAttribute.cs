using System;

namespace Microsoft.Practices.Mobile.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestMethodAttribute : Attribute
    {
        public TestMethodAttribute()
        {
        }
    }
}