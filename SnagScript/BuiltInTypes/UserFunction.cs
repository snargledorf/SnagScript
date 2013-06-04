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
using System.Collections.Generic;
using SnagScript.ParserNodes;


namespace SnagScript.BuiltInTypes
{

    /**
     * A user declared function.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class UserFunction : Function
    {
        protected List<Parameter> arguments;
        protected Node body;

        private string name;
        public override string Name
        {
            get { return name; }
        }

        public UserFunction(List<Parameter> arguments, Node body, String name)
        {
            this.arguments = arguments;
            this.body = body;
            this.name = name;

            // Reset the 'length' property since the Function constructor will have set it to 0
            this.SetProperty("length", new JavaScriptInteger(this.arguments.Count));
        }

        public Node Body
        {
            get { return body; }
        }

        public override int ArgumentCount
        {
            get { 
                if (arguments != null) // Need to check for null since the Function constructor accesses this property
                    return arguments.Count;
                return 0;
            }
        }

        public override String GetArgumentName(int index)
        {
            return arguments[index].Name;
        }

        public override JavaScriptObject GetDefaultValue(int index)
        {
            return arguments[index].GetDefaultValue();
        }

        protected override JavaScriptObject Execute(SourcePosition pos, Scope scope, JavaScriptObject thisObject)
        {
            try
            {
                return body.Evaluate(scope, thisObject);
            }
            catch (ReturnException e)
            {
                return e.Return;
            }
        }

        public UserFunction Clone()
        {
            return new UserFunction(this.arguments, this.body, this.Name);
        }
    }
}