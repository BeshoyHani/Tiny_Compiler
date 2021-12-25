using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny_Compiler
{
    public static class TinyCompiler
    {
        public static Scanner tiny_Scanner = new Scanner();
        public static Parser tiny_Parser = new Parser();
        public static List<Token> TokenStream = new List<Token>();

        public static Node treeroot;


        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner
            tiny_Scanner.StartScanning(SourceCode);

            //Parser
            tiny_Parser.StartParsing(TokenStream);
            treeroot = tiny_Parser.root;

            //Sematic Analysis
        }
    }
}
