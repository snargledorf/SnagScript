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
using System.Collections.Generic;
using System.Text;
namespace SnagScript.BuiltInTypes
{

    /**
     *
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class JavaScriptObject : IComparable<JavaScriptObject>
    {
        private Dictionary<String, JavaScriptProperty> properties = new Dictionary<string, JavaScriptProperty>();

        public JavaScriptObject()
        {
            // Set any common properties/functions
        }

        public JavaScriptObject GetProperty(string name, SourcePosition sourcePosition)
        {
            if (properties.ContainsKey(name))
            {
                return properties[name].value;
            }

            throw new UnsetPropertyException(name, sourcePosition);
        }

        public void SetProperty(string name, JavaScriptObject value)
        {
            if (properties.ContainsKey(name))
            {
                properties[name].value = value;
            }
            else
            {
                properties.Add(name, new JavaScriptProperty(value));
            }
        }

        public Function GetFunction(string functionName, SourcePosition sourcePosition)
        {
            if (this.HasFunction(functionName))
            {
                return (Function)properties[functionName].value;
            }

            throw new UnsetFunctionException(functionName, sourcePosition);
        }

        public bool HasFunction(string functionName)
        {
            if (properties.ContainsKey(functionName))
            {
                return properties[functionName].value is Function;
            }

            return false;
        }

        public void SetFunction(Function function)
        {
            this.SetProperty(function.Name, function);
        }

        public JavaScriptFloat ToFloat()
        {
            if (this is JavaScriptFloat)
            {
                return (JavaScriptFloat)this;
            }
            else if (this is JavaScriptInteger)
            {
                return new JavaScriptFloat(((JavaScriptInteger)this).Value);
            }
            else
            {
                return new JavaScriptFloat(this.ToString());
            }
        }

        public JavaScriptInteger ToInteger()
        {
            if (this is JavaScriptInteger)
            {
                return (JavaScriptInteger)this;
            }
            else if (this is JavaScriptFloat)
            {
                return new JavaScriptInteger(((JavaScriptFloat)this).ToInt32());
            }
            else
            {
                return new JavaScriptInteger(this.ToString());
            }
        }

        public JavaScriptBoolean ToBoolean()
        {
            if (this is JavaScriptBoolean)
            {
                return (JavaScriptBoolean)this;
            }
            else
            {
                return JavaScriptBoolean.ValueOf(this.ToString());
            }
        }

        public virtual JavaScriptString ToJavaScriptString()
        {
            if (this is JavaScriptString)
                return (JavaScriptString)this;

            // Implicit converting of types to string
            return new JavaScriptString(this.ToString());
        }

        public virtual int CompareTo(JavaScriptObject o) { throw new NotImplementedException(); }

        public virtual JavaScriptBoolean Equals(JavaScriptObject obj, Scope scope, JavaScriptObject thisObject) 
        {
            return JavaScriptBoolean.ValueOf(this == obj);
        }
    }
}