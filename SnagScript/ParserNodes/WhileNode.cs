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
using System.Text;
using SnagScript.BuiltInTypes;
using System.Collections.Generic;


namespace SnagScript.ParserNodes
{

    /**
     * while loop control structure.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class WhileNode : Node
    {
        private Node testCondition;
        private Node loopBody;

        public WhileNode(SourcePosition pos, Node testCondition, Node loopBody)
            : base(pos)
        {
            this.testCondition = testCondition;
            this.loopBody = loopBody;
        }

        public Node GetTestCondition()
        {
            return testCondition;
        }

        public Node GetLoopBody()
        {
            return loopBody;
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            sb.Append("while ");
            sb.Append(testCondition);
            sb.Append(' ');
            sb.Append(loopBody);
            sb.Append(')');
            return sb.ToString();
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            JavaScriptObject ret = null;
            while (testCondition.Evaluate(scope, thisObject).ToBoolean(testCondition.Position).BooleanValue)
            {
                ret = loopBody.Evaluate(scope, thisObject);
            }
            return ret;
        }
    }
}