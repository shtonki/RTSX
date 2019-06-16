using rtsx.src.util;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Linq;

namespace rtsx.src.state
{
    class GameState
    {
        private List<GameEntity> EntityList { get; } = new List<GameEntity>();
        public IEnumerable<GameEntity> Entities => EntityList;
        private List<GameEntity> Selected = new List<GameEntity>();

        public MouseEntity MouseEntity { get; } = new MouseEntity();
        private MouseStateInfo MSI => MouseEntity.MouseStateInfo;

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

        public void HandleAction(GameAction action)
        {
            switch (action.Action)
            {
                case GameActions.SelectEnd:
                    {
                        SelectEntities(MouseEntity.MouseStateInfo.Picked);
                    } break;

                case GameActions.RouteTo:
                    {
                        var paranoidCopy = MSI.Picked.ToList();

                        if (paranoidCopy.Count == 0)
                        {
                            RouteTo(MouseEntity.Location);
                        }
                        else if (paranoidCopy.Count == 1)
                        {
                            Follow(paranoidCopy[0]);
                        }
                        else
                        {
                            throw new RTSXException();
                        }
                    } break;
            }
        }

        private void SelectEntities(IEnumerable<GameEntity> entities)
        {
            // to avoid them pulling the rug out from under us
            var entitiesCopy = entities.ToList();
            
            foreach (var v in Selected)
            {
                v.BrushColour = Color.White;
            }

            Selected.Clear();

            foreach (var v in entitiesCopy)
            {
                v.BrushColour = Color.Green;
                Selected.Add(v);
            }
        }

        private void RouteTo(Coordinate destination)
        {
            foreach (var v in Selected)
            {
                v.Following = null;
                v.MoveTo = destination;
            }
        }

        private void Follow(GameEntity followed)
        {
            foreach (var v in Selected)
            {
                v.Following = followed;
            }
        }
    }
}
