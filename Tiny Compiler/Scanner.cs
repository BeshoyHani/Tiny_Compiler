using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public enum Token_Class
{
    Int, Float, String, Read, Write, Repeat, Until, If, Else, ElseIf, Then, Return, Endl,
    EndIf, EndUntil,
    Main, Parameters, Program,
    Dot, Semicolon, Comma, LCurlyParanthesis, RCurlyParanthesis, LRoundParanthesis, RRoundParanthesis,
    OpenComment, CloseComment,
    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant
}

namespace Tiny_Compiler
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
     
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();
        //Stack<char> bracket = new Stack<char>();
        public Scanner()
        {
            ReservedWords.Add("IF", Token_Class.If);
            ReservedWords.Add("ELSE", Token_Class.Else);
            ReservedWords.Add("ElseIf", Token_Class.ElseIf);
            ReservedWords.Add("ENDIF", Token_Class.EndIf);
            ReservedWords.Add("ENDUNTIL", Token_Class.EndUntil);
            ReservedWords.Add("PARAMETERS", Token_Class.Parameters);
            ReservedWords.Add("PROGRAM", Token_Class.Program);
            ReservedWords.Add("Main", Token_Class.Main);
            ReservedWords.Add("Return", Token_Class.Return);
            ReservedWords.Add("WRITE", Token_Class.Write);
            ReservedWords.Add("READ", Token_Class.Read);
            ReservedWords.Add("Endl", Token_Class.Endl);
            ReservedWords.Add("THEN", Token_Class.Then);
            ReservedWords.Add("UNTIL", Token_Class.Until);
            ReservedWords.Add("Int", Token_Class.Int);
            ReservedWords.Add("Float", Token_Class.Float);
            ReservedWords.Add("String", Token_Class.String);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LRoundParanthesis);
            Operators.Add(")", Token_Class.RRoundParanthesis);
            Operators.Add("{", Token_Class.LCurlyParanthesis);
            Operators.Add("}", Token_Class.RCurlyParanthesis);
            Operators.Add("/*", Token_Class.OpenComment);
            Operators.Add("*/", Token_Class.CloseComment);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("!", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);



        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (isWhiteSpace(CurrentChar))
                    continue;

                if (isLetter(CurrentChar))//if you read a Letter
                {
                    while( j+1 <SourceCode.Length &&  (isDigit(SourceCode[j+1]) || isLetter(SourceCode[j+1]) ) )
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                    }
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }

                else if (isDigit(CurrentChar))
                {
                    while (j+1 < SourceCode.Length && isDigit(SourceCode[j+1]))
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                    }
                    i = j;
                    FindTokenClass(CurrentLexeme);

                }

                else if(CurrentChar=='"')
                {
                    while (j + 1 < SourceCode.Length && SourceCode[j+1]!='\n')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];

                        if (SourceCode[j] == '"') break;
                    }

                    i = j;
                    FindTokenClass(CurrentLexeme);

                }

                else
                {
                    FindTokenClass(CurrentLexeme);
                }
            }

            //TinyCompiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {

            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;

            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
                Tok.token_type = ReservedWords[Lex];

            //Is it an identifier?
            else if (isIdentifier(Lex))
                Tok.token_type = Token_Class.Idenifier;

            //Is it a Constant?
            else if (isConstant(Lex))
                Tok.token_type = Token_Class.Constant;

            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
                Tok.token_type = Operators[Lex];

            //Is it a String?
            else if (isString(Lex))
                Tok.token_type = Token_Class.String;

            else
            {
                Errors.Error_List.Add(Lex + " is undefined");
                return;
            }
            //Is it an undefined?

            Tokens.Add(Tok);
        }



        bool isIdentifier(string lex){
            return new Regex("^[a-zA-Z]([a-zA-Z0-9])*$").IsMatch(lex);
        }

        bool isConstant(string lex){
            return new Regex("^[0-9]+([.][0-9]+)?$").IsMatch(lex);
        }

        bool isString(string lex){
            return new Regex("^[\"][^\"]+[\"]$").IsMatch(lex);
        }

        bool isWhiteSpace(char character){
            return character == ' ' || character == '\r' || character == '\n';
        }

        bool isLetter(char character){
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }

        bool isDigit(char character){
            return character >= '0' && character <= '9';
        }
    }
}
