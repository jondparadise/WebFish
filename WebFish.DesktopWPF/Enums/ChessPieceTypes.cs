using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFish.DesktopWPF.Attributes;

namespace WebFish.DesktopWPF.Enums
{
    enum ChessPieceTypes
    {
        [ChessPieceDetails("Empty")]
        Empty,
        [ChessPieceDetails("WhitePawn", "P")]
        WhitePawn,
        [ChessPieceDetails("WhiteRook", "R")]
        WhiteRook,
        [ChessPieceDetails("WhiteKnight", "N")]
        WhiteKnight,
        [ChessPieceDetails("WhiteBishop", "B")]
        WhiteBishop,
        [ChessPieceDetails("WhiteQueen", "Q")]
        WhiteQueen,
        [ChessPieceDetails("WhiteKing", "K")]
        WhiteKing,
        [ChessPieceDetails("BlackPawn", "p")]
        BlackPawn,
        [ChessPieceDetails("BlackRook", "r")]
        BlackRook,
        [ChessPieceDetails("BlackKnight", "n")]
        BlackKnight,
        [ChessPieceDetails("BlackBishop", "b")]
        BlackBishop,
        [ChessPieceDetails("BlackQueen", "q")]
        BlackQueen,
        [ChessPieceDetails("BlackKing", "k")]
        BlackKing
    }
}
