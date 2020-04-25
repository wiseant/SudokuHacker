using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SudokuHacker
{
    public class Solver
    {
        private bool solved;
        private readonly int[][] puzzle, solution;
        private readonly int dimens, horizontalBlockCount, verticalBlockCount;
        private readonly int[] numbers;
        private readonly bool recordSteps;
        private List<string> steps = new List<string>();

        public bool Solved => solved;

        public int[][] Solution
        {
            get
            {
                if (solved)
                    return solution;
                return null;
            }
        }

        public IReadOnlyList<string> Steps => steps.AsReadOnly();

        public Solver(int[][] puzzle, bool recordSteps=false)
        {
            this.puzzle = puzzle ?? throw new ArgumentNullException(nameof(puzzle));
            this.recordSteps = recordSteps;
            this.dimens = puzzle.Length;
            this.solution = new int[dimens][];
            this.numbers = new int[dimens];
            int i = 0;
            while ( i < dimens) {
                numbers[i] = ++i;
            }
            switch (dimens)
            {
                case 4:
                    horizontalBlockCount = 2;
                    verticalBlockCount = 2;
                    break;
                case 6:
                    horizontalBlockCount = 2;
                    verticalBlockCount = 3;
                    break;
                case 9:
                    horizontalBlockCount = 3;
                    verticalBlockCount = 3;
                    break;
                default:
                    throw new Exception("只支持解4宫格、6宫格、9宫格");
            }
        }

        private static void Copy(int[][] source, int[][] target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            int dimens = source.Length;
            for (int i=0; i < dimens; i++)
            {
                target[i] = new int[dimens];
                Array.Copy(source[i], target[i], dimens);
            }
        }

        private static int[][] Copy(int[][] source)
        {
            int[][] target = new int[source.Length][];
            Copy(source, target);
            return target;
        }

        public void Hack()
        {
            try
            {
                var alternatives = Guess(puzzle);
                var alternativeCountPerPoint = GetAlternativeCountByPoint(alternatives).OrderBy(p=>p.Value).ToArray();
                while (alternativeCountPerPoint.Length > 0)
                {
                    var decidings = alternativeCountPerPoint.Where(p => p.Value == 1).ToArray();
                    if (decidings.Length == 0) //没有可以推断出唯一值的格子
                    {
                        var point = alternativeCountPerPoint[0].Key;
                        var nums = alternatives[point.X, point.Y];
                        foreach (var num in nums)
                        {
                            var puzzle2 = Copy(puzzle);
                            puzzle2[point.X][point.Y] = num;
                            var solver = new Solver(puzzle2);
                            solver.Hack();
                            steps.AddRange(solver.Steps); //不管是否解出答案都把步骤记下来
                            if (solver.Solved)
                            {
                                Copy(solver.Solution, solution);
                                solved = true;
                                return;
                            }
                        }
                        //steps.Add($"当运算到格子 [{point.X + 1},{point.Y + 1}] 时，在所有候选值{string.Join(",", nums.Select(n=>n.ToString()).ToArray())}中都无法找到合适的解");
                        return;
                    }
                    else
                    {
                        foreach (var deciding in decidings)
                        {
                            puzzle[deciding.Key.X][deciding.Key.Y] = alternatives[deciding.Key.X, deciding.Key.Y][0];
                        }
                    }
                    alternatives = Guess(puzzle);
                    alternativeCountPerPoint = GetAlternativeCountByPoint(alternatives).OrderBy(p=>p.Value).ToArray();
                }
                Copy(puzzle, solution);
                solved = true;
            }
            catch(UnsolvableException ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 计算每一个格子的备选值个数
        /// </summary>
        /// <param name="alternatives"></param>
        /// <returns></returns>
        private Dictionary<Point, int> GetAlternativeCountByPoint(int[,][] alternatives)
        {
            var dict = new Dictionary<Point, int>();
            for (int row = 0; row < dimens; row++)
            {
                for (int col = 0; col < dimens; col++)
                {
                    var values = alternatives[row, col];
                    if (values == null)
                        continue;
                    dict.Add(new Point(row, col), values.Length);
                }
            }
            return dict;
        }

        private int[,][] Guess(int[][] puzzle)
        {
            var alternatives = new int[dimens, dimens][];
            for(int row=0;row< dimens; row++)
            {
                for(int col=0; col< dimens; col++)
                {
                    int value = puzzle[row][col];
                    if (value > 0)
                    {
                        continue;
                    }
                    var eliminations = new List<int>();
                    var exists = new List<int>();
                    //计算当前格子所处的3x3方块区域内已确定的数
                    var blockOfRow = (int)Math.Floor(1.0m*row / horizontalBlockCount);
                    var blockOfCol = (int)Math.Floor(1.0m*col / verticalBlockCount);
                    for(int r=blockOfRow*horizontalBlockCount; r<(blockOfRow+1)*horizontalBlockCount; r++)
                    {
                        for(int c=blockOfCol*verticalBlockCount; c<(blockOfCol+1)*verticalBlockCount; c++)
                        {
                            int val = puzzle[r][c];
                            if (val > 0)
                                exists.Add(val);
                        }
                    }
                    eliminations.AddRange(exists);
                    var reduplicateGroups = exists.GroupBy(p => p).Where(g => g.Count() > 1);
                    if (reduplicateGroups.Count()>0)
                    {
                        var msg = $"当前运算到的格子 [{row + 1},{col + 1}] 在所处方块区域内存在重复数字：{reduplicateGroups.First().Key}";
                        steps.Add(msg);
                        throw new UnsolvableException(msg);
                    }

                    //计算当前格子所处的整行中已确定的数
                    exists.Clear();
                    for(int i=0; i < dimens; i++)
                    {
                        int val = puzzle[row][i];
                        if (val > 0)
                            exists.Add(val);
                    }
                    eliminations.AddRange(exists);
                    reduplicateGroups = exists.GroupBy(p => p).Where(g => g.Count() > 1);
                    if (reduplicateGroups.Count() > 0)
                    {
                        var msg = $"当前运算到的格子 [{row + 1},{col + 1}] 所处的行内存在重复数字：{reduplicateGroups.First().Key}";
                        steps.Add(msg);
                        throw new UnsolvableException(msg);
                    }

                    //计算当前格子所处的整行中已确定的数
                    exists.Clear();
                    for(int i=0; i < dimens; i++)
                    {
                        int val = puzzle[i][col];
                        if (val > 0)
                            exists.Add(val);
                    }
                    eliminations.AddRange(exists);
                    reduplicateGroups = exists.GroupBy(p => p).Where(g => g.Count() > 1);
                    if (reduplicateGroups.Count() > 0)
                    {
                        var msg = $"当前运算到的格子 [{row + 1},{col + 1}] 所处的列内存在重复数字：{reduplicateGroups.First().Key}";
                        steps.Add(msg);
                        throw new UnsolvableException(msg);
                    }

                    alternatives[row, col] = numbers.Where(p => !eliminations.Any(e => e == p)).ToArray();
                }
            }

            return alternatives;
        }
    }
}
