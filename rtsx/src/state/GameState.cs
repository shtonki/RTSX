using rtsx.src.util;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Linq;
using rtsx.src.state.gameEntities;

namespace rtsx.src.state
{
    class GameState
    {
        private List<GameEntity> EntityList { get; } = new List<GameEntity>();
        public IEnumerable<GameEntity> Entities => EntityList;
        private List<GameEntity> Selected = new List<GameEntity>();
        private IEnumerable<GameEntity> Controlled => Selected.Where(s => s.Controllable);

        public MouseEntity MouseEntity { get; } = new MouseEntity();
        private MouseStateInfo MSI => MouseEntity.MouseStateInfo;

        private Timer StepTimer;
        private const double StepsPerSecond = 100;

        private SelectorStart SelectorStart;
        private SelectorEnd SelectorEnd;

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

        public void RemoveEntity(GameEntity gameEntity)
        {
            if (!EntityList.Remove(gameEntity))
            {
                Logging.Log("Tried to remove GameEntity from GameState it is not present in");
            }
        }

        private void Step()
        {
            foreach (var entity in Entities)
            {
                entity.Step();
            }

            HandleSelector();

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

        private IEnumerable<GameEntity> DetectCollisions(GameEntity detector)
        {
            List<GameEntity> collided = new List<GameEntity>();

            foreach (var entity in EntityList)
            {
                if (entity == detector) { continue; }

                if (GameEntity.CheckCollision(detector, entity).CollisionOccured)
                {
                    collided.Add(entity);
                }
            }

            return collided;
        }

        private void HandleSelector()
        {
            if (SelectorEnd != null)
            {
                if (SelectorStart == null) { throw new RTSXException(); }

                SelectorEnd.Location = MouseEntity.Location;
            }
        }

        public void HandleAction(GameAction action)
        {
            switch (action.Action)
            {
                case GameActions.SelectStart:
                    {
                        if (SelectorStart != null || SelectorEnd != null)
                        { throw new RTSXException(); }

                        SelectorStart = new SelectorStart();
                        SelectorStart.Location = MouseEntity.Location;
                        AddEntity(SelectorStart);

                        SelectorEnd = new SelectorEnd(SelectorStart);
                        SelectorEnd.Location = MouseEntity.Location;
                        AddEntity(SelectorEnd);
                    } break;

                case GameActions.SelectEnd:
                    {
                        var selector = new SelectorEntity(SelectorStart, SelectorEnd);

                        SelectEntities(DetectCollisions(selector));

                        RemoveEntity(SelectorStart);
                        SelectorStart = null;
                        RemoveEntity(SelectorEnd);
                        SelectorEnd = null;
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
                v.Selected = false;
            }

            Selected.Clear();

            foreach (var v in entitiesCopy)
            {
                v.Selected = true;
                Selected.Add(v);
            }
        }

        private void RouteTo(Coordinate destination)
        {
            foreach (var v in Controlled)
            {
                v.Following = null;
                v.MoveTo = destination;
            }
        }

        private void Follow(GameEntity followed)
        {
            foreach (var v in Controlled)
            {
                v.Following = followed;
            }
        }
    }
}
