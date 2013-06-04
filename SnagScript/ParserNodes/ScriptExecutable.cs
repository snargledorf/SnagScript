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
using System.Collections.Generic;
using System;
using System.Text;
using SnagScript.BuiltInTypes;


namespace SnagScript.ParserNodes
{
    /**
     * The root of the abstract syntax tree.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class ScriptExecutable : JavaScriptObject
    {
        private List<Node> statements;

        public ScriptExecutable(List<Node> statements)
        {
            this.statements = statements;
        }

        public JavaScriptObject Run(Scope globalScope)
        {
            JavaScriptObject ret = null;

            foreach (Node statement in statements)
            {
                ret = statement.Evaluate(globalScope, this);
            }

            return ret;
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Node node in this.statements)
            {
                sb.Append(node.ToString());
            }
            return sb.ToString();
        }
    }
}