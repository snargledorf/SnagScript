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
namespace SnagScript.BuiltInTypes
{

    /**
     *
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    sealed public class JavaScriptFloat : JavaScriptObject
    {
        private Double value;

        public JavaScriptFloat(String value) : this(Double.Parse(value))
        {

        }

        public JavaScriptFloat(Double value)
        {
            this.value = value;
        }

        public JavaScriptFloat Add(JavaScriptFloat augend)
        {
            return new JavaScriptFloat(value + augend.value);
        }

        public JavaScriptFloat Subtract(JavaScriptFloat subtrahend)
        {
            return new JavaScriptFloat(value - subtrahend.value);
        }

        public JavaScriptFloat Multiply(JavaScriptFloat multiplicand)
        {
            return new JavaScriptFloat(value * multiplicand.value);
        }

        public JavaScriptFloat Divide(JavaScriptFloat divisor)
        {
            return new JavaScriptFloat(value / divisor.value);
        }

        public JavaScriptFloat Remainder(JavaScriptFloat divisor)
        {
            return new JavaScriptFloat(value % divisor.value);
        }

        public JavaScriptFloat Power(JavaScriptFloat n)
        {
            return new JavaScriptFloat(Math.Pow(value, n.value));
        }

        public JavaScriptFloat Negate()
        {
            return new JavaScriptFloat(value * -1);
        }

        public int ToInt32()
        {
            return Convert.ToInt32(value);
        }

        public override int CompareTo(JavaScriptObject obj)
        {
            JavaScriptFloat number = (JavaScriptFloat)obj;
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
    }
}