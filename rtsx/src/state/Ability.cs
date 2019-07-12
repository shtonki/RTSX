using rtsx.src.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state
{
    abstract class Ability
    {
        public Sprites Sprite { get; private set; }

        protected Ability(Sprites sprite)
        {
            Sprite = sprite;
        }
    }

    class ActivatedAbility
    {

    }
}
