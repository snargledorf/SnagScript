using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnagScript.BuiltInTypes
{
    public class JavaScriptProperty : JavaScriptObject
    {
        public JavaScriptObject value;
        public bool configurable = false;
        public bool enumerable = false;

        public JavaScriptProperty(JavaScriptObject value)
        {
            this.value = value;
        }
    }
}
