/*
 * Copyright (c) 2013 Ryan Esteves
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
using System.Linq;
using System.Text;
using SnagScript.BuiltInTypes;

namespace SnagScript.ParserNodes
{
    public class FunctionDeclarationNode : Node
    {
        readonly static public List<Node> NO_PARAMETERS = new List<Node>(0);

        protected List<Node> parameters;
        protected Node body;
        public String Name { get; private set; }

        public FunctionDeclarationNode(SourcePosition pos, List<Node> parameters, String functionName, Node body)
            : base(pos)
        {
            this.Name = functionName;
            this.parameters = parameters;
            this.body = body;
        }
        
        public override JavaScriptObject Evaluate(Scope scope, BuiltInTypes.JavaScriptObject thisObject)
        {
            List<Parameter> resultParameters = ProcessParameters(scope, thisObject);
            UserFunction function = new UserFunction(resultParameters, body, this.Name);
            scope.SetFunction(function);
            return function;
        }

        protected List<Parameter> ProcessParameters(Scope scope, JavaScriptObject thisObject)
        {
            List<Parameter> resultParameters = new List<Parameter>(this.parameters.Count);
            foreach (Node node in parameters)
            {
                // TODO clean up getting parameters
                String parameterName;
                JavaScriptObject parameterValue;
                if (node is VariableNode)
                {
                    parameterName = ((VariableNode)node).Name;
                    parameterValue = null;
                }
                else if (node is AssignNode)
                {
                    parameterName = ((VariableNode)((AssignNode)node).LeftOperand).Name;
                    parameterValue = ((AssignNode)node).RightOperand.Evaluate(scope, thisObject);
                }
                else
                {
                    // This error should not occur
                    throw new Exception("Invalid function");
                }
                Parameter param = new Parameter(parameterName, parameterValue);
                resultParameters.Add(param);
            }
            return resultParameters;
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(function (");
            bool first = true;
            foreach (Node node in parameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(' ');
                }
                sb.Append(node);
            }
            sb.Append(") ");
            sb.Append(body);
            sb.Append(')');
            return sb.ToString();
        }
    }
}
