using System;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [Serializable]
    public class DamageOverTimeEffect: AppliedEffect
    {
        public DamageActionArgs DamageArgs { get; private set; }    // The damage effect args including total damage, damage type, crit effects.

        public int Ticks { get; private set; }                      // The total amount of ticks the effect does.
        public int TicksLeft { get; private set; }                  // The amount of ticks the effect has left.
        public float DamagePerTick { get; private set; }            // The amount of damage applied each tick.
        public float TickFrequency { get; private set; }            // How frequent the dot ticks in seconds.
        public Character Caster { get; private set; }               // Who applied the effect.
        public IDestructable Target { get; private set; }           // Who is being damaged.

        public DamageType EffectDamageType => DamageArgs.SpellDamageType;    // What type of damage the effect does.

        public float TotalDamage => Ticks * DamagePerTick;          // The total amount of damage the effect will do over its duration.
        public float DamageLeft => TicksLeft * DamagePerTick;       // The amount of damage the effect will apply over the rest of its duration.

        private float _currTickCooldown;

        public DamageOverTimeEffect(Character caster, IDestructable target, AppliedEffectObject template, DamageActionArgs damageArgs, int ticks, float tickFrequency)
        {
            Template = template;
            Name = template.Name;
            Target = target;
            Caster = caster;
            DamageArgs = damageArgs;
            Ticks = ticks;
            TicksLeft = ticks;
            TickFrequency = tickFrequency;

            DamagePerTick = DamageArgs.Damage / Ticks;

            _currTickCooldown = TickFrequency;
        }

        public override void Tick()
        {
            if (_currTickCooldown <= 0)
            {
                Target.TakeDamage(DamageAction.CreateDamageAction(Caster, DamageArgs.Damage, DamageArgs.SpellPowerRatio, DamageArgs.SpellDamageType, DamageArgs.CritChanceModifier, DamageArgs.CritBonusModifier));
                _currTickCooldown = TickFrequency;
                TicksLeft--;
                if (TicksLeft <= 0) IsExpired = true;
            }
            else
            {
                _currTickCooldown -= Time.deltaTime;
            }
        }

        public override void Refresh()
        {
            TicksLeft = Ticks;
            IsExpired = false;
        }

        public override void Clear()
        {
            IsExpired = true;
            TicksLeft = 0;
            Target = null;
        }
    }
}