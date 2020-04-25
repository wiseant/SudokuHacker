using System;

namespace SudokuHacker.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***数独Hacker***");
            while (true)
            {
                Console.WriteLine("\n请输入数独题目，每行数字之间以英文分号(;)分隔，空白格以空格或0表示\n(输入s4或s6或s9分别查看四宫格/六宫格/九宫格示例，按回车退出程序)");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    break;
                switch (input)
                {
                    case "s4":
                    case "S4":
                        input = "0310;1003;2 34;0020";
                        Console.WriteLine("演示解四宫格：\n" + input);
                        break;
                    case "s6":
                    case "S6":
                        var q6s = new[]{
                            "563104;200056;320401;016030;150603;6 4 12",
                            "060105;502000;000431;143000;000203;201050",
                        };
                        input = q6s[DateTime.Now.Second % q6s.Length];
                        Console.WriteLine("演示解六宫格：\n" + input);
                        break;
                    case "s9":
                    case "S9":
                        var q9s = new[]{
                            "800000000;003600000;070090200;050007000;000045700;000100030;001000068;008500010;090000400",
                            "5   9 2 1;  2  7  8; 8    3  ; 14  5   ;   9 3   ;   8  94 ;  3    6 ;6  2  1  ;8 9 6   5",
                        };
                        input = q9s[DateTime.Now.Second % q9s.Length];
                        Console.WriteLine("演示解标准九宫格：\n" + input);
                        break;
                }
                var ss = input.Split(';');
                var dimens = ss.Length;
                if (Array.IndexOf(new[] { 4, 6, 9 }, dimens) < 0)
                {
                    Console.WriteLine("只支持解四宫格/六宫格/九宫格，请按提示输入正确的题目。");
                    continue;
                }
                var puzzle = new int[dimens][];
                for (int r = 0; r < dimens; r++)
                {
                    var s = ss[r];
                    puzzle[r] = new int[dimens];
                    for (int c = 0; c < dimens; c++)
                    {
                        if (int.TryParse(s.Substring(c, 1), out int v))
                            puzzle[r][c] = v;
                    }
                }
                Solve(puzzle);
            }
            Console.WriteLine("游戏结束，再见！");
        }

        static void Solve(int[][] puzzle)
        {
            Console.WriteLine("题目：");
            Print(puzzle);
            var start = DateTime.Now;
            var solver = new Solver(puzzle);
            solver.Hack();
            var spent = (DateTime.Now - start).TotalMilliseconds;
            if (solver.Solved)
            {
                Console.WriteLine($"\n解答(用时{spent}ms)：");
                Print(solver.Solution);
            }
            else
            {
                Console.WriteLine("抱歉！我的智商该充电了，换道简单点的吧^_^");
            }
            //Console.WriteLine("\n解题步骤：");
            //Console.WriteLine(string.Join("\n", solver.Steps));
        }

        private static void Print(int[][] array)
        {
            var len = array.Length;
            for (int r = 0; r < len; r++)
            {
                var s = string.Empty;
                for (int c = 0; c < len; c++)
                {
                    s += array[r][c].ToString();
                }
                Console.WriteLine(s);
            }
        }

    }
}
