using System;

namespace Microsoft.Practices.Mobile.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TestClassAttribute : Attribute
    {
        public TestClassAttribute()
        {
            
        }
    }
}
