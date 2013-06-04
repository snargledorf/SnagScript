using SnagScript.BuiltInTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnagScript.ParserNodes
{
    public class ObjectNode : Node
    {
        Node expression;

        public ObjectNode(SourcePosition pos, Node expression)
            : base(pos)
        {
            this.expression = expression;
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            JavaScriptObject newObject = new JavaScriptObject();
            expression.Evaluate(scope, newObject);
            return newObject;
        }
    }
}
