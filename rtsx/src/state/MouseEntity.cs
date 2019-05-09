using rtsx.src.util;
using rtsx.src.view;
using System.Collections.Generic;
using System.Linq;

namespace rtsx.src.state
{
    class MouseStateInfo
    {
        public IEnumerable<GameEntity> Picked { get; }
        public IEnumerable<GameEntity> Entered { get; }
        public IEnumerable<GameEntity> Left { get; }

        public MouseStateInfo(
            IEnumerable<GameEntity> picked, 
            IEnumerable<GameEntity> entered, 
            IEnumerable<GameEntity> left)
        {
            Picked = picked;
            Entered = entered;
            Left = left;
        }
    }

    class MouseEntity : GameEntity
    {
        private const double MouseSize = 0.0005;

        private List<GameEntity> Previous = new List<GameEntity>(); 
        private List<GameEntity> Current = new List<GameEntity>(); 

        public MouseStateInfo MouseStateInfo { get; private set; }

        public MouseEntity() : base(new Coordinate(MouseSize, MouseSize))
        {
            
        }

        public override void Draw(Drawer drawer)
        {
            // todo
        }

        protected override void Move()
        {
            var picked = Current;
            // entered/left work backwards from what my brain says but i can't figure
            // out why so now it's backwards and it's backwards here and only here
            var left = Current.Except(Previous);
            var entered = Previous.Except(Current);
            MouseStateInfo = new MouseStateInfo(picked, entered, left);

            // black magic
            // no racism intended nor implied
            var swap = Previous;
            Previous = Current;
            Current = swap;
            Current.Clear();

            Location = GUI.Window.MousePosition;
        }

        public override void HandleCollision(CollisionInfo collisionInfo)
        {
            base.HandleCollision(collisionInfo);

            Current.Add(collisionInfo.Other);
        }
    }
}
