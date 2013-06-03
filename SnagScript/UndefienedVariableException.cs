using System;
namespace SnagScript
{

    /**
     * Variable has not been defiened.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class UndefinedVariableException : InterpreterException
    {
        private const long serialVersionUID = -3917677724212342759L;

        public UndefinedVariableException(String variableName)
            : base(variableName + " is not defined")
        {
        }
    }
}