using System;
using Xunit;
using Xunit.Abstractions;

namespace SudokuHacker.Test
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        [Fact]
        public void Test4_1()
        {
            var puzzle = new int[4][]
            {
                new int[4] {0, 3, 1, 0 },
                new int[4] {1, 0, 0, 3 },
                new int[4] {2, 0, 3, 4 },
                new int[4] {0, 4, 2, 0 },
            };
            output.WriteLine("题目：");
            Print(puzzle);
            var solver = new Solver(puzzle);
            solver.Hack();
            Assert.True(solver.Solved);
            output.WriteLine("\n解答：");
            Print(solver.Solution);
        }

        [Fact]
        public void Test4_2()
        {
            var puzzle = new int[4][]
            {
                new int[4] {0, 3, 1, 0 },
                new int[4] {1, 0, 0, 3 },
                new int[4] {2, 0, 3, 4 },
                new int[4] {0, 0, 0, 0 },
            };
            output.WriteLine("题目：");
            Print(puzzle);
            var solver = new Solver(puzzle);
            solver.Hack();
            Assert.True(solver.Solved);
            output.WriteLine("\n解答：");
            Print(solver.Solution);
        }

        [Fact]
        public void Test4_3()
        {
            var puzzle = new int[4][]
            {
                new int[4] {0, 3, 1, 0 },
                new int[4] {0, 0, 0, 3 },
                new int[4] {0, 0, 0, 0 },
                new int[4] {0, 0, 0, 0 },
            };
            output.WriteLine("题目：");
            Print(puzzle);
            var solver = new Solver(puzzle);
            solver.Hack();
            Assert.True(solver.Solved);
            output.WriteLine("\n解答：");
            Print(solver.Solution);
        }

        [Fact]
        public void Test9_1()
        {
            var puzzle = new int[9][]
            {
                new int[9] {0,5,0,0,0,1,6,9,3},
                new int[9] {9,3,7,0,2,8,0,0,1},
                new int[9] {0,1,0,0,0,0,0,7,8},
                new int[9] {2,0,1,0,0,0,0,4,0},
                new int[9] {7,8,0,0,0,2,3,0,6},
                new int[9] {0,0,0,0,0,5,8,0,9},
                new int[9] {5,0,9,2,6,0,0,0,4},
                new int[9] {0,0,0,0,9,4,5,0,0},
                new int[9] {1,0,6,5,0,0,9,0,0},
            };
            output.WriteLine("题目：");
            Print(puzzle);
            var solver = new Solver(puzzle);
            solver.Hack();
            Assert.True(solver.Solved);
            output.WriteLine("\n解答：");
            Print(solver.Solution);
        }

        [Fact]
        public void Test9_2()
        {
            var puzzle = new int[9][]
            {
                new int[9]{0,4,6,9,0,3,0,0,0},
                new int[9]{0,0,3,0,5,0,0,6,0},
                new int[9]{9,0,0,0,0,2,0,0,3},
                new int[9]{0,0,5,0,0,6,0,0,0},
                new int[9]{8,0,0,0,0,0,0,1,0},
                new int[9]{0,1,0,7,8,0,2,0,0},
                new int[9]{0,0,0,0,0,0,0,5,0},
                new int[9]{0,8,1,3,0,0,0,0,7},
                new int[9]{0,0,0,8,0,0,1,0,4},
            };
            output.WriteLine("题目：");
            Print(puzzle);
            var solver = new Solver(puzzle);
            solver.Hack();
            Assert.True(solver.Solved);
            output.WriteLine("\n解答：");
            Print(solver.Solution);
        }

        [Fact]
        public void Test9_3()
        {
            var puzzle = new int[9][]
            {
                new int[]{8,0,0,0,0,0,0,0,0},
                new int[]{0,0,3,6,0,0,0,0,0},
                new int[]{0,7,0,0,9,0,2,0,0},
                new int[]{0,5,0,0,0,7,0,0,0},
                new int[]{0,0,0,0,4,5,7,0,0},
                new int[]{0,0,0,1,0,0,0,3,0},
                new int[]{0,0,1,0,0,0,0,6,8},
                new int[]{0,0,8,5,0,0,0,1,0},
                new int[]{0,9,0,0,0,0,4,0,0},
            };
            output.WriteLine("题目：");
            Print(puzzle);
            var solver = new Solver(puzzle);
            solver.Hack();
            Assert.True(solver.Solved);
            output.WriteLine("\n解答：");
            Print(solver.Solution);
        }

        private void Print(int[][] array)
        {
            var len = array.Length;
            for (int r = 0; r < len; r++)
            {
                var s = string.Empty;
                for (int c = 0; c < len; c++)
                {
                    s += array[r][c].ToString();
                }
                output.WriteLine(s);
            }
        }

    }
}
