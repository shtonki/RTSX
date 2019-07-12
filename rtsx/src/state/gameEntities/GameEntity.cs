using rtsx.src.view;
using rtsx.src.util;
using System;
using System.Drawing;

namespace rtsx.src.state
{
    public enum EntitySize { Tiny, Small, Medium, };

    abstract class GameEntity : Drawable, Loggable
    {
        public const double GridSize = 0.025;

        public Coordinate Location { get; set; } = new Coordinate(0, 0);
        public double X => Location.X;
        public double Y => Location.Y;
        public Coordinate Size { get; set; } = new Coordinate(0.1, 0.1);

        public double LeftBounds
        {
            get { return Location.X - Size.X / 2; }
            set { Location = new Coordinate(value + Size.X / 2, Location.Y); }
        }
        public double RightBounds
        {
            get { return Location.X + Size.X / 2; }
            set { Location = new Coordinate(value - Size.X / 2, Location.Y); }
        }
        public double TopBounds
        {
            get { return Location.Y - Size.Y / 2; }
            set { Location = new Coordinate(Location.X, value + Size.Y / 2); }
        }
        public double BottomBounds
        {
            get { return Location.Y + Size.Y / 2; }
            set { Location = new Coordinate(Location.X, value - Size.Y / 2); }
        }

        public Coordinate MovementVector { get; protected set; }
        public Coordinate Destination { get; set; }
        public GameEntity Following { get; set; }
        public double MoveSpeed { get; set; } = 0.004;

        private BoundingBox BoundingBox { get; }
        public bool Collidable { get; set; }

        public bool Selected { get; set; }
        public bool Controllable { get; set; }

        public Sprites Sprite = Sprites.Hellspawn;
        public Color BrushColour { get; private set; } = Color.White;

        public GameEntity(Coordinate size)
        {
            Size = size;
            BoundingBox = new BoundingBox(Size.Clone());
        }

        public GameEntity(EntitySize size) : this(CalculateSize(size))
        {

        }

        public double DistanceTo(GameEntity other)
        {
            var distance = (Location - other.Location).Abs;
            return distance - BoundingBox.Size.X / 2 - other.BoundingBox.Size.X / 2;
        }

        public void Move(Coordinate movementVector)
        {
            MovementVector = movementVector;
            Location += movementVector;
        }

        public virtual void Draw(Drawer drawer)
        {
            var sizeHalved = Size * 0.5;
            drawer.DrawTexture(Sprite, Location - sizeHalved, 
                Location + sizeHalved, BrushColour);
        }

        public virtual void Step(GameState gameState)
        {
            gameState.RaiseGameEvent(new MoveEvent(this, DetermineMovement()));
        }

        protected virtual Coordinate DetermineMovement()
        {
            if (Following != null)
            {
                Destination = Following.Location;
            }

            if (Destination == null)
            {
                return Coordinate.ZeroVector;
            }

            var diff = Destination - Location;
            var angle = Math.Atan2(diff.Y, diff.X);

            if (diff.Abs < MoveSpeed)
            {
                Destination = null;
                return diff;
            }
            else
            {
                var xMovement = MoveSpeed * Math.Cos(angle);
                var yMovement = MoveSpeed * Math.Sin(angle);

                return new Coordinate(xMovement, yMovement);
            }
        }

        public virtual void HandleCollision(CollisionInfo collisionInfo)
        {
            var collisionResult = collisionInfo.CollisionResult;
            if (collisionResult.CollisionOccured)
            {
                var myNewLocation = collisionInfo.Self == collisionResult.A ? 
                    collisionResult.NewLocationA : collisionResult.NewLocationB;
                Location = myNewLocation;
            }
        }

        public virtual void RouteTo(Coordinate destination)
        {
            Destination = destination;
            Following = null;
        }

        public virtual void Follow(GameEntity followed)
        {
            Following = followed;
        }

        public static CollisionResult CheckCollision(GameEntity A, GameEntity B)
        {
            if (A.BoundingBox == null || B.BoundingBox == null)
            {
                return new CollisionResult(false, A, B);
            }

            var xDistance = Math.Abs(A.X - B.X);
            var yDistance = Math.Abs(A.Y - B.Y);

            var totalBoundingBoxSize = A.BoundingBox.Size + B.BoundingBox.Size;

            var xOverlap = totalBoundingBoxSize.X / 2 - xDistance;
            var yOverlap = totalBoundingBoxSize.Y / 2 - yDistance;

            // if we have detected a collision
            if (xOverlap > 0 && yOverlap > 0)
            {
                if (!A.Collidable || !B.Collidable)
                {
                    return new CollisionResult(true, A, B);
                }

                var previousCoordinatesA = A.Location - A.MovementVector;
                var previousCoordinatesB = B.Location - B.MovementVector;

                var leftEntity = 
                    previousCoordinatesA.X < previousCoordinatesB.X ? A : B;
                var rightEntity = leftEntity == A ? B : A;
                var topEntity =
                    previousCoordinatesA.Y < previousCoordinatesB.Y ? A : B;
                var bottomEntity = topEntity == A ? B : A;

                var xPrevOverlap = xOverlap - A.MovementVector.X - B.MovementVector.X;
                var yPrevOverlap = yOverlap - A.MovementVector.Y - B.MovementVector.Y;
                var xOverlapDiff = xOverlap - xPrevOverlap;
                var yOverlapDiff = yOverlap - yPrevOverlap;
                var xOverlapDiffPercentage = xOverlapDiff / xOverlap;
                var yOverlapDiffPercentage = yOverlapDiff / yOverlap;
                var xWiseCollision = Math.Abs(xOverlapDiffPercentage) > Math.Abs(yOverlapDiffPercentage);
                double ANewX;
                double ANewY;
                double BNewX;
                double BNewY;

                if (xWiseCollision)
                {
                    var rightBounds = rightEntity.X - rightEntity.Size.X / 2 - rightEntity.MovementVector.X;
                    var leftBounds = leftEntity.X + leftEntity.Size.X / 2 - leftEntity.MovementVector.X;
                    var gapX =  rightBounds - leftBounds;

                    // the percentage of the gap covered by the left entity
                    var middleX = leftEntity.MovementVector.X /
                        (leftEntity.MovementVector.X + rightEntity.MovementVector.X);

                    if (Double.IsNaN(middleX)) { middleX = 0.5; }
                    if (middleX > 1) { middleX = 1; }
                    if (middleX < 0) { middleX = 0; }

                    var leftNewX = leftEntity.X + gapX * middleX;
                    var rightNewX = rightEntity.X - gapX + gapX * middleX;

                    ANewX = leftEntity == A ? leftNewX : rightNewX;
                    ANewY = A.Y;
                    BNewX = leftEntity == B ? leftNewX : rightNewX;
                    BNewY = B.Y;
                }
                else
                {
                    var bottomBounds = bottomEntity.Y - bottomEntity.Size.Y / 2 - bottomEntity.MovementVector.Y;
                    var topBounds = topEntity.Y + topEntity.Size.Y / 2 - topEntity.MovementVector.Y;
                    var gapY = bottomBounds - topBounds;

                    // the percentage of the gap covered by the top entity
                    var middleY = topEntity.MovementVector.Y /
                        (topEntity.MovementVector.Y + bottomEntity.MovementVector.Y);

                    if (Double.IsNaN(middleY)) { middleY = 0.5; }
                    if (middleY > 1) { middleY = 1; }
                    if (middleY < 0) { middleY = 0; }

                    var topNewY = topEntity.Y + gapY * middleY;
                    var bottomNewY = bottomEntity.Y - gapY + gapY * middleY;

                    ANewX = A.X;
                    ANewY = topEntity == A ? topNewY : bottomNewY;
                    BNewX = B.X;
                    BNewY = topEntity == B ? topNewY : bottomNewY;
                }


                var ANewLocation = new Coordinate(ANewX, ANewY);
                var BNewLocation = new Coordinate(BNewX, BNewY);

                return new CollisionResult(true, A, B, ANewLocation, BNewLocation);
            }
            else
            {
                return new CollisionResult(false, A, B);
            }

        }

        protected static Coordinate CalculateSize(EntitySize size)
        {
            int multiplier;

            switch (size)
            {
                case EntitySize.Medium:
                    {
                        multiplier = 4;
                    }
                    break;

                case EntitySize.Small:
                    {
                        multiplier = 2;
                    }
                    break;
                case EntitySize.Tiny:
                    {
                        multiplier = 1;
                    }
                    break;

                default: throw new RTSXException();
            }

            return new Coordinate(GridSize * multiplier, GridSize * multiplier);
        }

        public string Log()
        {
            return String.Format($"X:{X} Y:{Y}");
        }
    }
}
