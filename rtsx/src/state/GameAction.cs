using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state
{
    class GameAction
    {
        public GameActions Action { get; }

        public GameAction(GameActions action)
        {
            Action = action;
        }
    }

    enum GameActions
    {
        None,

        SelectStart,
        SelectEnd,

        RouteTo,
    }
}
