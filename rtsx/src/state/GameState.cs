using rtsx.src.util;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;

namespace rtsx.src.state
{
    class GameState
    {
        private List<GameEntity> EntityList { get; } = new List<GameEntity>();
        public IEnumerable<GameEntity> Entities => EntityList;

        public MouseEntity MouseEntity { get; } = new MouseEntity();

        private Timer StepTimer;
        private const double StepsPerSecond = 100;

        public GameState()
        {
            StepTimer = new Timer();
            StepTimer.Interval = StepsPerSecond/60;
            StepTimer.Elapsed += (_, __) => Step();
            StepTimer.Start();

            EntityList.Add(MouseEntity);
        }

        public void AddEntity(GameEntity gameEntity)
        {
            if (EntityList.Contains(gameEntity))
            {
                Logging.Log("Tried to add duplicate GameEntity to GameState");
                return;
            }

            if (gameEntity is MouseEntity)
            {
                // don't allow future clown self to add another MouseEntity
                throw new RTSXException();
            }

            EntityList.Add(gameEntity);
        }

        private void Step()
        {
            foreach (var entity in Entities)
            {
                entity.Step();
            }

            DetectCollisions();

            HandleMouseState();
        }

        private void HandleMouseState()
        {
            var MouseStateInfo = MouseEntity.MouseStateInfo;

            if (MouseStateInfo == null) { return; }

            foreach (var e in MouseStateInfo.Entered)
            {
                e.BrushColour = Color.Fuchsia;
            }

            foreach (var e in MouseStateInfo.Left)
            {
                e.BrushColour = Color.White;
            }
        }

        private void DetectCollisions()
        {
            for (int i = 0; i < EntityList.Count; i++)
            {
                var entityI = EntityList[i];
                for (int j = i + 1; j < EntityList.Count; j++)
                {
                    var entityJ = EntityList[j];

                    var collisionResult = GameEntity.CheckCollision(entityI, entityJ);

                    if (collisionResult.CollisionOccured)
                    {
                        entityI.HandleCollision(new CollisionInfo(collisionResult, entityI));
                        entityJ.HandleCollision(new CollisionInfo(collisionResult, entityJ));
                    }
                }
            }
        }
    }
}
