using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state
{
    class Player
    {
        public bool Controlled;

        public Color PlayerColour;

        public Player(bool controlled, Color playerColour)
        {
            Controlled = controlled;
            PlayerColour = playerColour;
        }
    }
}
