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
     * if control structure.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class IfNode : Node
    {
        private Node testCondition;
        private Node thenBlock;
        private Node elseBlock;

        public IfNode(SourcePosition pos, Node testCondition, Node thenBlock, Node elseBlock)
            : base(pos)
        {
            this.testCondition = testCondition;
            this.thenBlock = thenBlock;
            this.elseBlock = elseBlock;
        }

        public Node TestCondition
        {
            get { return testCondition; }
        }

        public Node ThenBlock
        {
            get { return thenBlock; }
        }

        public Node ElseBlock
        {
            get { return elseBlock; }
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            sb.Append("if ");
            sb.Append(testCondition);
            sb.Append(' ');
            sb.Append(thenBlock);
            if (elseBlock != null)
            {
                sb.Append(' ');
                sb.Append(elseBlock);
            }
            sb.Append(')');
            return sb.ToString();
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            bool test = testCondition.Evaluate(scope,thisObject).ToBoolean().Value;
            if (test)
            {
                return thenBlock.Evaluate(scope,thisObject);
            }
            else if (elseBlock != null)
            {
                return elseBlock.Evaluate(scope,thisObject);
            }
            return JavaScriptBoolean.FALSE;
        }
    }
}