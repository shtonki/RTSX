using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rtsx.src.util;
using rtsx.src.view;

namespace rtsx.src.state.gameEntities
{
    class Projectile : GameEntity
    {
        public GameEntity Launcher { get; }
        public double Damage { get; }
        public Color Colour { get; }

        public Projectile(GameEntity launcher, Unit target, double damage, double speed) : base(EntitySize.Tiny)
        {
            Launcher = launcher;
            Damage = damage;
            Following = target;
            MoveSpeed = speed;
        }

        public override void Draw(Drawer drawer)
        {
            var sizeHalved = Size / 2;
            drawer.FillRectangle(Location - sizeHalved, Location + sizeHalved, Color.AntiqueWhite);
        }

        public override void HandleCollision(CollisionInfo collisionInfo)
        {
            if (collisionInfo.Other == Following)
            {
                collisionInfo.GameState.RaiseGameEvent(new DamageEvent(Launcher, Following as Unit, Damage));
                collisionInfo.GameState.RaiseGameEvent(new DestroyEvent(this));
            }
        }
    }

    class ProjectileBlueprint
    {
        public double ProjectileSpeed { get; }

        public ProjectileBlueprint(double projectileSpeed)
        {
            ProjectileSpeed = projectileSpeed;
        }

        public Projectile Launch(GameEntity attacker, Unit target, double damage)
        {
            return new Projectile(attacker, target, damage, ProjectileSpeed);
        }
    }
}
