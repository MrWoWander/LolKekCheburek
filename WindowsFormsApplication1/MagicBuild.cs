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
        public struct TokensanRegist
            {
                public string Name;
                public string Regex;
                public TokensanRegist(string N, string R)
                {
                    Name = N;
                    Regex = R;
                }

            }

        public class Tokensan {
            List<TokensanRegist> RegEx = new List<TokensanRegist>();
            public Tokensan() {
                RegEx.Add(new TokensanRegist("printT",@"(^print$)"));
                RegEx.Add(new TokensanRegist("openBracketsT", @"(^\($)"));
                RegEx.Add(new TokensanRegist("closeBracketsT", @"(^\)$)"));
                RegEx.Add(new TokensanRegist("varT", @"(^([A-Za-z][A-Za-z0-9]*)$)"));
                RegEx.Add(new TokensanRegist("arOpT", @"(^([+|\-|*|\/])$)"));
                RegEx.Add(new TokensanRegist("digitT", @"(^((-?\d+)(\,\d*)?)$)"));
                RegEx.Add(new TokensanRegist("equalOpT", @"^(=)$"));
                RegEx.Add(new TokensanRegist("endT", @"(^(;)$)"));
                RegEx.Add(new TokensanRegist("spaceT", @"(^\s$)"));
            }
            

            public string  TokenIsMatch(string progtext) {
                foreach (TokensanRegist r in RegEx) {
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

        public struct VarHol {
            public double value;
            public string name;

           public VarHol(string name, double value) {
                this.name = name;
                this.value = value;
            }
        }

        public class Machine
        {
            private Stack<VarHol> stack = new Stack<VarHol>();
            private List<VarHol> vars = new List<VarHol>();
            bool IsExist(string var)
            {
                foreach (VarHol v in vars)
                {
                    if (v.name == var) return true;

                }
                return false;
            }

            double GetVarHolByName(string name)
            {
                foreach (VarHol var in vars)
                {
                    if (var.name == name) return var.value;
                }
                return 0f;
            }

            int GetVarHolByNameIndex(string name)
            {
                for (int i = 0; i < vars.Count - 1; i++)
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
                                    stack.Push(new VarHol(token.Value, 0f));
                                    vars.Add(new VarHol(token.Value, 0f));
                                }
                                else
                                {
                                    stack.Push(new VarHol(token.Value, GetVarHolByName(token.Value)));

                                }
                            }
                            break;
                        case "digitT":
                            stack.Push(new VarHol("", Convert.ToDouble(token.Value)));
                            break;

                        case "arOpT":
                            {
                                double op1 = stack.Pop().value;
                                double op2 = stack.Pop().value;
                                if (token.Value == "+")
                                {
                                    stack.Push(new VarHol("", (op1 + op2)));
                                }
                                else if (token.Value == "-")
                                {
                                    stack.Push(new VarHol("", (op1 - op2)));
                                }
                                else if (token.Value == "*")
                                {
                                    stack.Push(new VarHol("", (op1 * op2)));
                                }
                                else if (token.Value == "/")
                                {
                                    stack.Push(new VarHol("", (op1 / op2)));
                                }
                            }
                            break;
                        case "equalOpT":
                            {
                                double opb = stack.Pop().value;
                                string opa = stack.Pop().name;
                                int a = (GetVarHolByNameIndex(opa));
                                vars[a] = new VarHol("", opb);
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

        private void Ckick(object sender, EventArgs e)
        {

            Tokensan tokensan = new Tokensan();
            Tokens.Text = "";
            Output.Text = "";
            Kurva_perdole.Text = "";
            List <Token> tks = tokensan.getTokens(ProgrammText.Text);

            foreach (Token t in tks){
                Tokens.Text += t.Name + ", ";
            }

            Kurva Kurva = new Kurva();
            List<Token> polis = Kurva.getpolsk(tks);

            foreach (Token t in polis)
            {
                Kurva_perdole.Text += t.Value+ ", ";
            }

            Machine machine = new Machine();
            Output.Text=machine.vmachine(polis);

        }
    }
}
