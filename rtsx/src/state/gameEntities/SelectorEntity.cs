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
    class SelectorStart : GameEntity
    {
        public SelectorStart() : base(new Coordinate(0, 0))
        {
        }
    }

    class SelectorEnd : GameEntity
    {
        private SelectorStart Start;

        public SelectorEnd(SelectorStart start) : base(new Coordinate(0, 0))
        {
            Start = start;
        }

        public override void Draw(Drawer drawer)
        {
            drawer.DrawRectangle(Start.Location, this.Location, Color.Black, 2);
        }
    }

    class SelectorEntity : GameEntity
    {
        public SelectorEntity(SelectorStart start, SelectorEnd end) : base(CalculateSize(start.Location, end.Location))
        {
            Location = CalculateLocation(start.Location, end.Location);
        }

        public override void Draw(Drawer drawer)
        {
            var sizeHalved = Size * 0.5;
            drawer.FillRectangle(Location - sizeHalved, Location + sizeHalved, Color.Fuchsia);
        }

        private static Coordinate CalculateSize(Coordinate start, Coordinate end)
        {
            var xSize = Math.Abs(start.X - end.X);
            var ySize = Math.Abs(start.Y - end.Y);
            return new Coordinate(xSize, ySize);
        }

        private static Coordinate CalculateLocation(Coordinate start, Coordinate end)
        {
            var xLocation = (start.X + end.X) / 2;
            var yLocation = (start.Y + end.Y) / 2;
            return new Coordinate(xLocation, yLocation);
        }
    }
}
