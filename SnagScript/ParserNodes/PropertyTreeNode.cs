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
using SnagScript.BuiltInTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnagScript.ParserNodes
{
    public class PropertyTreeNode : Node
    {
        private Node parent;
        public Node next { get; private set; }

        public PropertyTreeNode(SourcePosition pos, Node parent, Node next)
            : base(pos)
        {
            this.parent = parent;
            this.next = next;
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            if (this.parent is VariableNode)
            {
                VariableNode varNode = ((VariableNode)parent);
                return next.Evaluate(scope, scope.GetVariable(varNode.Name, varNode.Position));
            }
            else if (this.parent is PropertyNode)
            {
                PropertyNode propNode = ((PropertyNode)parent);
                return next.Evaluate(scope, thisObject.GetProperty(propNode.Name, propNode.Position));
            }

            throw new InvalidTypeException("Expected variable or property", this.Position);
        }
    }
}
