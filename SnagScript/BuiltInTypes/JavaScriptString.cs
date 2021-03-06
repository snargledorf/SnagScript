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
    sealed public class JavaScriptString : JavaScriptObject
    {
        private String value;

        public JavaScriptString(String value)
            : base()
        {
            this.value = value;
            this.SetProperty("length", new JavaScriptInteger(this.value.Length));
        }

        public JavaScriptString Concat(JavaScriptString other)
        {
            return new JavaScriptString(value + other.value);
        }

        public override String ToString()
        {
            return value;
        }

        public override int CompareTo(JavaScriptObject obj)
        {
            JavaScriptString str = (JavaScriptString)obj;
            return value.CompareTo(str.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override JavaScriptBoolean Equals(JavaScriptObject obj, Scope scope, JavaScriptObject thisObject)
        {
            return JavaScriptBoolean.ValueOf(this.CompareTo(obj) == 0);
        }
    }
}
