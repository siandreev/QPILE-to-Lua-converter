using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;

namespace Converter
{
    class LuaVisitor : QPILE_converterV2BaseVisitor<string>
    {
        public override string VisitProgram([NotNull] QPILE_converterV2Parser.ProgramContext context)
        {
           return Visit(context.children[1]);
        }

        public override string VisitDeclarationBlock([NotNull] QPILE_converterV2Parser.DeclarationBlockContext context)
        {
            return base.VisitDeclarationBlock(context);
        }

        public override string VisitParamsBlock([NotNull] QPILE_converterV2Parser.ParamsBlockContext context)
        {
            return base.VisitParamsBlock(context);
        }

        public override string VisitProgramBlock([NotNull] QPILE_converterV2Parser.ProgramBlockContext context)
        {
            var stList = Visit(context.children[2]);
            return "function main()" + "\n" + stList + "end";
        }

        public override string VisitStatementList([NotNull] QPILE_converterV2Parser.StatementListContext context)
        {
            string res = "";
            foreach (var elem in context.children)
            {
                if (elem.GetText() != "\r\n")
                {
                    var st = Visit(elem);
                    res += st + "\n";
                }
            }
            return res;
        }

        public override string VisitStatement([NotNull] QPILE_converterV2Parser.StatementContext context)
        {
            if ((context.ifOperator() != null ) || (context.forOperator() != null) || (context.funcDescr() != null) || (context.procedureCall() != null))
            {
                var res = Visit(context.children[0]);
                return res; 
            }

            if (context.RETURN() != null)
            {
                return AddSpaces("return RESULT");
            }

            if (context.BREAK() != null)
            {
                return AddSpaces("break");
            }

            if (context.CONTINUE() != null)
            {
                return AddSpaces("continue");
            }

            if (context.EQUAL() != null)
            {
                var exp = Visit(context.children[2]);
                return "    " +  context.name().GetText() + " = " + exp ;
            }     

            return base.VisitStatement(context);
        }

        public override string VisitProcedureCall([NotNull] QPILE_converterV2Parser.ProcedureCallContext context)
        {
            if (context.qProcedureCall() != null)
            {
                return Visit(context.children[0]);
            }

            var fName = Visit(context.children[0]);
            var argList1 = Visit(context.children[2]);
            return AddSpaces(fName + " (" + argList1 + ")");
        }

        public override string VisitQProcedureCall([NotNull] QPILE_converterV2Parser.QProcedureCallContext context)
        {
            if (context.MESSAGE() != null)
            {
                var argList1 = Visit(context.children[2]);
                return AddSpaces("message" + " (" + argList1 + ")");
            }

            if (context.DELETE_ALL_ITEMS() != null)
            {
                return AddSpaces("isCleared = Clear (0)");
            }

            if (context.ADD_ITEM() != null)
            {
                var index = Visit(context.children[2]); 
                return AddSpaces("rowNumber = InsertRow (0, " + index + ")" );
            }

            return base.VisitQProcedureCall(context);
        }

        public override string VisitFuncDescr([NotNull] QPILE_converterV2Parser.FuncDescrContext context)
        {
            var name = Visit(context.children[1]);
            var fargList = Visit(context.children[3]);
            var statementList = Visit(context.children[6]);
            string res = "#@" + "function " + name + " (" + fargList + ") " + "\n" + statementList +"end" + "@#";
            return res;
        }

        public override string VisitIfOperator([NotNull] QPILE_converterV2Parser.IfOperatorContext context)
        {
            var condition = Visit(context.children[1]);
            condition = condition.Replace("=", "==");
            var ifStatement = Visit(context.children[3]);
            var elseStatement = Visit(context.children[7]);
            var res = "if " + condition + " then" + "\n" +
                  ifStatement +
                  "else" + "\n" +
                   elseStatement   + "end";
            return AddSpaces(res);
        }

        public override string VisitForOperator([NotNull] QPILE_converterV2Parser.ForOperatorContext context)
        {
            if (context.FROM() != null)
            {
                var name = Visit(context.children[1]);
                var expression1 = Visit(context.children[3]);
                var expression2 = Visit(context.children[5]);
                var statement = Visit(context.children[7]);
                var res = "for " + name + " = " + expression1 + "," + expression2 + " do" + "\n" +
                     statement +
                     "end";
                return AddSpaces(res);
            }

            return null; //foreach не обрабатывается
        }

        public override string VisitCondition([NotNull] QPILE_converterV2Parser.ConditionContext context)
        {
           if (context.primaryCondition() != null)
           {
                return Visit(context.children[0]);
           }

           if (context.OR() != null)
           {
                var condition1 = Visit(context.children[0]);
                var condition2 = Visit(context.children[2]);
                return condition1 + " or " + condition2;
           }

            if (context.AND() != null)
            {
                var condition1 = Visit(context.children[0]);
                var condition2 = Visit(context.children[2]);
                return condition1 + " and " + condition2;
            }

            if (context.LPAREN() != null)
            {
                var condition = Visit(context.children[1]);
                return " (" + condition + ") ";
            }

            return Visit(context.children[0]);
        }

        public override string VisitPrimaryCondition([NotNull] QPILE_converterV2Parser.PrimaryConditionContext context)
        {
            var expression1 = Visit(context.children[0]);
            var expression2 = Visit(context.children[2]);
            string relation = null;
            if (context.EQUAL() != null)
            {
                relation = " = ";
            }

            if (context.LE() != null)
            {
                relation = " <= ";
            }

            if (context.LT() != null)
            {
                relation = " < ";
            }

            if (context.GE() != null)
            {
                relation = " >= ";
            }

            if (context.GT() != null)
            {
                relation = " > ";
            }

            if (context.NOT_EQUAL() != null)
            {
                relation = " ~= ";
            }

            return expression1 + relation + expression2;
        }

        public override string VisitExpression([NotNull] QPILE_converterV2Parser.ExpressionContext context)
        {
            if (context.PLUS() != null)
            {
                var exp = Visit(context.children[0]);
                var term = Visit(context.children[2]);
                return exp + " + " + term; 
            }

            if (context.MINUS() != null)
            {
                var exp = Visit(context.children[0]);
                var term = Visit(context.children[2]);
                return exp + " - " + term;
            }

            if (context.COMPOUND() != null)
            {
                var exp = Visit(context.children[0]);
                var term = Visit(context.children[2]);
                return exp + " .. " + term;
              
            }
            var onlyTerm = Visit(context.children[0]);
            return onlyTerm;
        }

        public override string VisitTerm([NotNull] QPILE_converterV2Parser.TermContext context)
        {
            if (context.SLASH() != null)
            {
                var term = Visit(context.children[0]);
                var primary = Visit(context.children[2]);
                return term + " / " + primary;
            }

            if (context.STAR() != null)
            {
                var term = Visit(context.children[0]);
                var primary = Visit(context.children[2]);
                return term + " * " + primary;
            }

            var onlyPrimary = Visit(context.children[0]);
            return onlyPrimary;
        }

        public override string VisitPrimary([NotNull] QPILE_converterV2Parser.PrimaryContext context)
        {
            if (context.number() != null)
            {
                var number = Visit(context.children[0]);
                return number;
            }

            if (context.name() != null)
            {
                var name = Visit(context.children[0]);
                return name;
            }

            if (context.STRING_SYMBOLS() != null)
            {
                return context.STRING_SYMBOLS().GetText();
            }

            if (context.MINUS() != null)
            {
                var primary = Visit(context.children[1]);
                return "-" + primary;
            }

            if (context.LPAREN() != null)
            {
                var exp = Visit(context.children[1]);
                return "(" + exp + ")";
            }

            return Visit(context.children[0]);
        }

        public override string VisitFunctionCall([NotNull] QPILE_converterV2Parser.FunctionCallContext context)
        {
            if (context.qFunctionCall() == null)
            {
                var fName = Visit(context.children[0]);
                var argList1 = Visit(context.children[2]);
                return fName + " (" + argList1 + ")";
            }
            return Visit(context.children[0]);
        }

        public override string VisitQFunctionCall([NotNull] QPILE_converterV2Parser.QFunctionCallContext context)
        {
            if (context.CREATE_MAP() != null)
            {
                return "{}";
            }

            if (context.SET_VALUE() != null)
            {
                var name = Visit(context.children[2]);
                var key = context.children[4].GetText();
                var value = Visit(context.children[6]);
                return "setValue(" + name + ", " + key + ", " + value + ")" + "\n"
                    + GenerateSet_ValueFunction();
            }

            if (context.GET_INFO_PARAM() != null)
            {
                var str = context.children[2].GetText();
                return "getInfoParam" + " ("  + str  + ")";
            }

            if (context.SUBSTR() != null)
            {
                string str;
                if (context.STRING_SYMBOLS() != null)
                {
                    str = context.children[2].GetText();
                }
                else
                {
                    str = Visit(context.children[2]);
                }
                    var index = Visit(context.children[4]);
                    var length = Visit(context.children[6]);
                    return "string.sub" + "(" + str + ", " + index + ", " + index + " + " + length + ")"; 
                
            }

            return base.VisitQFunctionCall(context);
        }

        public override string VisitArgList1([NotNull] QPILE_converterV2Parser.ArgList1Context context)
        {
            if (context.argList1() == null)
            {
                return Visit(context.children[0]);
            }

            var arglist1 = Visit(context.children[0]);
            var expression = Visit(context.children[2]);
            return arglist1 + ", " + expression;
        }

        public override string VisitFargList([NotNull] QPILE_converterV2Parser.FargListContext context)
        {
            if (context.fargList() == null)
            {
                return Visit(context.children[0]);
            }

            var fargList = Visit(context.children[0]);
            var name = Visit(context.children[2]);
            return fargList + ", " + name;
        }

        public override string VisitFName([NotNull] QPILE_converterV2Parser.FNameContext context)
        {
            return Visit(context.children[0]);
        }

        public override string VisitName([NotNull] QPILE_converterV2Parser.NameContext context)
        {
            return context.IDENT().ToString();
        }

        public override string VisitNumber([NotNull] QPILE_converterV2Parser.NumberContext context)
        {
            if (context.NUM_INT() != null)
            {
                return context.NUM_INT().GetText();
            }

            return context.NUM_REAL().GetText();

        }

        public static string AddSpaces(string source)
        {
            string st = source + " ";
            string spaces = "    ";
            string substring = "\n";
            var indices = new List<int>();
            indices.Add(-1);
            int index = source.IndexOf(substring, 0);
            int i = 0;
            while (index > -1)
            {

                indices.Add(index);
                index = source.IndexOf(substring, index + substring.Length);
            }

            foreach (int elem in indices)
            {
                i++;
                st = st.Insert(elem + 4 * i - 3, spaces);
            }
            return st;
        }

        public static string GenerateSet_ValueFunction()
        {
            return "#@function setValue(name, key, value)" + "\n" +
                AddSpaces("name" + " [" + "key" + "]" + " = " + "value") + "\n"
                + AddSpaces("return name") + "\n" + "end@#";
        }
    }
}
