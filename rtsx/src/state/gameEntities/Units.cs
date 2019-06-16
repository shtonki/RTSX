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

        private const double MoveSpeedMultiplier = 0.0007;
        private const double BaseAttackTime = 100;
        private const double DamagePoint = 0.5;

        public Player Owner { get; }

        public Statii Status { get; protected set; } = Statii.Normal;
        public Attributes Attributes { get; protected set; }

        private double AttackCounter;
        private bool InBackswing;
        private Unit Attacking;
        public bool CanAttack => Attributes.AttackRange != null;

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
                drawer.FillRectangle(Location - backSize, Location + backSize, Color.Transparent);
            }

            base.Draw(drawer);
        }

        public override void Step()
        {
            // ugly hack to allow for MoveSpeed to be in GameEntity and have another
            // variable in Attributes called MovementSpeed
            MoveSpeed = MoveSpeedMultiplier * Attributes.MovementSpeed;

            // sanity checks
            if (Attributes.CurrentHealth > Attributes.MaxHealth)
            {
                Attributes.CurrentHealth.SetTo(Attributes.MaxHealth);
            }

            base.Step();
        }

        protected override Coordinate DetermineMovement()
        {
            // if we want to attack and can attack we begin attacking
            if (Following != null && CanAttack && Following is Unit &&  DistanceTo(Following) < Attributes.AttackRange)
            {
                Status = Statii.Attacking;
                Attacking = Following as Unit;
            }

            if (Status == Statii.Normal)
            {
                return base.DetermineMovement();
            }
            if (Status == Statii.Attacking)
            {
                AttackCounter++;

                // check for damage point
                if (!InBackswing && AttackCounter > BaseAttackTime * DamagePoint)
                {
                    InBackswing = true;
                    Attacking.Attributes.CurrentHealth.Modify(-Attributes.AttackDamage);
                }

                // check for end of backswing
                if (AttackCounter > BaseAttackTime)
                {
                    InBackswing = false;
                    Attacking = null;
                    Status = Statii.Normal;
                    AttackCounter = 0;
                }

                return Coordinate.ZeroVector;
            }

            throw new RTSXException();
        }
    }

    class DummyUnit : Unit
    {
        public DummyUnit(Player owner) : base(EntitySize.Medium, owner)
        {
            Attributes = new Attributes(100, 10, 0.01, 10);

            Sprite = Sprites.Warrior;
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
