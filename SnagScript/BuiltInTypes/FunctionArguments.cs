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
    class FunctionArguments : JavaScriptObject, IEnumerable<JavaScriptObject>
    {
        private List<JavaScriptObject> elements;

        public FunctionArguments(List<JavaScriptObject> elements)
        {
            this.elements = elements;
        }

        public JavaScriptObject this[int index]
        {
            get
            {
                return elements[index];
            }
            set
            {
                elements[index] = value;
            }
        }

        public int Count
        {
            get { return elements.Count; }
        }

        public void Add(JavaScriptObject element)
        {
            elements.Add(element);
        }

        public override int CompareTo(JavaScriptObject o)
        {
            throw new NotImplementedException();
        }

        #region IEnumerable implementation
        public System.Collections.IEnumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }
        #endregion

        #region IEnumerable implementation
        IEnumerator<JavaScriptObject> IEnumerable<JavaScriptObject>.GetEnumerator()
        {
            return elements.GetEnumerator();
        }
        #endregion
    }
}
