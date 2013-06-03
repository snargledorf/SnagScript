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
    sealed public class JavaScriptBoolean : JavaScriptObject
    {
        static readonly public JavaScriptBoolean TRUE = new JavaScriptBoolean(true);
        static readonly public JavaScriptBoolean FALSE = new JavaScriptBoolean(false);

        private bool value;

        private JavaScriptBoolean(bool value)
        {
            this.value = value;
        }

        public bool BooleanValue
        {
            get
            {
                return this.value;
            }
        }

        static public JavaScriptBoolean ValueOf(bool value)
        {
            return value ? TRUE : FALSE;
        }

        public JavaScriptBoolean And(JavaScriptBoolean boolean)
        {
            return ValueOf(this.value && boolean.value);
        }

        public JavaScriptBoolean Or(JavaScriptBoolean boolean)
        {
            return ValueOf(this.value || boolean.value);
        }

        public JavaScriptBoolean Not()
        {
            return ValueOf(!this.value);
        }

        public override JavaScriptString ToJavaScriptString()
        {
            return new JavaScriptString(this.ToString());
        }

        public override String ToString()
        {
            return value.ToString();
        }

        public override int CompareTo(JavaScriptObject o)
        {
            throw new NotImplementedException();
        }
        public override int GetHashCode()
        {
            return 1;
        }
        public override bool Equals(Object obj)
        {
            return this == obj;
        }
    }
}