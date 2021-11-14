using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny_Compiler
{
    public static class TinyCompiler
    {
        public static Scanner tiny_Scanner = new Scanner();


        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner
            tiny_Scanner.StartScanning(SourceCode);

            //Parser
            //Sematic Analysis
        }
    }
}
