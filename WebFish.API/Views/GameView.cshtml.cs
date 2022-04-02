using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebFish.API.Views
{
    public class GameViewViewModel
    {
        public string? ID { get; set; }
        public Board GameBoard { get; set; }

        public GameViewViewModel(string id = null)
        {
            this.ID = id;
            this.GameBoard = new Board();
        }

        public class Board
        {
            public Square[,] Squares { get; set; }
            public bool CellLabelsVisible { get; set; } = true;
            public int FullTurnCounter = 0;
            public int HalfTurnCounter = 0;
            public bool WhiteTurn = true;

            public Board()
            {
                Squares = new Square[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int ii = 0; ii < 8; ii++)
                    {
                        Squares[i, ii] = new Square(i, ii);
                    }
                }

                this.NewGame();
            }

            public void NewGame()
            {
                this.FullTurnCounter = 0;
                this.HalfTurnCounter = 0;
                this.WhiteTurn = true;

                this.SetPositionsFrom2dIntArray(this.StartPosition);
            }

            private int[,] StartPosition
            {
                get
                {
                    return new int[8, 8]
                    {
                        { 8, 9, 10, 11, 12, 10, 9, 8 },
                        { 7, 7, 7, 7, 7, 7, 7, 7 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 2, 3, 4, 5, 6, 4, 3, 2 }
                    };
                }
            }

            public int[,] GetBoardAsIntArray()
            {
                int[,] retVal = new int[8, 8];
                
                for (int i = 0; i < 8; i++)
                {
                    for (int ii = 0; ii < 8; ii++)
                    {
                        retVal[i, ii] = (this.Squares[i, ii].Piece != null ? (int)this.Squares[i, ii].Piece.PieceType : 0);
                    }
                }
                return retVal;
            }

            public string[,] GetBoardAsStringArray()
            {
                string[,] retVal = new string[8, 8];

                for (int i = 0; i < 8; i++)
                {
                    for (int ii = 0; ii < 8; ii++)
                    {
                        retVal[i, ii] = (this.Squares[i, ii].Piece != null ? this.Squares[i, ii].Piece.AsString() : null);
                    }
                }
                return retVal;
            }
                

            private void SetPositionsFrom2dIntArray(int[,] input)
            {
                for(int i=0; i < input.GetLength(0); i++)
                {
                    for(int ii = 0; ii < input.GetLength(1); ii++)
                    {
                        this.Squares[i, ii].Piece = this.PieceFromInt(input[i, ii]);
                    }
                }
            }

            private Piece PieceFromInt(int input)
            {
                return new Piece()
                {
                    PieceType = (ChessPieceTypes)input
                };
            }
        }

        public class Square
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Piece? Piece { get; set; }

            public bool IsDark
            {
                get
                {
                    return (X % 2 == 0) ? (Y % 2 == 0) : (Y % 2 == 1);
                }
            }

            public string Label
            {
                get
                {
                    return GetLabelFromCoords(this.X, this.Y);
                }
            }


            public bool IsOccuppied { get { return this.Piece != null; } }

            public Square(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        public class Piece
        { 
            public ChessPieceTypes? PieceType { get; set; }
            public string AsString()
            {

                if (this.PieceType.HasValue)
                {
                    switch (this.PieceType.Value)
                    {
                        case (ChessPieceTypes.None):
                            return null;

                        case (ChessPieceTypes.PawnWhite):
                            return "P";

                        case (ChessPieceTypes.RookWhite):
                            return "R";

                        case (ChessPieceTypes.KnightWhite):
                            return "N";

                        case (ChessPieceTypes.BishopWhite):
                            return "B";

                        case (ChessPieceTypes.QueenWhite):
                            return "Q";

                        case (ChessPieceTypes.KingWhite):
                            return "K";

                        case (ChessPieceTypes.PawnBlack):
                            return "p";

                        case (ChessPieceTypes.RookBlack):
                            return "r";

                        case (ChessPieceTypes.KnightBlack):
                            return "n";

                        case (ChessPieceTypes.BishopBlack):
                            return "b";

                        case (ChessPieceTypes.QueenBlack):
                            return "q";

                        case (ChessPieceTypes.KingBlack):
                            return "k";
                    }
                }

                return null;
            }

            public bool IsBlack
            {
                get
                {
                    return ((int)this.PieceType > 6);
                }
            }
        }


        public static string GetLabelFromCoords(int x, int y)
        {
            char rank = (char)(65 + y);
            return $"{rank.ToString().ToLower()}{8 - x}";
        }

        public enum ChessPieceTypes
        {
            None = 0,
            PawnWhite = 1,
            RookWhite = 2,
            KnightWhite = 3,
            BishopWhite = 4,
            QueenWhite = 5,
            KingWhite = 6,
            PawnBlack = 7,
            RookBlack = 8,
            KnightBlack = 9,
            BishopBlack = 10,
            QueenBlack = 11,
            KingBlack = 12
        }
    }
}
