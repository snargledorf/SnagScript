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
using SnagScript.BuiltInTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnagScript.ParserNodes
{
    public class TryCatchFinallyNode : Node
    {
        private BlockNode tryBlock;
        private CatchNode catchNode;
        private BlockNode finallyBlock;

        public TryCatchFinallyNode(SourcePosition pos, BlockNode tryBlock, CatchNode catchNode, BlockNode finallyBlock)
            : base(pos)
        {
            this.tryBlock = tryBlock;
            this.catchNode = catchNode;
            this.finallyBlock = finallyBlock;
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            JavaScriptObject retValue = null;
            try
            {
                retValue = this.tryBlock.Evaluate(scope, thisObject);
            }
            catch (Exception ex)
            {
                if (catchNode != null)
                {
                    Scope catchScope = new Scope(scope);
                    catchNode.SetException(ex, catchScope);
                    catchNode.Evaluate(catchScope, thisObject);
                }
            }
            finally
            {
                if (finallyBlock != null)
                {
                    finallyBlock.Evaluate(scope, thisObject);
                }
            }

            return retValue;
        }
    }
}
