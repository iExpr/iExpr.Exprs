using iExpr.Evaluators;
using iExpr.Exprs.Program.Helpers;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exprs.Program
{
    internal class ForFunctionValue : PreFunctionValue
    {
        public IExpr Begin { get; private set; }
        public IExpr Condition { get; private set; }
        public IExpr Step { get; private set; }

        static void Execute(ForFunctionValue main, FunctionArgument args, EvalContext cal)
        {
            var children = args.Contents;
            try
            {
                for (cal.Evaluate(main.Begin);
                            OperationHelper.GetValue<bool>(cal.Evaluate(main.Condition));
                            cal.Evaluate(main.Step))
                {
                    try
                    {
                        foreach (var v in children) cal.Evaluate(v);
                    }
                    catch (BContinue)
                    {
                        continue;
                    }
                }
            }
            catch (BBreak)
            {
                return;
            }
        }

        public ForFunctionValue(IExpr begin,IExpr cond,IExpr step):base()
        {
            Begin = begin;
            Condition = cond;
            Step = step;
            Keyword = $"for({begin},{cond},{step})";
            EvaluateFunc = (x, y) => { Execute(this,x, y); return BuiltinValues.Null; };
        }
    }

    internal class ForeachFunctionValue : PreFunctionValue
    {
        public IExpr Collection { get; private set; }

        public string Vid { get; private set; }

        static void Execute(ForeachFunctionValue main, FunctionArgument args, EvalContext cal)
        {
            var children = args.Contents;
            try
            {
                CollectionValue l = OperationHelper.GetValue<CollectionValue>(cal.Evaluate(main.Collection));
                foreach (var t in l)
                {
                    cal.Variables.Set(main.Vid, t);
                    try
                    {
                        foreach (var v in children) cal.Evaluate(v);
                    }
                    catch (BContinue)
                    {
                        continue;
                    }
                }
            }
            catch (BBreak)
            {
                return;
            }
        }

        public ForeachFunctionValue(IExpr list,string vid) : base()
        {
            Collection = list;
            Vid = vid;
            Keyword = $"foreach({list},{vid})";
            EvaluateFunc = (x, y) => { Execute(this, x, y); return BuiltinValues.Null; };
        }
    }

    internal class WhileFunctionValue : PreFunctionValue
    {
        public IExpr Condition { get; private set; }

        static void Execute(WhileFunctionValue main, FunctionArgument args, EvalContext cal)
        {
            var children = args.Contents;
            try
            {
                while (OperationHelper.GetValue<bool>(cal.Evaluate(main.Condition)))
                {
                    try
                    {
                        foreach (var v in children) cal.Evaluate(v);
                    }
                    catch (BContinue)
                    {
                        continue;
                    }
                }
            }
            catch (BBreak)
            {
                return;
            }
        }

        public WhileFunctionValue(IExpr cond) : base()
        {
            Condition = cond;
            Keyword = $"while({cond})";
            EvaluateFunc = (x, y) => { Execute(this, x, y); return BuiltinValues.Null; };
        }
    }

    internal class DoWhileFunctionValue : PreFunctionValue
    {
        public IExpr Condition { get; private set; }

        static void Execute(DoWhileFunctionValue main, FunctionArgument args, EvalContext cal)
        {
            var children = args.Contents;
            try
            {
                do
                {
                    try
                    {
                        foreach (var v in children) cal.Evaluate(v);
                    }
                    catch (BContinue)
                    {
                        continue;
                    }
                }
                while (OperationHelper.GetValue<bool>(cal.Evaluate(main.Condition)));
            }
            catch (BBreak)
            {
                return;
            }
        }

        public DoWhileFunctionValue(IExpr cond) : base()
        {
            Condition = cond;
            Keyword = $"while({cond})";
            EvaluateFunc = (x, y) => { Execute(this, x, y); return BuiltinValues.Null; };
        }
    }

    internal class IfFunctionValue : PreFunctionValue
    {
        public IExpr Condition { get; private set; }

        static IExpr Execute(IfFunctionValue main, FunctionArgument args, EvalContext cal)
        {
            var children = args.Contents;
            if (OperationHelper.GetValue<bool>(cal.Evaluate(main.Condition)))
            {
                foreach (var v in children) cal.Evaluate(v);
                return BuiltinValues.Null;
            }
            else return new ElseFunctionValue(true, main);
        }

        public IfFunctionValue(IExpr cond) : base()
        {
            Condition = cond;
            Keyword = $"if({cond})";
            EvaluateFunc = (x, y) => Execute(this, x, y);;
        }
    }

    internal class ElseFunctionValue : PreFunctionValue
    {
        public IExpr PreExpr { get; private set; }

        public bool IsHappen { get; private set; }

        static void Execute(ElseFunctionValue main, FunctionArgument args, EvalContext cal)
        {
            if (main.IsHappen == false) return;
            var children = args.Contents;
            foreach (var v in children) cal.Evaluate(v);
        }

        public ElseFunctionValue(bool ishappen,IExpr pre=null) : base()
        {
            PreExpr = pre;
            IsHappen = ishappen;
            Keyword = $"{pre}else";
            EvaluateFunc = (x, y) => { Execute(this, x, y); return BuiltinValues.Null; };
        }
    }

    public class ControlStatements
    {
        public static PreFunctionValue For { get; } = new PreFunctionValue(
                "for",
                (FunctionArgument _args, EvalContext cal) =>
                {
                    var args = _args.Arguments;
                    OperationHelper.AssertArgsNumberThrowIf(3, args);
                    IExpr begin = args[0], cond = args[1], step = args[2];
                    return new ForFunctionValue(begin, cond, step);
                },
                3,true
                );

        public static PreFunctionValue ForEach { get; } = new PreFunctionValue(
            "foreach",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!(args[1] is VariableToken)) throw new Exceptions.EvaluateException("not a variable token.");
                return new ForeachFunctionValue(args[0],(args[1] as VariableToken).ID);
            },2, true
            );


        public static PreFunctionValue While { get; } = new PreFunctionValue(
            "while",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                return new WhileFunctionValue(args[0]);
            },1, true
            );

        public static PreFunctionValue DoWhile { get; } = new PreFunctionValue(
            "do",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                return new DoWhileFunctionValue(args[0]);
            }, 1, true
            );

        public static PreFunctionValue If { get; } = new PreFunctionValue(
            "if",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                return new IfFunctionValue(args[0]);
            }, 1, true
            );

        public static Operator Return { get; } = new Operator(
            "return",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                throw new BReturn(args[0]);
            },
            (IExpr[] args) => $"return {Operator.BlockToString(args[0])}",
            (double)Priority.High,
            Association.Right,
            1);

        public static Operator Break { get; } = new Operator(
            "break",
            (IExpr[] args, EvalContext cal) =>
            {
                throw new BBreak();
            },
            (IExpr[] args) => $"break",
            (double)Priority.High,
            Association.Right,
            0);

        public static Operator Continue { get; } = new Operator(
            "continue",
            (IExpr[] args, EvalContext cal) =>
            {
                throw new BContinue();
            },
            (IExpr[] args) => $"continue",
            (double)Priority.High,
            Association.Right,
            0);
    }
}
