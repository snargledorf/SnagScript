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

namespace SnagScript.BuiltInTypes
{

    /**
     * A function callable by the interpreter.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public abstract class Function : JavaScriptObject
    {
        public Function()
        {
            this.SetProperty("call", this);
        }

        public abstract string Name { get; }

        /**
         * Get the number of parameters to this function.
         *
         * @return Number of parameters
         */
        abstract public int ArgumentCount { get; }

        /**
         * Get the name of the parameter
         *
         * @param index Parameter index
         * @return The name of the parameter
         */
        abstract public String GetArgumentName(int index);

        /**
         * Get the default value of the functions parameter.
         *
         * @param index Parameter index
         * @return The default value of the parameter. Return null if no default.
         */
        abstract public JavaScriptObject GetDefaultValue(int index);

        /*
         * Evaluate the function.
         *
         * @param interpreter
         * @param pos Source position of function call
         * @return The result of evaluating the function.
         */
        public JavaScriptObject Execute(Scope parentscope, List<JavaScriptObject> arguments, SourcePosition pos, JavaScriptObject thisObject)
        {
            Scope executionscope = new Scope(parentscope);
            int numberMissingArgs = 0;
            int numberRequiredArgs = 0;
            FunctionArguments argumentsVariable = new FunctionArguments(arguments);
            for (int paramIndex = 0; paramIndex < this.ArgumentCount; paramIndex++)
            {
                String argumentName = this.GetArgumentName(paramIndex);
                JavaScriptObject value = this.GetDefaultValue(paramIndex);
                if (value == null)
                {
                    numberRequiredArgs++;
                }
                if (paramIndex < argumentsVariable.Count)
                {
                    // Value provided in function call overrides the default value
                    value = argumentsVariable[paramIndex];
                }
                if (value == null)
                {
                    numberMissingArgs++;
                }

                executionscope.SetLocalVariable(argumentName, value);
            }

            executionscope.SetLocalVariable("arguments", argumentsVariable);

            if (numberMissingArgs > 0)
            {
                throw new TooFewArgumentsException(this.Name, numberRequiredArgs, argumentsVariable.Count, pos);
            }

            return this.Execute(pos, executionscope, thisObject);
        }

        protected abstract JavaScriptObject Execute(SourcePosition pos, Scope scope, JavaScriptObject thisObject);

        public override int CompareTo(JavaScriptObject o)
        {
            throw new NotImplementedException();
        }
    }
}