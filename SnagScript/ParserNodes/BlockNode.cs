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
using System.Text;
using SnagScript.BuiltInTypes;


namespace SnagScript.ParserNodes
{


    /**
     * A block is a group of statements.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class BlockNode : Node
    {
        private List<Node> statements;

        public BlockNode(SourcePosition pos, List<Node> statements)
            : base(pos)
        {
            this.statements = statements;
        }

        public Node this[int index]
        {
            get { return statements[index]; }
            set { statements[index] = value; }
        }

        protected List<Node> Statements
        {
            get { return statements; }
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            foreach (Node node in statements)
            {
                sb.Append(node.ToString());
            }
            sb.Append(')');
            return sb.ToString();
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            Scope localScope = new Scope(scope);
            JavaScriptObject ret = null;
            foreach (Node statement in statements)
            {
                ret = statement.Evaluate(localScope, thisObject);
            }
            return ret;
        }
    }
}