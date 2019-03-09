using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state
{
    class CollisionResult
    {
        public static CollisionResult NoCollision => new CollisionResult(false);

        public bool CollisionOccured { get; }

        public CollisionResult(bool collisionOccured)
        {
            CollisionOccured = collisionOccured;
        }
    }
}
