using rtsx.src.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state
{
    /// <summary>
    /// A class containing the information generated when detecting collisions
    /// </summary>
    class CollisionResult
    {
        public bool CollisionOccured { get; }

        public GameEntity A { get; }
        public GameEntity B { get; }

        public CollisionResult(bool collisionOccured, GameEntity a, GameEntity b)
        {
            CollisionOccured = collisionOccured;
            A = a;
            B = b;
        }
        
    }

    /// <summary>
    /// A class containing the information about a collision as presented to a
    /// GameEntity involved in the collision
    /// </summary>
    class CollisionInfo
    {
        public CollisionResult CollisionResult { get; }

        public GameEntity Self { get; }
        public GameEntity Other { get; }

        public CollisionInfo(CollisionResult collisionResult, GameEntity self)
        {
            CollisionResult = collisionResult;

            Self = self;

            if (CollisionResult.A == Self) { Other = CollisionResult.B; }
            else if (CollisionResult.B == Self) { Other = CollisionResult.A; }
            else { throw new RTSXException(); }
        }
    }
}
