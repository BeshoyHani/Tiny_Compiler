using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{

    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }

    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());

            return root;
        }


        public Node Program()
        {
            Node program = new Node("Program");

            int temp_InputPointer = InputPointer;


            Node main_res = Main_Function();

            if (main_res != null)
            {
                program.Children.Add(main_res);
                return program;
            }
            InputPointer = temp_InputPointer;


            Node res = Function_Statement();

            if (res != null)
            {
                program.Children.Add(res);
                program.Children.Add(Program());

                return program;
            }

            return null;
        }

        public Node Main_Function()
        {
            Node node = new Node("Main_Function");

            int temp_InputPointer = InputPointer;

            Node res = DataType();

            if (res != null)
            {
               
                node.Children.Add(res);
                node.Children.Add(match(Token_Class.Main));
                node.Children.Add(match(Token_Class.LeftParentheses));
                node.Children.Add(match(Token_Class.RightParentheses));
                node.Children.Add(Function_Body());
                return node;
            }
            InputPointer = temp_InputPointer;

            return node;
        }

        public Node Function_Statement()
        {
            Node node = new Node("Function_Statement");

            Node res = Function_Declartion();

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(Function_Body());
                return node;
            }

            return null;
        }

        private Node Function_Body()
        {
            Node node = new Node("Function_Body");
            int temp_InputPointer = InputPointer;

            Node res = match(Token_Class.LeftBraces);

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(Statements());
                node.Children.Add(Return_Statement());
                node.Children.Add(match(Token_Class.RightBraces));
                return node;
            }

            InputPointer = temp_InputPointer;

            return null;
        }




        private Node Statements()
        {
            Node node = new Node("Statements");
            int temp_InputPointer = InputPointer;

            Node res = Statement();

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(Statements());

                return node;
            }
            InputPointer = temp_InputPointer;
            return node;

        }

        private Node Statement()
        {
            Node node = new Node("Statement");
            int temp_InputPointer = InputPointer;

            Node res = Read_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                return node;
            }
            InputPointer = temp_InputPointer;
            res = Write_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                return node;
            }
            InputPointer = temp_InputPointer;
            res = Assignment_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(match(Token_Class.Semicolon));
                return node;
            }
            InputPointer = temp_InputPointer;
            res = Declaration_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                return node;
            }
            InputPointer = temp_InputPointer;
            res = If_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                return node;
            }
            InputPointer = temp_InputPointer;
            res = Repeat_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                return node;
            }
            InputPointer = temp_InputPointer;
            res = Return_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                return node;
            }
            InputPointer = temp_InputPointer;
            return null;
        }

        private Node Function_Declartion()
        {
            Node node = new Node("Function_Declartion");
            int temp_InputPointer = InputPointer;

            Node res = DataType();

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(match(Token_Class.Identifier));
                node.Children.Add(match(Token_Class.LeftParentheses));
                node.Children.Add(ParList());
                node.Children.Add(match(Token_Class.RightParentheses));
                return node;
            }
            InputPointer = temp_InputPointer;
            return null;
        }

        private Node ParList()
        {
            Node node = new Node("ParList");

            Node res = Parameter();
            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(ParListDash());
            }

            return node;
        }

        private Node ParListDash()
        {
            Node node = new Node("ParListDash");

            if (check(Token_Class.Comma))
            {
                node.Children.Add(match(Token_Class.Comma));
                node.Children.Add(Parameter());
                node.Children.Add(ParListDash());
            }

            return node;
        }

        private Node Parameter()
        {
            Node node = new Node("Parameter");
            int temp_InputPointer = InputPointer;

            Node res = DataType();

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(match(Token_Class.Identifier));

                return node;
            }
            InputPointer = temp_InputPointer;
            return null;
        }
        private Node Repeat_Statement()
        {
            Node node = new Node("Repeat_Statement");

            if (check(Token_Class.Repeat))
            {
                node.Children.Add(match(Token_Class.Repeat));
                node.Children.Add(Statements());
                node.Children.Add(match(Token_Class.Until));
                node.Children.Add(Condition_Statement());
                return node;
            }
            return null;
        }
        private Node Condition()
        {
            Node node = new Node("Condition");

            if (check(Token_Class.Identifier))
            {
                node.Children.Add(match(Token_Class.Identifier));
                node.Children.Add(Condition_Operator());
                node.Children.Add(Term());
                return node;
            }

            return null;
        }
        private Node Else_Statement()
        {
            Node node = new Node("Else_Statement");

            if (check(Token_Class.Else))
            {
                node.Children.Add(match(Token_Class.Else));
                node.Children.Add(Statements());
                node.Children.Add(match(Token_Class.End));
                return node;
            }

            return null;
        }
        private Node Else_If_Statement()
        {
            Node node = new Node("Else_If_Statement");


            if (check(Token_Class.ElseIf))
            {
                node.Children.Add(match(Token_Class.ElseIf));
                node.Children.Add(Condition_Statement());
                node.Children.Add(Statements());
                node.Children.Add(After_If());
                return node;
            }
            return null;
        }
        private Node If_Statement()
        {
            Node node = new Node("If_Statement");


            if (check(Token_Class.IF))
            {
                node.Children.Add(match(Token_Class.IF));
                node.Children.Add(Condition_Statement());
                node.Children.Add(Statements());
                node.Children.Add(After_If());
                return node;
            }
            return null;
        }

        private Node After_If()
        {
            Node node = new Node("After_If");
            int temp_InputPointer = InputPointer;

            Node res = Else_If_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                return node;
            }
            InputPointer = temp_InputPointer;
            res = Else_Statement();

            if (res != null)
            {
                node.Children.Add(res);
                return node;
            }
            InputPointer = temp_InputPointer;
            if (TokenStream[InputPointer].token_type == Token_Class.End)
            {
                node.Children.Add(match(Token_Class.End));
                return node;
            }

            return null;
        }

        private Node Condition_Operator()
        {
            Node node = new Node("Condition_Operator");
            if (TokenStream[InputPointer].token_type == Token_Class.LessThan)
            {
                node.Children.Add(match(Token_Class.LessThan));
                return node;
            }
            if (TokenStream[InputPointer].token_type == Token_Class.GreaterThan)
            {
                node.Children.Add(match(Token_Class.GreaterThan));
                return node;
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Equal)
            {
                node.Children.Add(match(Token_Class.Equal));
                return node;
            }
            if (TokenStream[InputPointer].token_type == Token_Class.NotEqual)
            {
                node.Children.Add(match(Token_Class.NotEqual));
                return node;
            }
            return null;
        }
        private Node Boolean_Operator()
        {
            Node node = new Node("Boolean_Operator");
            if (TokenStream[InputPointer].token_type == Token_Class.OR)
            {
                node.Children.Add(match(Token_Class.OR));
                return node;
            }
            if (TokenStream[InputPointer].token_type == Token_Class.AND)
            {
                node.Children.Add(match(Token_Class.AND));
                return node;
            }

            return null;
        }

        private Node Condition_Statement()
        {
            Node node = new Node("Condition_Statement");
            int temp_InputPointer = InputPointer;

            Node res = Condition();

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(CStatement());
                return node;
            }
            InputPointer = temp_InputPointer;

            return null;
        }

        private Node CStatement()
        {
            Node node = new Node("CStatement");
            int temp_InputPointer = InputPointer;

            Node res = Boolean_Operator();

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(Condition());
                node.Children.Add(CStatement());
                return node;
            }
            InputPointer = temp_InputPointer;

            return node;
        }

        private Node DataType()
        {
            Node node = new Node("DataType");

            if (check(Token_Class.DataType_INT))
                node.Children.Add(match(Token_Class.DataType_INT));

            else if (check(Token_Class.DataType_Float))
                node.Children.Add(match(Token_Class.DataType_Float));
            else if (check(Token_Class.DataType_String))
                node.Children.Add(match(Token_Class.DataType_String));
            else
                return null;
            return node;
        }

        private Node Function_Call()
        {
            Node node = new Node("Function_Call");
            int temp_InputPointer = InputPointer;

            Node res = match(Token_Class.Identifier);

            if (res == null)
            {
                InputPointer = temp_InputPointer;
                return null;
            }

            node.Children.Add(res);
            node.Children.Add(match(Token_Class.LeftParentheses));
            node.Children.Add(Arguments());
            node.Children.Add(match(Token_Class.RightParentheses));

            return node;
        }

        private Node Arguments()
        {
            Node node = new Node("Arguments");

            int temp_InputPointer = InputPointer;

            Node res = match(Token_Class.Identifier);

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(Arg());
                return node;
            }

            InputPointer = temp_InputPointer;

            return node;
        }

        private Node Arg()
        {
            Node node = new Node("Arg");

            int temp_InputPointer = InputPointer;

            Node res = match(Token_Class.Comma);

            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(match(Token_Class.Identifier));
                node.Children.Add(Arg());
                return node;
            }

            InputPointer = temp_InputPointer;

            return node;
        }

        private Node Term()
        {
            Node node = new Node("Term");


            if (check(Token_Class.Number))
            {
                node.Children.Add(match(Token_Class.Number));
            }
            else if (check(Token_Class.Identifier))
            {
                node.Children.Add(match(Token_Class.Identifier));
            }
            else
            {
                Node res = Function_Call();
                if (res == null) return null;
                node.Children.Add(res);
            }

            return node;
        }

        private Node Arithmatic_Operator()
        {
            Node node = new Node("Arithmatic_Operator");

            if (check(Token_Class.Plus))
                node.Children.Add(match(Token_Class.Plus));
            else if (check(Token_Class.Minus))
                node.Children.Add(match(Token_Class.Minus));
            else if (check(Token_Class.Multiply))
                node.Children.Add(match(Token_Class.Multiply));
            else if (check(Token_Class.Division))
                node.Children.Add(match(Token_Class.Division));
            else
                return null;

            return node;
        }

        private Node Equation()
        {
            Node node = new Node("Equation");

            Node res = Eq();
            if (res != null)
            {
                node.Children.Add(res);
            }

            else if (check(Token_Class.LeftParentheses))
            {
                node.Children.Add(match(Token_Class.LeftParentheses));
                node.Children.Add(Eq());
                node.Children.Add(match(Token_Class.RightParentheses));

            }
            else
                return null;

            return node;
        }

        private Node Eq()
        {
            Node node = new Node("Eq");

            Node Term_res = Term();

            if (Term_res != null)
            {
                node.Children.Add(Term_res);
                return node;
            }

            Node Equation_res = Equation();
            if (Equation_res != null)
            {
                node.Children.Add(Equation_res);
                node.Children.Add(Arithmatic_Operator());
                node.Children.Add(Term());

                return node;
            }
            return null;
        }
        private Node Expression()
        {
            Node node = new Node("Expression");

            Node Term_res = Term();

            if (check(Token_Class.String))
                node.Children.Add(match(Token_Class.String));
            else if (Term_res != null)
                node.Children.Add(Term_res);
            else
            {
                Node Equation_res = Equation();
                if (Equation_res == null) return null;
                node.Children.Add(Equation_res);
            }

            return node;
        }

        private Node Assignment_Statement()
        {
            Node node = new Node("Assignment_Statement");

            if (check(Token_Class.Identifier))
            {
                node.Children.Add(match(Token_Class.Identifier));
                node.Children.Add(match(Token_Class.Equal));
                node.Children.Add(Expression());
            }
            else
                return null;
            return node;

        }

        private Node Declaration_Statement()
        {
            Node node = new Node("Declaration_Statement");

            Node res = DataType();
            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(match(Token_Class.Identifier));
                node.Children.Add(Optional_Assignment());
                node.Children.Add(IdList());
                node.Children.Add(match(Token_Class.Semicolon));
            }
            else
                return null;
            return node;

        }

        private Node IdList()
        {
            Node node = new Node("IdList");

            if (check(Token_Class.Comma))
            {
                node.Children.Add(match(Token_Class.Semicolon));
                node.Children.Add(match(Token_Class.Identifier));
                node.Children.Add(Optional_Assignment());
                node.Children.Add(IdList());
            }

            return node;
        }

        private Node Optional_Assignment()
        {
            Node node = new Node("Optional_Assignment");

            if (check(Token_Class.Assign))
            {
                node.Children.Add(match(Token_Class.Assign));
                node.Children.Add(Expression());
            }
            return node;
        }


        private Node Write_Statement()
        {
            Node node = new Node("Write_Statement");

            if (!check(Token_Class.Write)) return null;

            node.Children.Add(match(Token_Class.Write));
            node.Children.Add(Write_StatementDash());

            return node;
        }

        private Node Write_StatementDash()
        {
            Node node = new Node("Write_StatementDash");

            Node res = Expression();
            if (res != null)
            {
                node.Children.Add(res);
                node.Children.Add(match(Token_Class.Semicolon));
            }
            else if (check(Token_Class.Endl))
            {
                node.Children.Add(match(Token_Class.Endl));
                node.Children.Add(match(Token_Class.Semicolon));
            }
            else
                return null;
            return node;

        }

        private Node Read_Statement()
        {
            Node node = new Node("Read_Statement ");

            if (!check(Token_Class.Read)) return null;

            node.Children.Add(match(Token_Class.Read));
            node.Children.Add(match(Token_Class.Identifier));
            node.Children.Add(match(Token_Class.Semicolon));

            return node;
        }

        private Node Return_Statement()
        {
            Node node = new Node("Return_Statement");


            if (check(Token_Class.Return) == false) return null;


            node.Children.Add(match(Token_Class.Return));
            node.Children.Add(Expression());
            node.Children.Add(match(Token_Class.Semicolon));


            return null;
        }

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {

                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        bool check(Token_Class ExpectedToken)
        {
            if (InputPointer < TokenStream.Count && ExpectedToken == TokenStream[InputPointer].token_type)
                return true;
            else return false;
        }


        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }

        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
