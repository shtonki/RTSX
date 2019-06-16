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
        private const double MoveSpeedMultiplier = 0.001;

        public Player Owner { get; }

        public Attributes Attributes { get; protected set; }


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
                var healthPercentage = Attributes.CurrentHealth.Value / Attributes.MaxHealth.Value;
                var backSize = sizeHalved * healthPercentage;
                drawer.FillRectangle(Location - backSize, Location + backSize, Owner.PlayerColour);
            }

            base.Draw(drawer);
        }

        public override void Step()
        {
            Attributes.CurrentHealth.BaseValue -= 0.1;
            // ugly hack to allow for MoveSpeed to be in GameEntity and have another
            // variable in Attributes called MovementSpeed
            MoveSpeed = MoveSpeedMultiplier * Attributes.MovementSpeed.Value;
            base.Step();
        }
    }

    class DummyUnit : Unit
    {
        public DummyUnit(Player owner) : base(EntitySize.Medium, owner)
        {
            Attributes = new Attributes(100, 10);

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
