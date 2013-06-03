using SnagScript.BuiltInTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnagScript.ParserNodes
{
    public class ForLoopNode : Node
    {
        private Node initialization;
        private Node condition;
        private Node final;
        private Node loopBlock;

        public ForLoopNode(SourcePosition pos, Node initialization, Node condition, Node final, Node loopBlock)
            : base(pos)
        {
            this.initialization = initialization;
            this.condition = condition;
            this.final = final;
            this.loopBlock = loopBlock;
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            if (initialization != null)
            {
                initialization.Evaluate(scope, thisObject);
            }

            while (condition.Evaluate(scope, thisObject).ToBoolean().Value)
            {
                loopBlock.Evaluate(scope, thisObject);

                if (final != null)
                {
                    final.Evaluate(scope, thisObject);
                }
            }

            return null;
        }
    }
}
