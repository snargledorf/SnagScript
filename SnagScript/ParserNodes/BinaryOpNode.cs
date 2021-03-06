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
using SnagScript;
using System.Text;


namespace SnagScript.ParserNodes
{

    /**
     * Base class for binary operators.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public abstract class BinaryOpNode : Node
    {
        protected String oper;
        protected Node left;
        protected Node right;

        /**
         * @param left  Left operand
         * @param right Right operand
         */
        protected BinaryOpNode(SourcePosition pos, String oper, Node left, Node right)
            : base(pos)
        {
            this.oper = oper;
            this.left = left;
            this.right = right;
        }

        /**
         * Return operator symbol
         */
        public String GetName()
        {
            return oper;
        }

        /**
         * Get left operand
         */
        public Node LeftOperand
        {
            get { return left; }
        }

        /**
         * Get right operand
         */
        public Node RightOperand
        {
            get { return right; }
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            sb.Append(GetName());
            sb.Append(' ');
            sb.Append(left.ToString());
            sb.Append(' ');
            sb.Append(right.ToString());
            sb.Append(')');
            return sb.ToString();
        }
    }
}