/*
 * Copyright (c) 2008 Cameron Zemek
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using SnagScript.BuiltInTypes;
using System.Text;


namespace SnagScript.ParserNodes
{

    /**
     * Call to function.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class FunctionCallNode : Node
    {
        readonly static public List<Node> NO_ARGUMENTS = new List<Node>(0);

        protected String functionName;
        protected List<Node> arguments;

        public FunctionCallNode(SourcePosition pos, String functionName, List<Node> arguments)
            : base(pos)
        {
            this.functionName = functionName;
            this.arguments = arguments;
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            Function function;
            if (thisObject.HasFunction(functionName))
            {
                function = thisObject.GetFunction(functionName, this.Position);
            }
            else
            {                
                function = scope.GetFunction(functionName, this.Position);
            }

            return this.CallFunction(scope, function, thisObject);
        }

        protected JavaScriptObject CallFunction(Scope scope, Function function, JavaScriptObject thisObject)
        {
            // Evaluate the arguments
            List<JavaScriptObject> args = new List<JavaScriptObject>(arguments.Count);
            foreach (Node node in arguments)
            {
                args.Add(node.Evaluate(scope, thisObject));
            }           

            return function.Execute(scope, args, this.Position, thisObject);
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            sb.Append(functionName);
            foreach (Node arg in arguments)
            {
                sb.Append(' ');
                sb.Append(arg);
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}