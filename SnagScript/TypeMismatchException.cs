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


namespace SnagScript
{

    /**
     * Types don't match.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class TypeMismatchException : InterpreterException
    {
        private const long serialVersionUID = 9115378805326306069L;

        static private String ToString(Type type)
        {
            if (type == typeof(JavaScriptArray))
            {
                return "array";
            }
            else if (type == typeof(JavaScriptBoolean))
            {
                return "boolean";
            }
            else if (type == typeof(JavaScriptFloat))
            {
                return "float";
            }
            else if (type == typeof(JavaScriptString))
            {
                return "string";
            }
            else if (type == typeof(JavaScriptInteger))
            {
                return "integer";
            }
            else
            {
                return type.Name;
            }
        }

        public TypeMismatchException(SourcePosition pos, Type expect, Type actual)
            : base("Type mismatch - Excepted type '" + ToString(expect) + "' but got type '" + ToString(actual) + "'", pos)
        {
        }
    }
}