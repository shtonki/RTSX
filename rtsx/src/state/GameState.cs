using rtsx.src.util;
using System.Collections.Generic;
using System.Timers;

namespace rtsx.src.state
{
    class GameState
    {
        private List<GameEntity> EntityList { get; } = new List<GameEntity>();
        public IEnumerable<GameEntity> Entities => EntityList;

        private Timer StepTimer;
        private const double StepsPerSecond = 100;

        public GameState()
        {
            StepTimer = new Timer();
            StepTimer.Interval = StepsPerSecond/60;
            StepTimer.Elapsed += (_, __) => Step();
            StepTimer.Start();
        }

        public void AddEntity(GameEntity gameEntity)
        {
            if (EntityList.Contains(gameEntity))
            {
                Logging.Log("Tried to add duplicate GameEntity to GameState");
                return;
            }

            EntityList.Add(gameEntity);
        }

        private void Step()
        {
            foreach (var entity in Entities)
            {
                entity.Step();
            }

            for (int i = 0; i < EntityList.Count; i++)
            {
                var entityI = EntityList[i];
                for (int j = i + 1; j < EntityList.Count; j++)
                {
                    var entityJ = EntityList[j];

                    var collisionResult = GameEntity.CheckCollision(entityI, entityJ);

                    entityI.DBG = collisionResult.CollisionOccured;
                    entityJ.DBG = collisionResult.CollisionOccured;
                }
            }
        }
    }
}
