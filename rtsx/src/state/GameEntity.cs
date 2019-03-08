using rtsx.src.view;
using rtsx.src.util;
using System;
using System.Drawing;

namespace rtsx.src.state
{
    class GameEntity : Drawable
    {
        public Coordinate Location { get; protected set; } = new Coordinate(0, 0);
        public Coordinate Size { get; protected set; } = new Coordinate(0.1, 0.1);
        public double X => Location.X;
        public double Y => Location.Y;
        public Coordinate MoveTo { get; set; }
        public double MoveSpeed { get; protected set; } = 0.01;

        public GameEntity()
        {
        }

        public GameEntity(Coordinate size)
        {
            Size = size;
        }

        public virtual void Draw(Drawer drawer)
        {
            var sizeHalved = Size * 0.5;
            drawer.FillRectangle(Location - sizeHalved, Location + sizeHalved, Color.Fuchsia);
        }

        public void Step()
        {
            Move();
        }

        protected virtual void Move()
        {
            if (MoveTo == null) { return; }

            var diff = MoveTo - Location;
            var angle = Math.Atan2(diff.Y, diff.X);

            Coordinate movementCoordinate;

            if (diff.Abs < MoveSpeed)
            {
                MoveTo = null;
                movementCoordinate = diff;
            }
            else
            {
                var xMovement = MoveSpeed * Math.Sin(angle);
                var yMovement = MoveSpeed * Math.Cos(angle);

                movementCoordinate = new Coordinate(xMovement, yMovement);
            }

            Location += movementCoordinate;
        }
    }
}
