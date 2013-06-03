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
using SnagScript.BuiltInTypes;
using System;
using System.Text;
using System.Collections.Generic;


namespace SnagScript.ParserNodes
{

    /**
     * foreach control structure.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class LookupNode : Node
    {
        private VariableNode variableNode;
        private Node keyNode;

        public LookupNode(SourcePosition pos, VariableNode variableNode, Node keyNode)
            : base(pos)
        {
            this.variableNode = variableNode;
            this.keyNode = keyNode;
        }

        public JavaScriptObject Get(Scope scope, JavaScriptObject thisObject)
        {
            JavaScriptObject variable = scope.GetVariable(variableNode.Name, variableNode.Position);
            if (variable is JavaScriptArray)
            {
                int index = GetIndex(scope, thisObject);
                return ((JavaScriptArray)variable)[index];
            }
            throw new InvalidTypeException("lookup expects an array.", Position);
        }

        public void Set(JavaScriptObject result, Scope scope, JavaScriptObject thisObject)
        {
            JavaScriptObject variable = scope.GetVariable(variableNode.Name, variableNode.Position);

            if (variable is JavaScriptArray)
            {
                int index = GetIndex(scope, thisObject);
                ((JavaScriptArray)variable)[index] = result;
                return;
            }
            throw new InvalidTypeException("lookup expects an array.", Position);
        }

        private int GetIndex(Scope scope, JavaScriptObject thisObject)
        {
            return keyNode.Evaluate(scope, thisObject).ToInteger().Value;
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            sb.Append("lookup ");
            sb.Append(variableNode);
            sb.Append(' ');
            sb.Append(keyNode);
            sb.Append(')');
            return sb.ToString();
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            return Get(scope, thisObject);
        }
    }
}