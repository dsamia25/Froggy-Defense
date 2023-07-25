using System;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [Serializable]
    public class DamageOverTimeEffect
    {
        public string Name { get; private set; }                    // The effect's name.
        public int Ticks { get; private set; }                      // The total amount of ticks the effect does.
        public int TicksLeft { get; private set; }                  // The amount of ticks the effect has left.
        public float DamagePerTick { get; private set; }            // The amount of damage applied each tick.
        public float TickFrequency { get; private set; }            // How frequent the dot ticks in seconds.
        public DamageType EffectDamageType { get; private set; }    // What type of damage the effect does.
        public Character Caster { get; private set; }               // Who applied the effect.
        public IDestructable Target { get; private set; }           // Who is being damaged.

        public float TotalDamage => Ticks * DamagePerTick;          // The total amount of damage the effect will do over its duration.
        public float DamageLeft => TicksLeft * DamagePerTick;       // The amount of damage the effect will apply over the rest of its duration.

        private float _currTickCooldown;

        public DamageOverTimeEffect(Character caster, IDestructable target, string name, float damagePerTick, DamageType effectDamageType, int ticks, float tickFrequency)
        {
            Name = name;
            Target = target;
            Caster = caster;
            DamagePerTick = damagePerTick;
            EffectDamageType = effectDamageType;
            Ticks = ticks;
            TicksLeft = ticks;
            TickFrequency = tickFrequency;

            _currTickCooldown = TickFrequency;
        }

        /// <summary>
        /// Returns a new damage instance from the effect. If all ticks are used,
        /// returns null.
        /// </summary>
        /// <returns></returns>
        public void Tick()
        {
            if (_currTickCooldown <= 0)
            {
                DamageAction damage = new DamageAction(Caster, DamagePerTick, EffectDamageType);
                Target.TakeDamage(damage);
                _currTickCooldown = TickFrequency;
                TicksLeft--;
            } else
            {
                _currTickCooldown -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Refreshs the ticks left to the full amount.
        /// </summary>
        public void Refresh()
        {
            TicksLeft = Ticks;
        }
    }

    [Serializable]
    public class DamageOverTimeEffectBuilder
    {
        public string Name = "DOT";
        public int Ticks = 1;
        public float DamagePerTick = 1;
        public float TickFrequency = 1;
        public DamageType EffectDamageType;
    }
}