using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Media;

namespace WindowsFormsApplication1
{
    public partial class MagicBuild : Form
    {


        public struct Token
        {
            public string Name;
            public string Value;
            public Token(string n, string v)
            {
                Name = n;
                Value = v;
            }
        }
        public struct TokenRegist
            {
                public string Name;
                public string Regex;
                public TokenRegist(string N, string R)
                {
                    Name = N;
                    Regex = R;
                }

            }

        public class Tokenizator {
            List<TokenRegist> RegEx = new List<TokenRegist>();
            public Tokenizator() {
                RegEx.Add(new TokenRegist("printT",@"(^print$)"));
                RegEx.Add(new TokenRegist("openBracketsT", @"(^\($)"));
                RegEx.Add(new TokenRegist("closeBracketsT", @"(^\)$)"));
                RegEx.Add(new TokenRegist("varT", @"(^([A-Za-z][A-Za-z0-9]*)$)"));
                RegEx.Add(new TokenRegist("arOpT", @"(^([+|\-|*|\/])$)"));
                RegEx.Add(new TokenRegist("digitT", @"(^((-?\d+)(\,\d*)?)$)"));
                RegEx.Add(new TokenRegist("equalOpT", @"^(=)$"));
                RegEx.Add(new TokenRegist("endT", @"(^(;)$)"));
                RegEx.Add(new TokenRegist("spaceT", @"(^\s$)"));
                RegEx.Add(new TokenRegist("Error: unknown element", "[$.`'{}<>]"));

            }
            

            public string  TokenIsMatch(string progtext) {
                foreach (TokenRegist r in RegEx) {
                    if (Regex.IsMatch(progtext, r.Regex)) { return r.Name; break; }
                }
                return null;
            }

            public bool IsSpace(Token s) {
                return Regex.IsMatch(s.Value, @"(^\s$)");
            }
            
            public List<Token> getTokens (string programmText) {
                string buffer1= "";
                string buffer2= "";
                List<Token> ListOfTokens = new List<Token>();

                for (int i=0; i <= programmText.Length -1 ;i++) {
                    buffer2 = buffer1;
                    buffer1 += programmText[i];
                    if (TokenIsMatch(buffer1) == null) {
                        i--;
                        ListOfTokens.Add(new Token(TokenIsMatch(buffer2), buffer2));
                        buffer1 = "";
                    }
                    
                }

                if (TokenIsMatch(buffer1) != null)
                {
                    ListOfTokens.Add(new Token(TokenIsMatch(buffer1), buffer1));
                }
                ListOfTokens.RemoveAll(IsSpace);
                return ListOfTokens;
            }
            
            
        }

        public struct Lex {
            public string token1;
            public string token2;
            public Lex(string token1,string token2) {
                this.token1 = token1;
                this.token2 = token2;
            }
        }

        public class Lexer
        {

            List<Lex> Lexs = new List<Lex>();

            public Lexer()
            {

                Lexs.Add(new Lex("printT", "openBracketsT"));

                Lexs.Add(new Lex("openBracketsT", "varT"));
                Lexs.Add(new Lex("openBracketsT", "digitT"));
                Lexs.Add(new Lex("openBracketsT", "spaceT"));

                Lexs.Add(new Lex("closeBracketsT", "arOpt"));
                Lexs.Add(new Lex("closeBracketsT", "equalOpT"));
                Lexs.Add(new Lex("closeBracketsT", "endT"));
                Lexs.Add(new Lex("closeBracketsT", "spaceT"));

                Lexs.Add(new Lex("varT", "closeBracketsT"));
                Lexs.Add(new Lex("varT", "arOpT"));
                Lexs.Add(new Lex("varT", "equalOpT"));
                Lexs.Add(new Lex("varT", "endT"));
                Lexs.Add(new Lex("varT", "spaceT"));

                Lexs.Add(new Lex("arOpT", "openBracketsT"));
                Lexs.Add(new Lex("arOpT", "varT"));
                Lexs.Add(new Lex("arOpT", "digitT"));
                Lexs.Add(new Lex("arOpT", "spaceT"));

                Lexs.Add(new Lex("digitT", "closeBracketsT"));
                Lexs.Add(new Lex("digitT", "arOpT"));
                Lexs.Add(new Lex("digitT", "endT"));
                Lexs.Add(new Lex("digit", "SpaceT"));

                Lexs.Add(new Lex("equalOpT", "openBracketsT"));
                Lexs.Add(new Lex("equalOpT", "varT"));
                Lexs.Add(new Lex("equalOpT", "digitT"));
                Lexs.Add(new Lex("equalOpT", "SpaceT"));

                Lexs.Add(new Lex("endT", "printT"));
                Lexs.Add(new Lex("end", "openBracketsT"));
                Lexs.Add(new Lex("end", "varT"));
                Lexs.Add(new Lex("end", "spaceT"));

                Lexs.Add(new Lex("spaceT", "printT"));
                Lexs.Add(new Lex("spaceT", "openBracketsT"));
                Lexs.Add(new Lex("spaceT", "closeBracketsT"));
                Lexs.Add(new Lex("spaceT", "varT"));
                Lexs.Add(new Lex("spaceT", "arOpT"));
                Lexs.Add(new Lex("spaceT", "digitT"));
                Lexs.Add(new Lex("spaceT", "equalOpT"));
                Lexs.Add(new Lex("spaceT", "endT"));
            }

            private bool IsRightLex(Token t1, Token t2)
            {

                foreach (Lex l in Lexs)
                {
                    if ((t1.Name == l.token1) && (t2.Name == l.token2))
                    {
                        return true;

                    }
                }
                return false;

            }

            public bool Lexe(List<Token> Tokens)
            {
                if (Tokens == null) { return true; }
                if (Tokens[0].Name == "Error: unknown element") { return false; }
                for (int i = 0; i <= Tokens.Count - 2; i++)
                {
                    if (IsRightLex(Tokens[i], Tokens[i + 1]) == false)
                        return false;

                }
                return true;

            }

            public bool BracketsRight(List<Token> Tokens)
            {
                int openBracketsT = 0, closeBracketsT = 0;
                foreach (Token tk in Tokens)
                {
                    switch (tk.Name)
                    {
                        case "openBracketsT": { openBracketsT++; break; }
                        case "closeBracketsT": { closeBracketsT++; break; }
                        default: break;
                    }
                }

                if (openBracketsT == closeBracketsT)  return true;
                else return false;

            }


        }

        public class Kurva {                                       

            public int hmpriority(Token t) {                               
                if (t.Value == "/" || t.Value == "*") return 10;
                if (t.Value == "+" || t.Value == "-") return 9;
                if (t.Value == "="||t.Value== "print") return 8;
                else return 0;
            }

            public List<Token> getpolsk (List<Token> nepolsk) {            
                bool toprint = false;
                List<Token> alreadyPolsk = new List<Token>();
                Stack<Token> stack = new Stack<Token>();
                foreach (Token token in nepolsk) {
                    if (token.Name == "printT") toprint = true;
                    else if (token.Name == "endT") while (stack.Count != 0) alreadyPolsk.Add(stack.Pop());
                    else if (token.Name == "digitT" || token.Name == "varT") alreadyPolsk.Add(token);
                    else if (token.Name == "openBracketsT") stack.Push(token);
                    else if (token.Name == "closeBracketsT")
                    {
                        while (stack.Peek().Name != "openBracketsT" || stack.Count == 0) alreadyPolsk.Add(stack.Pop());
                        stack.Pop();
                    }
                    else if (token.Name == "arOpT" || token.Name == "equalOpT")
                    {
                        if (stack.Count == 0) { stack.Push(token); }
                        else if (stack.Peek().Name == "openBracketsT") stack.Push(token);
                        else if (hmpriority(token) <= hmpriority(stack.Peek()))
                        {
                            while (stack.Count != 0 && hmpriority(token) <= hmpriority(stack.Peek()))
                            {
                                if (stack.Peek().Name == "openBracketsT") break;
                                alreadyPolsk.Add(stack.Pop());
                            }
                            stack.Push(token);
                        }
                        else if (hmpriority(token) > hmpriority(stack.Peek())) stack.Push(token);
                    }

                }
                if (toprint) alreadyPolsk.Add(new Token("printT", "print"));

                while (stack.Count != 0) {
                    alreadyPolsk.Add(stack.Pop());
                }
                return alreadyPolsk;

            }
        }

        public struct VrHol {                          
            public double value;
            public string name;

           public VrHol(string name, double value) {
                this.name = name;
                this.value = value;
            }
        }

        public class Machine
        {
            private Stack<VrHol> stack = new Stack<VrHol>();
            private List<VrHol> vars = new List<VrHol>();
            bool IsExist(string var)
            {
                foreach (VrHol v in vars)
                {
                    if (v.name == var) return true;

                }
                return false;
            }

            double GetVrHolByName(string name)
            {
                foreach (VrHol var in vars)
                {
                    if (var.name == name) return var.value;
                }
                return 0f;
            }

            int GetVrHolByNameIndex(string name)
            {
                for (int i = 0; i < vars.Count; i++)
                {
                    if (vars[i].name == name) return i;
                }
                return -1;
            }
            public string vmachine(List<Token> polskStroka)
            {

                string output = "";
                foreach (Token token in polskStroka)
                {
                    switch (token.Name)
                    {
                        case "printT":
                            output += stack.Pop().value;
                            break;
                        case "varT":
                            {
                                if (!IsExist(token.Value))
                                {
                                    stack.Push(new VrHol(token.Value, 0f));
                                    vars.Add(new VrHol(token.Value, 0f));
                                }
                                else
                                {
                                    stack.Push(new VrHol(token.Value, GetVrHolByName(token.Value)));
                                    

                                }
                            }
                            break;
                        case "digitT":
                            stack.Push(new VrHol("", Convert.ToDouble(token.Value)));
                            break;

                        case "arOpT":
                            {
                                double op1 = stack.Pop().value;
                                double op2 = stack.Pop().value;
                                if (token.Value == "+")
                                {
                                    stack.Push(new VrHol("", (op1 + op2)));
                                }
                                else if (token.Value == "-")
                                {
                                    stack.Push(new VrHol("", (op2 - op1)));
                                }
                                else if (token.Value == "*")
                                {
                                    stack.Push(new VrHol("", (op1 * op2)));
                                }
                                else if (token.Value == "/")
                                {
                                    stack.Push(new VrHol("", (op1 / op2)));
                                }
                            }
                            break;
                        case "equalOpT":
                            {
                                double opb = stack.Pop().value;
                                string opa = stack.Pop().name;
                                int a = (GetVrHolByNameIndex(opa));
                                vars[a] = new VrHol("", opb);
                            }
                            break;

                        case "endT": { stack.Clear(); break; }
                        default: { break; }

                    }
                }
                return output;
            }
        }


        public MagicBuild()
        {
            InitializeComponent();
        }

        private void MagicBuild_Load(object sender, EventArgs e)
        {
           
        }


        private void Click(object sender, EventArgs e)
        {

            Tokenizator tokenezator = new Tokenizator();
            Lexer lexer = new Lexer();
            Tokens.Text = "";
            Output.Text = "";
            Perdole.Text = "";
            List <Token> tks = tokenezator.getTokens(ProgrammText.Text);

            foreach (Token t in tks){
                Tokens.Text += t.Name + ", ";
            }

            if (lexer.BracketsRight(tks)){

                if (lexer.Lexe(tks))
                {

                    Kurva Kurva = new Kurva();
                    List<Token> polis = Kurva.getpolsk(tks);

                    foreach (Token t in polis)
                    {
                        Perdole.Text += t.Value + ", ";
                    }

                    Machine machine = new Machine();
                    Output.Text = machine.vmachine(polis);
                }
                else Output.Text = "There is some trouble's whith UR sinthax. PLS check it";
            }
            else Output.Text = "U have not equal amount of brackets";
        }
    }
}
