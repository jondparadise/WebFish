using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFish.Engine.Helpers
{
    public static class FENHelper
    {
        private static string BaseFENStringFromBoardArray(string[][] board, bool whiteToMove = true)
        {
            if (board.Length != 8) throw new ArgumentException("Invalid board setup");

            StringBuilder sb = new StringBuilder();
            
            for(int i=0; i< board.Length; i++)
            {
                int incrementer = 0;
                if (board[i].Length != 8) throw new ArgumentException("Invalid board setup");

                for(int ii=0; ii < board[i].Length; ii++)
                {
                    if(ii < 7)
                    {
                        if (!string.IsNullOrWhiteSpace(board[i][ii]))
                        {
                            if (incrementer > 0)
                            {
                                sb.Append(incrementer);
                                incrementer = 0;
                            }
                            sb.Append(board[i][ii]);
                        }
                        else
                        {
                            incrementer++;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(board[i][ii]))
                        {
                            sb.Append(board[i][ii]);
                        }
                        else
                        {
                            incrementer++;
                            sb.Append(incrementer);
                        }
                    }
                }

                if(i < 7)
                {
                    sb.Append("/");
                }
                
            }

            sb.Append($" {(whiteToMove ? "w" : "b")} ");

            return sb.ToString();
        }

        public static string FENStringFromBoardArray(string[][] board, bool whiteToMove)
        {
            return $"{BaseFENStringFromBoardArray(board, whiteToMove)}";
        }

        public static string GetNewGameString()
        {
            var baseString = FENStringFromBoardArray(startPosition, true);
            return $"{baseString} KQkq - 0 1"; /* [Castlings available ('-' if none)] [en-passant target square  ('-' if none)] [Half moves] [Full moves]*/
        }

        private static string[][] startPosition
        {
            get
            {
                return new string[][]
                {
                   "rnbqkbnr".ToArray().Select(item => item.ToString()) .ToArray(),
                   "pppppppp".ToArray().Select(item => item.ToString()) .ToArray(),
                   "        ".ToArray().Select(item => item.ToString()) .ToArray(),
                   "        ".ToArray().Select(item => item.ToString()) .ToArray(),
                   "        ".ToArray().Select(item => item.ToString()) .ToArray(),
                   "        ".ToArray().Select(item => item.ToString()) .ToArray(),
                   "PPPPPPPP".ToArray().Select(item => item.ToString()) .ToArray(),
                   "RNBQKBNR".ToArray().Select(item => item.ToString()) .ToArray()
                };
            }
        }

        private static string[][] testPosition
        {
            get
            {
                return new string[][]
                {
                   "rnbqkbnr".ToArray().Select(item => item.ToString()) .ToArray(),
                   "p pp pp ".ToArray().Select(item => item.ToString()) .ToArray(),
                   "       p".ToArray().Select(item => item.ToString()) .ToArray(),
                   " p  p   ".ToArray().Select(item => item.ToString()) .ToArray(),
                   "   P P  ".ToArray().Select(item => item.ToString()) .ToArray(),
                   "       N".ToArray().Select(item => item.ToString()) .ToArray(),
                   "PPP P PP".ToArray().Select(item => item.ToString()) .ToArray(),
                   "RNBQKB R".ToArray().Select(item => item.ToString()) .ToArray()
                };
            }
        }

        public static string FENStringFromTestBoard()
        {
            return FENStringFromBoardArray(testPosition, false);
        }

        
        public static List<string> TestBoard(int iteration = 0)
        {
            switch (iteration)
            {
                case (1):
                    return ConsoleBoard(testPosition);

                default:
                    return ConsoleBoard(startPosition);
            }
        }

        public static List<string> ConsoleBoard(string[][] board)
        {
            List<string> retVal = new List<string>();
            for(int i=0; i< board.Length; i++)
            {
                retVal.Add(string.Join(string.Empty, board[i]));
            }
            return retVal;
        }
    }

}
