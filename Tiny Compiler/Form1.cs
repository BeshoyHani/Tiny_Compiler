using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearAll();

            //string Code=textBox1.Text.ToLower();
            string Code = textBox1.Text;
            TinyCompiler.Start_Compiling(Code);

            //   PrintLexemes();

            PrintTokens();

            treeView1.Nodes.Add(Parser.PrintParseTree(TinyCompiler.treeroot));


            PrintErrors();
        }

        void PrintTokens()
        {

            foreach (Token token in  TinyCompiler.tiny_Scanner.Tokens)
            {
                dataGridView1.Rows.Add(token.lex, token.token_type);
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                textBox2.Text += Errors.Error_List[i];
                textBox2.Text += "\r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        void ClearAll()
        {
            textBox2.Clear();
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();

            TinyCompiler.TokenStream.Clear();
            TinyCompiler.tiny_Scanner.Tokens.Clear();
            Errors.Error_List.Clear();
        }
        

    }
}
