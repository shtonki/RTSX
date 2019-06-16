using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state
{
    class Attributes
    {
        public FloatStat MovementSpeed { get; }

        public FloatStat MaxHealth { get; }
        public FloatStat CurrentHealth { get; }

        public Attributes(double maxHealth, double movementSpeed)
        {
            MovementSpeed = new FloatStat(movementSpeed);

            MaxHealth = new FloatStat(maxHealth);
            CurrentHealth = new FloatStat(maxHealth);
        }
    }

    class FloatStat
    {
        public double Value => BaseValue;

        public double BaseValue { get; set; }

        public FloatStat(double baseValue)
        {
            BaseValue = baseValue;
        }
    }
}
