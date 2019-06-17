using rtsx.src.state.gameEntities;
using rtsx.src.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state
{
    abstract class GameEvent
    {
    }

    class MoveEvent : GameEvent
    {
        public GameEntity Mover { get; }
        public Coordinate MovementVector { get; }

        public MoveEvent(GameEntity mover, Coordinate movementVector)
        {
            Mover = mover;
            MovementVector = movementVector;
        }
    }

    class BeginAttackEvent : GameEvent
    {
        public Unit Attacker { get; }
        public Unit Attacked { get; }

        public BeginAttackEvent(Unit attacker, Unit attacked)
        {
            Attacker = attacker;
            Attacked = attacked;
        }
    }

    class LaunchProjectileEvent : GameEvent
    {
        public Unit Launcher { get; }
        public Unit Target { get; }
        public Projectile Projectile { get; }

        public LaunchProjectileEvent(Unit launcher, Unit target, Projectile projectile)
        {
            Launcher = launcher;
            Target = target;
            Projectile = projectile;
        }
    }

    class DamageEvent : GameEvent
    {
        public GameEntity Attacker { get; }
        public Unit Target { get; }
        public double Amount { get; }

        public DamageEvent(GameEntity attacker, Unit target, double amount)
        {
            Attacker = attacker;
            Target = target;
            Amount = amount;
        }
    }

    class DestroyEvent : GameEvent
    {
        public GameEntity Destroyed { get; }

        public DestroyEvent(GameEntity destroyed)
        {
            Destroyed = destroyed;
        }
    }
}
