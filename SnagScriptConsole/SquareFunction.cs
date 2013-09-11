using SnagScript;
using SnagScript.BuiltInTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnagScriptInterpreter
{
    public class SquareFunction : Function
    {
        public override string Name
        {
            get { return "square"; }
        }

        public override int ArgumentCount
        {
            get { return 1; }
        }

        public override string GetArgumentName(int index)
        {
            return "number";
        }

        public override SnagScript.BuiltInTypes.JavaScriptObject GetDefaultValue(int index)
        {
            return null;
        }

        protected override JavaScriptObject Execute(SourcePosition pos, Scope scope, JavaScriptObject thisObject)
        {
            JavaScriptInteger integer = scope.GetVariable("number", pos).ToInteger();
            return new JavaScriptInteger(integer.Value * integer.Value);
        }
    }
}
