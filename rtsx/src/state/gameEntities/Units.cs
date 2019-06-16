using rtsx.src.util;
using rtsx.src.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state.gameEntities
{
    public enum UnitSize { Medium, };


    abstract class Unit : GameEntity
    {
        public Player Owner { get; }

        public Unit(UnitSize size, Player owner) : base(CalculateSize(size))
        {
            Owner = owner;
            Collidable = true;
            Controllable = owner.Controlled;
        }

        public override void Draw(Drawer drawer)
        {
            var sizeHalved = Size * 0.5;
            drawer.DrawRectangle(Location - sizeHalved, Location + sizeHalved, Selected ? Color.Green : Color.White, 3);

            var backSize = sizeHalved * 0.7;
            drawer.FillRectangle(Location - backSize, Location + backSize, Owner.PlayerColour);

            base.Draw(drawer);
        }

        private static Coordinate CalculateSize(UnitSize size)
        {
            int multiplier;

            switch (size)
            {
                case UnitSize.Medium:
                    {
                        multiplier = 4;
                    } break;

                default: throw new RTSXException();
            }

            return new Coordinate(GridSize * multiplier, GridSize * multiplier);
        }
    }

    class DummyUnit : Unit
    {
        public DummyUnit(Player owner) : base(UnitSize.Medium, owner)
        {
            Sprite = Sprites.Warrior;
        }
    }
}
