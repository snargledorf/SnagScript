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
using SnagScript.BuiltInTypes;
using System.Collections.Generic;
namespace SnagScript.ParserNodes
{

    /**
     * Assignment (=) operator. Assigns a value to a variable.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class AssignNode : BinaryOpNode
    {
        private bool isLocal = false;
        public AssignNode(SourcePosition pos, Node var, Node expression, bool isLocal)
            : base(pos, "set!", var, expression)
        {
            this.isLocal = isLocal;
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            Node left = this.LeftOperand;
            JavaScriptObject value = this.RightOperand.Evaluate(scope, thisObject);
            if (left is VariableNode)
            {
                String name = ((VariableNode)left).Name;
                if (isLocal)
                {
                    scope.SetLocalVariable(name, value);
                }
                else
                {
                    scope.SetVariable(name, value);
                }
            }
            else if (left is LookupNode)
            {
                ((LookupNode)left).Set(value, scope, thisObject);
            }
            else if (left is PropertyNode)
            {
                String name = ((PropertyNode)left).Name;
                thisObject.SetProperty(name, value);
            }
            else
            {
                throw new InvalidTypeException("Left hand of assignment must be a variable.", left.Position);
            }
            return value;
        }
    }
}