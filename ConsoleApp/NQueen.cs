using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class NQueenProblem
    {
        // Size of playing board (width and height are the same)
        const int BoardSize = 8;

        // Number of queens to place on a board
        const int QueenCount = 8;

        // Defines board as 2 dimensional boolean array
        // bool value determines if queen is placed on a position
        bool[,] board = new bool[BoardSize, BoardSize];

        /// <summary>
        /// Initializes playing board by setting all elements to false
        /// </summary>
        void InitBoard()
        {
            for (var x = 0; x < BoardSize; x++)
                for (var y = 0; y < BoardSize; y++)
                    board[x, y] = false;
        }

        /// <summary>
        /// Determines if position on X,Y is a valid to place Queen
        /// </summary>
        bool IsValidPosition(int x, int y)
        {
            // Queen must be placed on a position:
            // 1. no queen exist on a row
            // 2. no queen exist on a column
            // 3. no queen exist on a diagonals

            for (var i = 0; i < BoardSize; i++)
                if (board[i, y]) return false;

            for (var i = 0; i < BoardSize; i++)
                if (board[x, i]) return false;


            // See how for loop is handled
            // It is NOT the same as
            //
            //for (var pX =x; pX >=0; pX--)
            //    for (var pY=y; pY >=0; pY--)
            //
            // that code would no be valid


            // Top Left Diagonal
            for (int pX = x, pY = y;
                     pX >= 0 && pY >= 0;
                     pX--, pY--)
                if (board[pX, pY]) return false;

            // Bottom Left Diagonal
            for (int pX = x, pY = y;
                 pX >= 0 && pY < BoardSize;
                 pX--, pY++)
                if (board[pX, pY]) return false;


            return true;
        }

        /// <summary>
        /// Counts number of queens on a board
        /// </summary>
        int CountQueens()
        {
            int count = 0;

            for (var x = 0; x < BoardSize; x++)
                for (var y = 0; y < BoardSize; y++)
                    if (board[x, y]) count++;

            return count;
        }

        /// <summary>
        /// Displays playing board on a Console
        /// </summary>
        public void DisplayBoard()
        {
            Console.WriteLine();

            StringBuilder ln = new StringBuilder();

            for (var y = 0; y < BoardSize; y++)
            {
                ln.Clear();

                for (var x = 0; x < BoardSize; x++)
                {
                    char c = (board[x, y]) ? 'x' : ' ';

                    ln.Append($"[{c}]");
                }

                Console.WriteLine(ln);
            }


            Console.WriteLine();
            Console.WriteLine();
        }


        /// <summary>
        /// Tries to solve board
        /// </summary>
        public bool Solve()
        {
            
            // Set all fields in a board to 0
            InitBoard();

            // Run method recursively
            var ok = PlaceQueen_Backtrack(0);

            return ok;
        }

        /// <summary>
        /// Recursively and with backtracking tries to place queen on a next position
        /// </summary>
        /// <param name="x">X position of a queen, always run this method with 0</param>
        bool PlaceQueen_Backtrack(int x)
        {
            // Check if we have completed game board
            if (CountQueens() >= QueenCount)
                return true;


            // Board is not complete, proceed to next step ...


            for (var pY = 0; pY < BoardSize; pY++)
            {
                // Check if this is valid position to place queen
                // If it is, place it and invoke this method again
                if (IsValidPosition(x, pY))
                {
                    board[x, pY] = true;


                    // Recursivelly call this method again with the next X position
                    if (PlaceQueen_Backtrack(x + 1))
                        return true;


                    // backtrack when we receive false as a result
                    board[x, pY] = false;
                }
            }

            // this position is not valid, notify the calling method so it can backtrack
            return false;
        }

    }


}
