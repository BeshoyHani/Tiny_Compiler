using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public enum Token_Class
{
    DataType_INT, DataType_Float, DataType_String, String,
    Read, Write,
    Return, Endl,
    IF, Else, ElseIf, EndIf, Then, Repeat, Until,
    Dot, Semicolon, Comma, LeftBraces, RightBraces, RightParentheses, LeftParentheses,
    Comment,
    Plus, Minus, Multiply, Division,
    AND, OR, Assign, Equal, LessThan, GreaterThan, NotEqual,
    Identifier, Number
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
            ReservedWords.Add("if", Token_Class.IF);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("elsef", Token_Class.ElseIf);
            ReservedWords.Add("end", Token_Class.EndIf);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("int", Token_Class.DataType_INT);
            ReservedWords.Add("float", Token_Class.DataType_Float);
            ReservedWords.Add("string", Token_Class.DataType_String);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LeftParentheses);
            Operators.Add(")", Token_Class.RightParentheses);
            Operators.Add("{", Token_Class.LeftBraces);
            Operators.Add("}", Token_Class.RightBraces);
            Operators.Add(":=", Token_Class.Assign);
            Operators.Add("=", Token_Class.Equal);
            Operators.Add("<", Token_Class.LessThan);
            Operators.Add(">", Token_Class.GreaterThan);
            Operators.Add("<>", Token_Class.NotEqual);
            Operators.Add("&&", Token_Class.AND);
            Operators.Add("||", Token_Class.OR);
            Operators.Add("+", Token_Class.Plus);
            Operators.Add("-", Token_Class.Minus);
            Operators.Add("*", Token_Class.Multiply);
            Operators.Add("/", Token_Class.Division);



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

                else if (isDigit(CurrentChar))//If you read a digit
                {
                    while (j+1 < SourceCode.Length && isDigit(SourceCode[j+1]))
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                    }
                    i = j;
                    FindTokenClass(CurrentLexeme);

                }

                else if(CurrentChar=='"')//If you read a string
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

                else if(CurrentChar == '/' && j + 1 < SourceCode.Length && SourceCode[j + 1] == '*')//If you read a comment
                {
                    while (j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];

                        if (SourceCode[j] == '/' && SourceCode[j - 1] == '*')
                            break;
                    }
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }

                else if(isTwo_LetterOp(j, SourceCode, ':', '=') || isTwo_LetterOp(j, SourceCode, '&', '&') || isTwo_LetterOp(j, SourceCode, '|', '|')) //If two-leter operator
                {
                    j++;
                    CurrentLexeme += SourceCode[j];
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }

                else//If Symbol
                {
                    FindTokenClass(CurrentLexeme);
                }
            }

            TinyCompiler.TokenStream = Tokens;

        }
        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;

            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
                Tok.token_type = ReservedWords[Lex];

            //Is it an identifier?
            else if (isIdentifier(Lex))
                Tok.token_type = Token_Class.Identifier;

            //Is it a Number?
            else if (isNumber(Lex))
                Tok.token_type = Token_Class.Number;

            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
                Tok.token_type = Operators[Lex];

            //Is it a String?
            else if (isString(Lex))
                Tok.token_type = Token_Class.String;

            //Is Comment?
            else if (isComment(Lex))
                Tok.token_type = Token_Class.Comment;


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

        bool isNumber(string lex){
            return new Regex("^[0-9]+([.][0-9]+)?$").IsMatch(lex);
        }

        bool isString(string lex){
            return new Regex("^[\"][^\"]*[\"]$").IsMatch(lex);
        }

        bool isComment(string lex)
        {
            MessageBox.Show(lex);
            return new Regex(@"/\*[\s\S]*?\*/").IsMatch(lex);
            //@"^(/\*).*(\*\/)$"
        }

        bool isWhiteSpace(char character){
            return character == ' ' || character == '\r' || character == '\n' || character == '\t';
        }

        bool isLetter(char character){
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }

        bool isDigit(char character){
            return character >= '0' && character <= '9';
        }

        bool isTwo_LetterOp(int i, string str, char first, char second)
        {
            return i + 1 < str.Length && str[i] == first && str[i + 1] == second; 
        }
    }
}
