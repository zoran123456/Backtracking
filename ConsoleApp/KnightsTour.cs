using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class KnightsTour
    {
        // Board Size (width and height are equal), can me any number from 5 to 50
        const int BoardSize = 8;

        // Indicates maximum number of moves, for a complete trip maximum moves are width*height
        const int MovesMax = BoardSize * BoardSize;

        // Board where knight moves will be written to
        int[,] board = new int[BoardSize, BoardSize];

        // Defines 8 possible moves for a knight (eg. move 2X and 1Y, move -1X and 2Y, ...)
        int[] mx = { 2, 1, -1, -2, -2, -1, 1, 2 };
        int[] my = { 1, 2, 2, 1, -1, -2, -2, -1 };

        // Set some randomization, so that we get random moves each time
        Random rnd = new Random();

        // Internal helper POCO object, used to sort optimal next position
        // Used in combination with GetWarnsdorffMoves method
        class MoveIndexPossibleMoves
        {
            public int Index { get; set; }
            public int Moves { get; set; }
        }

        /// <summary>
        /// Initializes 2D game board by setting each field value to 0
        /// </summary>
        void InitBoard()
        {
            for (var x = 0; x < BoardSize; x++)
                for (var y = 0; y < BoardSize; y++)
                    board[x, y] = 0;
        }

        /// <summary>
        /// Determines whether game board field on X,Y is valid place for a knight move
        /// </summary>
        bool IsValidPosition(int x, int y)
        {
            return (x >= 0 && x < BoardSize &&
                    y >= 0 && y < BoardSize &&
                    board[x, y] == 0);
        }

        /// <summary>
        /// Returns number of valid knight moves on a X,Y position
        /// </summary>
        int PossibleMovesFromPosition(int pX, int pY)
        {
            int count = 0;

            for (var i = 0; i < 8; i++)
            {
                int x = pX + mx[i];
                int y = pY + my[i];
                if (IsValidPosition(x, y))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Gets next valid knight positions from the X,Y position, and sorts them by possible moves count
        /// </summary>
        List<MoveIndexPossibleMoves> GetWarnsdorffMoves(int pX, int pY)
        {
            // Read more about Warnsdorff's rule
            // https://www.wikiwand.com/en/Knight%27s_tour


            List<MoveIndexPossibleMoves> moves = new List<MoveIndexPossibleMoves>();

            for (var i = 0; i < 8; i++)
            {
                int x = pX + mx[i];
                int y = pY + my[i];

                int moveCount = PossibleMovesFromPosition(x, y);

                var mipm = new MoveIndexPossibleMoves() { Index = i, Moves = moveCount };
                moves.Add(mipm);
            }

            return moves.OrderBy(e => e.Moves).ThenBy(i => rnd.Next()).ToList();
        }

        /// <summary>
        /// Displays board solution on a Console
        /// </summary>
        public void DisplaySolution(Stopwatch sw)
        {
            StringBuilder ln = new StringBuilder();

            string[] columns = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
            string footer = "\n    ";
            for (var i = 0; i < BoardSize; i++)
                footer += $"[{columns[i]} ]";


            Console.WriteLine("");
            Console.WriteLine($"Elapsed: {sw.Elapsed.TotalSeconds} seconds");
            Console.WriteLine("");

            string padding = (MovesMax > 99) ? "D3" : "D2";

            for (var y = 0; y < BoardSize; y++)
            {
                ln.Clear();
                ln.Append($"{(y + 1):D2}  ");

                for (var x = 0; x < BoardSize; x++)
                {
                    int num = board[x, y];
                    string res = (num > 0) ? num.ToString(padding) : "  ";

                    ln.Append($"[{res}]");
                }

                Console.WriteLine(ln);
            }

            Console.WriteLine(footer);
        }

        /// <summary>
        /// Tries to solve board
        /// </summary>
        /// <param name="x">X position of a knight</param>
        /// <param name="y">Y position of a knight</param>
        /// <returns></returns>
        public bool Solve(int x, int y)
        {
            // Set all fields in a board to 0
            InitBoard();

            // Run method recursively
            if (!Solve_Backtrack(x, y, 1))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Recursively and with backtracking tries to place knight on a next position
        /// </summary>
        /// <param name="x">X position of a knight</param>
        /// <param name="y">Y position of a knight</param>
        /// <param name="num">Knight position number</param>
        /// <returns></returns>
        bool Solve_Backtrack(int x, int y, int num)
        {
            // Check if we have completed game board
            if (num > MovesMax)
                return true;


            // Board is not complete, proceed to next step ...


            // Check if this is valid position to place knight
            // If it is, place it and invoke this method again
            if (IsValidPosition(x, y))
            {

                board[x, y] = num;

                // HUGE, massive, crazy speed improvement
                // Read more on Warnsdorff rule
                // https://www.wikiwand.com/en/Knight%27s_tour

                bool useWarnsdorffRules = true;

                List<MoveIndexPossibleMoves> moves = null;
                if (useWarnsdorffRules)
                {
                    moves = GetWarnsdorffMoves(x, y);
                }

                // Try to place next knight move on the first valid position
                // We have 8 possible knight moves (imagine rotating Tetris "L" block)
                for (var i = 0; i < 8; i++)
                {
                    int newX;
                    int newY;

                    if (useWarnsdorffRules)
                    {
                        newX = x + mx[moves[i].Index];
                        newY = y + my[moves[i].Index];
                    }
                    else
                    {
                        newX = x + mx[i];
                        newY = y + my[i];
                    }

                    // Recursivelly call this method again with the next X, Y positions
                    if (Solve_Backtrack(newX, newY, num + 1))
                        return true;
                }

                // backtrack when we receive false as a result
                board[x, y] = 0;

            }

            // this position is not valid, notify the calling method so it can backtrack
            return false;
        }

    }
}
