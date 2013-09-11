using SnagScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnagScriptInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Interpreter interpreter = new Interpreter();

            interpreter.CustomFunctions.Add(new SquareFunction());
            interpreter.CustomFunctions.Add(new ExitFunction());

            String scriptPath = "script.js";
            if (args.Length > 0)
            {
                scriptPath = args[0];
            }

            interpreter.Execute(File.OpenRead(scriptPath));
        }
    }
}
