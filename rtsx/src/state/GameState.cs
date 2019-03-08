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
        /// <summary> In milliseconds </summary>
        private const double StepInterval = 10;

        public GameState()
        {
            StepTimer = new Timer();
            StepTimer.Interval = StepInterval;
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
        }
    }
}
