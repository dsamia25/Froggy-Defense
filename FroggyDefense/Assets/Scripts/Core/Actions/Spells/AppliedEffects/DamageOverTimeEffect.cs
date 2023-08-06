using System;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [Serializable]
    public class DamageOverTimeEffect: AppliedEffect
    {
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

        public DamageOverTimeEffect(Character caster, IDestructable target, AppliedEffectObject template, float damagePerTick, DamageType effectDamageType, int ticks, float tickFrequency)
        {
            Template = template;
            Name = template.Name;
            Target = target;
            Caster = caster;
            DamagePerTick = damagePerTick;
            EffectDamageType = effectDamageType;
            Ticks = ticks;
            TicksLeft = ticks;
            TickFrequency = tickFrequency;

            _currTickCooldown = TickFrequency;
        }

        public override void Tick()
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

        public override void Refresh()
        {
            TicksLeft = Ticks;
        }
    }

    // OLD WAY. Made the effect directly builable in the inspector.
    //[Serializable]
    //public class DamageOverTimeEffectBuilder: AppliedEffectBuilder
    //{
    //    public DamageType DamageType;
    //}
}