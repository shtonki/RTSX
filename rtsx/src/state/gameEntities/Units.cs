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
        private Color BackgroundColour;

        public Unit(UnitSize size, Color backgroundColour) : base(CalculateSize(size))
        {
            BackgroundColour = backgroundColour;
            Collidable = true;
        }

        public override void Draw(Drawer drawer)
        {
            var sizeHalved = Size * 0.5;
            drawer.DrawRectangle(Location - sizeHalved, Location + sizeHalved, Selected ? Color.Green : Color.White, 3);

            var backSize = sizeHalved * 0.7;
            drawer.FillRectangle(Location - backSize, Location + backSize, BackgroundColour);

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
        public DummyUnit() : base(UnitSize.Medium, Color.Pink)
        {
            Sprite = Sprites.Warrior;
        }
    }
}
