using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnagScript.BuiltInTypes;

namespace SnagScript
{
    public class Scope
    {
        private Dictionary<String, JavaScriptObject> variablesAndFunctions = new Dictionary<String, JavaScriptObject>();
        private Scope parentscope;

        public Scope()
        {
            variablesAndFunctions = new Dictionary<string, JavaScriptObject>();
        }

        public Scope(Scope parent) : this()
        {
            this.parentscope = parent;
        }


        public void SetVariable(string name, JavaScriptObject value)
        {
            if (parentscope == null || this.HasVariable(name))
            {
                this.SetLocalVariable(name, value);
            }
            else
            {
                parentscope.SetVariable(name, value);
            }
        }

        public void SetLocalVariable(string name, JavaScriptObject value)
        {
            if (variablesAndFunctions.ContainsKey(name))
            {
                variablesAndFunctions[name] = value;
            }
            else
            {
                variablesAndFunctions.Add(name, value);
            }
        }

        public JavaScriptObject GetVariable(string functionName, SourcePosition sourcePosition)
        {
            if (variablesAndFunctions.ContainsKey(functionName))
            {
                return variablesAndFunctions[functionName];
            }
            else if (parentscope != null)
            {
                return parentscope.GetVariable(functionName, sourcePosition);
            }

            throw new UnsetVariableException(functionName, sourcePosition);
        }

        public bool HasVariable(string name)
        {
            return variablesAndFunctions.ContainsKey(name);
        }

        public Function GetFunction(string functionName, SourcePosition sourcePosition)
        {
            if (this.HasFunction(functionName))
            {
                return (Function)variablesAndFunctions[functionName];
            }
            else if (parentscope != null)
            {
                return parentscope.GetFunction(functionName, sourcePosition);
            }

            throw new UnsetFunctionException(functionName, sourcePosition);
        }

        public bool HasFunction(string functionName)
        {
            if (variablesAndFunctions.ContainsKey(functionName))
            {
                return variablesAndFunctions[functionName] is Function;
            }

            return false;
        }

        public void SetFunction(Function function)
        {
            this.SetLocalVariable(function.Name, function);
        }
    }
}
