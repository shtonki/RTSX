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

        public FloatStat AttackRange { get; }
        public FloatStat AttackDamage { get; }
        public FloatStat AttackSpeed { get; }

        public Attributes(double maxHealth, 
            double movementSpeed, 
            double attackRange, double attackDamage, double attackSpeed)
            : this(maxHealth, movementSpeed)
        {
            AttackRange = new FloatStat(attackRange);
            AttackDamage = new FloatStat(attackDamage);
            AttackSpeed = new FloatStat(attackSpeed);
        }

        public Attributes(double maxHealth, 
            double movementSpeed)
            : this(maxHealth)
        {
            MovementSpeed = new FloatStat(movementSpeed);
        }

        public Attributes(double maxHealth)
        {
            MaxHealth = new FloatStat(maxHealth);
            CurrentHealth = new FloatStat(maxHealth);
        }
    }

    class FloatStat
    {
        public static implicit operator double(FloatStat stat)
        {
            return stat.BaseValue + stat.Modifier;
        }

        private double BaseValue { get; set; }
        private double Modifier { get; set; }

        public FloatStat(double baseValue)
        {
            BaseValue = baseValue;
        }

        public void Modify(double value)
        {
            Modifier += value;
        }

        public void ModifyPercentage(double value)
        {
            Modifier += this * value * 0.01;
        }

        public void SetTo(double Value)
        {
            Modifier = Value - BaseValue;
        }
    }
}
