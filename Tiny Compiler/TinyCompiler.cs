﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny_Compiler
{
    public static class TinyCompiler
    {
        public static Scanner Jason_Scanner = new Scanner();


        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner
            Jason_Scanner.StartScanning(SourceCode);

            //Parser
            //Sematic Analysis
        }
    }
}