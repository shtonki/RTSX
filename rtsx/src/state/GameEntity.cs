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

        private BoundingBox BoundingBox { get; }

        public GameEntity(Coordinate size)
        {
            Size = size;
            BoundingBox = new BoundingBox(Size.Clone());
        }

        public bool DBG;

        public virtual void Draw(Drawer drawer)
        {
            var sizeHalved = Size * 0.5;
            //drawer.FillRectangle(Location - sizeHalved, Location + sizeHalved, Color.Fuchsia);
            drawer.drawTextureR(GUI.A, Location - sizeHalved, 
                Location + sizeHalved, DBG ? Color.White : Color.Black);
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

        public static CollisionResult CheckCollision(GameEntity A, GameEntity B)
        {
            if (A.BoundingBox == null || B.BoundingBox == null)
            {
                return CollisionResult.NoCollision;
            }

            var xDistance = Math.Abs(A.X - B.X);
            var yDistance = Math.Abs(A.Y - B.Y);

            var totalBoundingBoxSize = A.BoundingBox.Size + B.BoundingBox.Size;

            var xOverlap = xDistance < totalBoundingBoxSize.X / 2;
            var yOverlap = yDistance < totalBoundingBoxSize.Y / 2;

            return new CollisionResult(xOverlap && yOverlap);
        }
    }
}
