using System;
using System.Collections.Generic;
using System.Text;

namespace coffee_kiosk_solution.Data.Attributes
{
    public class CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class SkipAttribute : Attribute
        {
        }
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class ContainAttribute : Attribute
        {
        }
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class StringAttribute : System.Attribute
        {
        }
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class GuidAttribute : System.Attribute
        {
        }
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class SpecificAttribute : System.Attribute
        {
        }
    }
}
