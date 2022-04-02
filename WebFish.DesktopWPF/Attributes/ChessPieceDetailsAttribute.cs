using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFish.DesktopWPF.Attributes
{
    public class ChessPieceDetailsAttribute : Attribute
    {
        public string Name { get; set; }
        public string FENAbbreviation { get; set; }
        public ChessPieceDetailsAttribute(string name, string fen = null) 
        {
            this.Name = name;
            this.FENAbbreviation = fen;
        }
    }
}
