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
namespace SnagScript.BuiltInTypes
{

    /**
     * print built-in function. Prints text to standard output.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class PrintFunction : Function
    {
        public override JavaScriptObject GetDefaultValue(int index)
        {
            return null;
        }

        public override int ArgumentCount
        {
            get { return 1; }
        }

        public override String GetArgumentName(int index)
        {
            return "string";
        }

        protected override JavaScriptObject Execute(SourcePosition pos, Scope scope, JavaScriptObject thisObject)
        {
            JavaScriptString str = scope.GetVariable("string", pos).ToJavaScriptString();
            Console.Write(str.ToString());

            return null;
        }

        public override string Name
        {
            get { return "print"; }
        }
    }
}