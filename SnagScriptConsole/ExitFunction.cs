using SnagScript.BuiltInTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnagScriptInterpreter
{
    class ExitFunction : Function
    {
        private JavaScriptInteger defaultExitCode = new JavaScriptInteger(0);

        public override string Name
        {
            get { return "exit"; }
        }

        public override int ArgumentCount
        {
            get { return 1; }
        }

        public override string GetArgumentName(int index)
        {
            return "exitCode";
        }

        public override JavaScriptObject GetDefaultValue(int index)
        {
            return defaultExitCode;
        }

        protected override JavaScriptObject Execute(SnagScript.SourcePosition pos, SnagScript.Scope scope, JavaScriptObject thisObject)
        {
            int exitCode = scope.GetVariable("exitCode", pos).ToInteger().Value;
            Environment.Exit(exitCode);
            return null;
        }
    }
}
