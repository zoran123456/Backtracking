using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class SudokuGenerator
    {
        // Size of a Sudoku board (width and height are the same)
        const byte BoardSize = 9;

        // Size of Sudoku square (Sudoku board is divided into 3x3 squares)
        const byte SquareSize = 3;

        // Determines number of digits to place on a board
        const int NumbersToPlace = BoardSize * BoardSize;

        // Game Board, where numbers 1-9 (actually BoardSize) will be stored
        byte[,] board = new byte[BoardSize, BoardSize];


        /// <summary>
        /// Initializes board by setting all elements to 0
        /// </summary>
        void InitBoard()
        {
            for (var x = 0; x < BoardSize; x++)
                for (var y = 0; y < BoardSize; y++)
                    board[x, y] = 0;
        }

        /// <summary>
        /// Checks if board contains empty elements
        /// </summary>
        /// <returns></returns>
        bool BoardComplete()
        {
            int count = 0;
            for (var x = 0; x < BoardSize; x++)
                for (var y = 0; y < BoardSize; y++)
                    if (board[x, y] > 0) count++;

            return count == NumbersToPlace;
        }

        /// <summary>
        /// Checks if position X,Y is valid to place number
        /// </summary>
        /// <param name="x">X position on game board</param>
        /// <param name="x">Y position on game board</param>
        /// <param name="y">Number that is to be placed</param>
        /// <returns></returns>
        bool ValidPosition(byte num, byte x, byte y)
        {

            // Invalid as position is occupied with another number
            if (board[x, y] != 0)
                return false;

            // Check if there is the same number in a row
            for (var pX = 0; pX < BoardSize; pX++)
                if (board[pX, y] == num) return false;

            // Check if there is the same number in a column
            for (var pY = 0; pY < BoardSize; pY++)
                if (board[x, pY] == num) return false;

            // Check if there is the same number in a square
            byte sqX = (byte)(x - x % SquareSize);
            byte sqY = (byte)(y - y % SquareSize);

            for (var pX = sqX; pX < sqX + SquareSize; pX++)
                for (var pY = sqY; pY < sqY + SquareSize; pY++)
                    if (board[pX, pY] == num) return false;

            return true;
        }

        /// <summary>
        /// Displays game board on a Console
        /// </summary>
        public void DisplayBoard()
        {
            Console.WriteLine();

            StringBuilder bldr = new StringBuilder();

            for (var y = 0; y < BoardSize; y++)
            {
                bldr.Clear();

                for (var x = 0; x < BoardSize; x++)
                {
                    string digit = board[x, y] == 0 ? " " : board[x, y].ToString();
                    bldr.Append($"[{digit}]");

                    if ((x + 1) % SquareSize == 0)
                        bldr.Append(" ");
                }

                Console.WriteLine(bldr);

                if ((y + 1) % SquareSize == 0)
                    Console.WriteLine("");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Tries to generate Sudoku board
        /// </summary>
        public bool Generate()
        {
            // Initialize game board by setting all elements to 0
            InitBoard();

            // Add fixed digits, good place for some randomization
            board[0, 0] = 3;
            board[1, 0] = 8;
            board[2, 0] = 5;

            board[3, 4] = 3;
            board[4, 4] = 8;
            board[5, 4] = 5;

            board[6, 8] = 3;
            board[7, 8] = 8;
            board[8, 8] = 5;



            // Place all numbers by recursively call this method
            return PlaceNumber_Backtrack();
        }

        /// <summary>
        /// Searches in a board for a first valid position to place digit
        /// </summary>
        bool GetFirstEmptyPosition(out byte x, out byte y)
        {
            for (byte pX = 0; pX < BoardSize; pX++)
            {
                for (byte pY = 0; pY < BoardSize; pY++)
                {
                    if (board[pX, pY] == 0)
                    {
                        x = pX;
                        y = pY;
                        return true;
                    }
                }
            }

            // If no empty position exists, set X,Y to some dummy value
            x = 99;
            y = 99;

            return false;
        }

        /// <summary>
        /// Recursively and with backtracking tries to place digit on a next position
        /// </summary>
        bool PlaceNumber_Backtrack()
        {
            // Checks if board is complete
            // If true, end recursion and return true
            if (BoardComplete())
                return true;

            // Board is not complete, proceed to next step ...

            // Get first available position to place digit
            byte x, y;
            bool foundEmptyPosition = GetFirstEmptyPosition(out x, out y);

            // If not empty position exists, assume board is empty
            // This is just for security checking, method should always return true
            if (!foundEmptyPosition)
                return true;

            // Try to place any of the digits to a X,Y position
            for (byte i = 1; i <= BoardSize; i++)
            {
                // Checks if position is valid to place digit
                if (ValidPosition(i, x, y))
                {
                    // Place digit to a position
                    board[x, y] = i;

                    // proceed to place next digit, until board is complete
                    if (PlaceNumber_Backtrack())
                        return true;

                    // If placing digit failed, backtrack
                    board[x, y] = 0;
                }
            }

            // No placing possible, return false
            return false;
        }

    }
}
