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

namespace SnagScript.BuiltInTypes
{
    public sealed class JavaScriptInteger : JavaScriptObject
    {
        private Int64 value;

        public JavaScriptInteger(String value)
            : this(Int64.Parse(value))
        {

        }

        public JavaScriptInteger(Int64 value)
        {
            this.value = value;
        }

        public JavaScriptInteger Add(JavaScriptInteger augend)
        {
            return new JavaScriptInteger(value + augend.value);
        }

        public JavaScriptInteger Subtract(JavaScriptInteger subtrahend)
        {
            return new JavaScriptInteger(value - subtrahend.value);
        }

        public JavaScriptInteger Multiply(JavaScriptInteger multiplicand)
        {
            return new JavaScriptInteger(value * multiplicand.value);
        }

        public JavaScriptInteger Divide(JavaScriptInteger divisor)
        {
            return new JavaScriptInteger(value / divisor.value);
        }

        public JavaScriptInteger Remainder(JavaScriptInteger divisor)
        {
            return new JavaScriptInteger(value % divisor.value);
        }

        public JavaScriptInteger Power(JavaScriptInteger n)
        {
            return new JavaScriptInteger(Convert.ToInt64(Math.Pow(value, n.value)));
        }

        public JavaScriptInteger Negate()
        {
            return new JavaScriptInteger(value * -1);
        }

        public override int CompareTo(JavaScriptObject obj)
        {
            JavaScriptInteger number = (JavaScriptInteger)obj;
            return value.CompareTo(number.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override String ToString()
        {
            return value.ToString();
        }

        public override bool Equals(Object obj)
        {
            return CompareTo((JavaScriptObject)obj) == 0;
        }

        public int ToInt32()
        {
            return Convert.ToInt32(this.value);
        }
    }
}
