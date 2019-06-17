using rtsx.src.util;
using rtsx.src.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state.gameEntities
{
    abstract class Unit : GameEntity
    {
        public enum Statii { Normal, Attacking, }

        private const double MoveSpeedMultiplier = 0.0004;
        private const double BaseAttackTime = 70;
        private const double DamagePoint = 0.5;

        public Player Owner { get; }

        public Statii Status { get; set; } = Statii.Normal;
        public Attributes Attributes { get; protected set; }

        public bool IsAlive => Attributes.CurrentHealth > 0;

        protected ProjectileBlueprint AttackProjectile;
        public Unit Attacking { get; set; }
        private double AttackCounter;
        private bool InBackswing;
        public bool CanBeAttacked => IsAlive;
        public bool CanAttack(GameEntity other)
        {
            if (Status != Statii.Normal)
            {
                return false;
            }

            if (!(other is Unit))
            { return false; }

            var unit = other as Unit;

            return this.IsAlive && 
                unit.IsAlive && 
                unit.Owner != this.Owner;
        }

        public Unit(EntitySize size, Player owner) : base(size)
        {
            Owner = owner;
            Collidable = true;
            Controllable = owner.Controlled;
        }

        public override void Draw(Drawer drawer)
        {
            var sizeHalved = Size * 0.5;
            drawer.DrawRectangle(Location - sizeHalved, Location + sizeHalved, Selected ? Color.Green : Color.White, 3);

            if (Attributes.MaxHealth != null)
            {
                var healthPercentage = Attributes.CurrentHealth / Attributes.MaxHealth;
                var backSize = sizeHalved * healthPercentage;
                drawer.FillRectangle(Location - backSize, Location + backSize, Owner.PlayerColour);
            }

            if (AttackCounter > 0)
            {
                var swingPercentage = AttackCounter / BaseAttackTime;
                var backSize = sizeHalved * swingPercentage;
                var start = Location - sizeHalved;
                var end = new Coordinate(start.X - 0.01, start.Y + swingPercentage * Size.Y);
                drawer.FillRectangle(start, end, Color.Crimson);
            }

            base.Draw(drawer);
        }

        public override void Step(GameState gameState)
        {
            if (Attributes.CurrentHealth < 0)
            {
                gameState.RaiseGameEvent(new DestroyEvent(this));
                return;
            }

            UpdateAttributes();

            HandleAttacking(gameState);

            base.Step(gameState);
        }

        private void UpdateAttributes()
        {
            // ugly hack to allow for MoveSpeed to be in GameEntity and have another
            // variable in Attributes called MovementSpeed
            MoveSpeed = MoveSpeedMultiplier * Attributes.MovementSpeed;

            // sanity checks
            if (Attributes.CurrentHealth > Attributes.MaxHealth)
            {
                Attributes.CurrentHealth.SetTo(Attributes.MaxHealth);
            }
        }

        private void HandleAttacking(GameState gameState)
        {
            // if we want to attack and can attack we begin attacking
            if (Following != null &&
                CanAttack(Following) &&
                DistanceTo(Following) < Attributes.AttackRange)
            {
                gameState.RaiseGameEvent(new BeginAttackEvent(this, Following as Unit));
            }

            if (Status == Statii.Attacking)
            {
                AttackCounter += Attributes.AttackSpeed;

                // check for damage point
                if (!InBackswing && AttackCounter > BaseAttackTime * DamagePoint)
                {
                    InBackswing = true;
                    if (AttackProjectile != null)
                    {

                        var projectile = AttackProjectile.Launch(this, Attacking, Attributes.AttackDamage);
                        gameState.RaiseGameEvent(new LaunchProjectileEvent(
                            this, Attacking, projectile));
                    }
                    else
                    {
                        gameState.RaiseGameEvent(new DamageEvent(this, Attacking, Attributes.AttackDamage));
                    }
                }

                // check for end of backswing
                if (AttackCounter > BaseAttackTime)
                {
                    InBackswing = false;
                    Attacking = null;
                    Status = Statii.Normal;
                    AttackCounter = 0;
                }
            }
        }

        protected override Coordinate DetermineMovement()
        {
            if (Status == Statii.Normal)
            {
                return base.DetermineMovement();
            }
            if (Status == Statii.Attacking)
            {
                return Coordinate.ZeroVector;
            }

            throw new RTSXException();
        }

        public override void RouteTo(Coordinate destination)
        {
            if (AttackCounter > 0)
            {
                InBackswing = false;
                Attacking = null;
                Status = Statii.Normal;
                AttackCounter = 0;
            }
            base.RouteTo(destination);
        }
    }

    class Warrior : Unit
    {
        public Warrior(Player owner) : base(EntitySize.Medium, owner)
        {
            Attributes = new Attributes(100, 10, 0.03, 12, 1);

            Sprite = Sprites.Warrior;
        }
    }

    class Ranger : Unit
    {
        public Ranger(Player owner) : base(EntitySize.Medium, owner)
        {
            Attributes = new Attributes(70, 15, 0.45, 8, 1.5);
            AttackProjectile = new ProjectileBlueprint(0.01);

            Sprite = Sprites.Ranger;
        }
    }

    public enum Fixtures { Tree,}

    class Fixture : GameEntity
    {
        public Fixture(Fixtures fixture, EntitySize size, Player owner) : base(size)
        {
            switch (fixture)
            {
                case Fixtures.Tree:
                    {
                        Sprite = Sprites.Tree;
                        Collidable = true;
                    } break;
            }
        }

    }
}
