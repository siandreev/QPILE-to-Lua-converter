using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace Converter
{
    class Logic
    {
        public static StringReader CreateReader(string path)
        {
            string text = File.ReadAllText(path);
            StringReader reader = new StringReader(text);
            return reader;
        }

        public static void WriteIntoFile(string text, string newPath)
        {
            using (StreamWriter sw = new StreamWriter(newPath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(text);
            }
        }
        
        public static string Convert(StringReader reader)
        {
            AntlrInputStream inpText = new AntlrInputStream(reader);
            QPILE_converterV2Lexer lexer = new QPILE_converterV2Lexer(inpText);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            QPILE_converterV2Parser parser = new QPILE_converterV2Parser(tokens);
            try
            {
                IParseTree tree = null;
                tree = parser.program();
                var visitor = new LuaVisitor();
                var results = ReplaceFunctions(visitor.Visit(tree));
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
 
        }

        public static string ReplaceFunctions(string s)
        {
            int index1;
            int index2;
            string sub1;
            string sub2;
            while (s.Contains("#@"))
            {
                index1 = s.IndexOf("#@");
                index2 = s.IndexOf("@#");
                sub1 = s.Substring(index1, index2 - index1 + 2);
                sub2 = sub1.Substring(2, sub1.Length - 4);
                s = s.Replace(sub1, "");
                s = s + "\n" +sub2 +"\n";
            }
            return s;
        }

        public static string DeleteSpaces(string s)
        {
            string expression = "\\n *\\n";
            Regex regex = new Regex(expression);
            while (Regex.IsMatch(s, expression))
            {
                s = regex.Replace(s, "\n");
            }
            return s;
        }

        public static string Convert(string str)
        {
            AntlrInputStream inpText = new AntlrInputStream(str);
            QPILE_converterV2Lexer lexer = new QPILE_converterV2Lexer(inpText);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            QPILE_converterV2Parser parser = new QPILE_converterV2Parser(tokens);
            try
            {
                IParseTree tree = null;
                tree = parser.program();
                var visitor = new LuaVisitor();
                var results = ReplaceFunctions(visitor.Visit(tree));
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }

        }

    }
    
}
