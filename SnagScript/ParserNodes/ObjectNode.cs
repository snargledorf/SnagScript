using SnagScript.BuiltInTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnagScript.ParserNodes
{
    public class ObjectNode : Node
    {
        private Dictionary<String, Node> properties = new Dictionary<String, Node>();

        public void SetProperty(String name, Node property)
        {
            if (properties.ContainsKey(name))
            {
                properties[name] = property;
            }
            else
            {
                properties.Add(name, property);
            }
        }

        public Node GetProperty(string name)
        {
            return properties[name];
        }

        public ObjectNode(SourcePosition pos)
            : base(pos)
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            foreach (KeyValuePair<String, Node> obj in this.properties)
            {
                sb.AppendFormat("[Name: {0}, Value: {1}]", obj.Key, obj.Value.ToString());
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override JavaScriptObject Evaluate(Scope scope, JavaScriptObject thisObject)
        {
            JavaScriptObject genericObject = new JavaScriptObject();
            foreach (KeyValuePair<String, Node> property in this.properties)
            {
                genericObject.SetProperty(property.Key, property.Value.Evaluate(scope, thisObject));
            }
            return genericObject;
        }
    }
}
