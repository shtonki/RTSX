using rtsx.src.util;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Linq;
using rtsx.src.state.gameEntities;
using static rtsx.src.state.gameEntities.Unit;

namespace rtsx.src.state
{
    class GameState
    {
        private List<GameEntity> EntityList { get; } = new List<GameEntity>();
        public IEnumerable<GameEntity> Entities => EntityList;
        public IEnumerable<Unit> Units => EntityList.Where(e => e is Unit).Cast<Unit>();
        private List<GameEntity> Selected = new List<GameEntity>();
        private IEnumerable<GameEntity> Controlled => Selected.Where(s => s.Controllable);

        public MouseEntity MouseEntity { get; } = new MouseEntity();
        private MouseStateInfo MSI => MouseEntity.MouseStateInfo;

        private Timer StepTimer;
        private const double StepsPerSecond = 100;

        private SelectorStart SelectorStart;
        private SelectorEnd SelectorEnd;

        private Queue<GameEvent> PendingEvents = new Queue<GameEvent>();

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

        public void RaiseGameEvent(GameEvent gameEvent)
        {
            PendingEvents.Enqueue(gameEvent);
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
                    }
                    break;

                case GameActions.SelectEnd:
                    {
                        var selector = new SelectorEntity(SelectorStart, SelectorEnd);

                        SelectEntities(DetectCollisions(selector));

                        RemoveEntity(SelectorStart);
                        SelectorStart = null;
                        RemoveEntity(SelectorEnd);
                        SelectorEnd = null;
                    }
                    break;

                case GameActions.RouteTo:
                    {
                        var pickedCopy = MSI.Picked.ToList();

                        if (pickedCopy.Count == 0)
                        {
                            foreach (var e in Controlled)
                            {
                                e.RouteTo(MouseEntity.Location);
                            }
                        }
                        else if (pickedCopy.Count == 1)
                        {
                            foreach (var e in Controlled)
                            {
                                e.Follow(pickedCopy[0]);
                            }
                        }
                        else
                        {
                            Logging.Log("Picked multiple objects and rather than shit the bed with an exception I'm logging it.");
                        }
                    }
                    break;
            }
        }

        private void Step()
        {
            foreach (var entity in Entities)
            {
                entity.Step(this);
            }

            HandlePendingEvents();

            HandleSelector();

            DetectCollisions();
        }

        private void HandlePendingEvents()
        {
            while (PendingEvents.Count > 0)
            {
                HandleEvent(PendingEvents.Dequeue());
            }
        }

        private void HandleEvent(GameEvent gameEvent)
        {
            bool log = false;

            if (gameEvent is MoveEvent)
            {
                var moveEvent = gameEvent as MoveEvent;

                moveEvent.Mover.Move(moveEvent.MovementVector);
            }

            else if (gameEvent is BeginAttackEvent)
            {
                var beginAttackEvent = gameEvent as BeginAttackEvent;

                beginAttackEvent.Attacker.Status = Statii.Attacking;
                beginAttackEvent.Attacker.Attacking = beginAttackEvent.Attacked;
            }

            else if (gameEvent is LaunchProjectileEvent)
            {
                var launchProjectileEvent = gameEvent as LaunchProjectileEvent;
                var projectile = launchProjectileEvent.Projectile;
                AddEntity(projectile);
                projectile.Location = launchProjectileEvent.Launcher.Location;
            }

            else if (gameEvent is DamageEvent)
            {
                var damageEvent = gameEvent as DamageEvent;
                damageEvent.Target.Attributes.CurrentHealth.Modify(-damageEvent.Amount);
            }

            else if (gameEvent is DestroyEvent)
            {
                var destroyEvent = gameEvent as DestroyEvent;
                RemoveEntity(destroyEvent.Destroyed);
            }

            else
            {
                throw new RTSXException();
            }

            if (log)
            {
                Logging.Log(gameEvent);
            }
        }

        private void DetectCollisions()
        {
            List<CollisionInfo> collisions = new List<CollisionInfo>();

            for (int i = 0; i < EntityList.Count; i++)
            {
                var entityI = EntityList[i];
                for (int j = i + 1; j < EntityList.Count; j++)
                {
                    var entityJ = EntityList[j];

                    var collisionResult = GameEntity.CheckCollision(entityI, entityJ);

                    if (collisionResult.CollisionOccured)
                    {
                        collisions.Add(new CollisionInfo(this, collisionResult, entityI));
                        collisions.Add(new CollisionInfo(this, collisionResult, entityJ));
                    }
                }
            }

            foreach (var collision in collisions)
            {
                collision.Self.HandleCollision(collision);
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
    }
}
