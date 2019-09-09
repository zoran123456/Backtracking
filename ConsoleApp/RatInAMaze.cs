using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ConsoleApp
{
    public class RatInAMaze
    {
        string boardName;

        Point startPos;
        Point solutionPos;

        char[,] gameBoard;
        bool[,] solution;

        int boardWidth;
        int boardHeight;

        public RatInAMaze(string boardName)
        {
            this.boardName = boardName;
        }

        void InitBoard()
        {
            string boardPath = System.IO.Path.Combine("board", boardName);
            var boardFile = System.IO.File.ReadAllLines(boardPath);

            #region remove tabs from text file (happens due pasting from excel)

            for (var y = 0; y < boardFile.Length; y++)
            {
                string s = boardFile[y].Replace("\t", "");
                boardFile[y] = s;
            }

            #endregion

            boardWidth = boardFile[0].Length;
            boardHeight = boardFile.Length;

            gameBoard = new char[boardWidth, boardHeight];
            solution = new bool[boardWidth, boardHeight];

            for (var y = 0; y < boardHeight; y++)
            {
                var line = boardFile[y];
                for (var x = 0; x < line.Length; x++)
                {
                    char c = line[x];
                    if (c == '1') startPos = new Point(x, y);
                    else if (c == '2') solutionPos = new Point(x, y);

                    if (c == '1' || c == '2') c = '.';

                    gameBoard[x, y] = c;
                }
            }
        }

        bool IsValidPos(int x, int y)
        {

            return (x >= 0 && x < boardWidth &&
                    y >= 0 && y < boardHeight &&
                    gameBoard[x, y] == '.' &&
                    solution[x, y] == false
                    );

        }

        public string PrintSolution()
        {
            StringBuilder result = new StringBuilder();

            for (var y = 0; y < boardHeight; y++)
            {
                StringBuilder ln = new StringBuilder();

                for (var x = 0; x < boardWidth; x++)
                {
                    if (solution[x, y])
                        ln.Append("+");
                    else
                    {
                        if (gameBoard[x, y] == '.')
                            ln.Append(".");
                        else
                            ln.Append("x");
                    }
                    ln.Append("\t");
                }

                result.AppendLine(ln.ToString());
            }


            string solutionTextFile = System.IO.Path.Combine("board", "_solution.txt");
            System.IO.File.WriteAllText(solutionTextFile, result.ToString());

            return result.ToString();
        }

        public bool FindSolution()
        {
            InitBoard();

            // Initiate recursion
            bool succ = _SolutionBackTrack(startPos.X, startPos.Y);

            return succ;
        }

        bool _SolutionBackTrack(int x, int y)
        {
            // If we are on end position, end recursion
            // solution is found

            if (x == solutionPos.X && y == solutionPos.Y)
            {
                solution[x, y] = true;
                return true;
            }


            // If position is valid
            // X and Y are in the range of board and
            // position on X,Y is not part of the solution
            // latter one is *REALLY* important as without
            // it it would return stack overflow, as
            // endless loops would happen

            if (IsValidPos(x, y))
            {
                solution[x, y] = true;

                if (_SolutionBackTrack(x + 1, y))
                    return true;

                if (_SolutionBackTrack(x, y + 1))
                    return true;

                if (_SolutionBackTrack(x - 1, y))
                    return true;

                if (_SolutionBackTrack(x, y - 1))
                    return true;


                // Backtrack
                solution[x, y] = false;
                return false;
            }

            return false;

        }

    }
}
