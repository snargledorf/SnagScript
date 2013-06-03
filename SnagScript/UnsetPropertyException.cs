using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnagScript
{
    class UnsetPropertyException : InterpreterException
    {
        public UnsetPropertyException(string propertyName, SourcePosition pos)
            : base(propertyName + " is not set", pos)
        {

        }
    }
}
