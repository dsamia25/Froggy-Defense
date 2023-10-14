using System;
using UnityEngine;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Core.Spells
{
    [Serializable]
    public class DamageOverTimeEffect: AppliedEffect
    {
        public DamageActionArgs DamageArgs { get; protected set; }    // The damage effect args including total damage, damage type, crit effects.

        public int Ticks { get; protected set; }                      // The total amount of ticks the effect does.
        public int TicksLeft { get; protected set; }                  // The amount of ticks the effect has left.
        public float DamagePerTick { get; protected set; }            // The amount of damage applied each tick.
        public float TickFrequency { get; protected set; }            // How frequent the dot ticks in seconds.

        public DamageType EffectDamageType => DamageArgs.SpellDamageType;    // What type of damage the effect does.

        public float TotalDamage => Ticks * DamagePerTick;          // The total amount of damage the effect will do over its duration.
        public float DamageLeft => TicksLeft * DamagePerTick;       // The amount of damage the effect will apply over the rest of its duration.

        protected float _currTickCooldown;

        public DamageOverTimeEffect(ActionArgs args, IDestructable target, AppliedEffectObject template, DamageActionArgs damageArgs, int ticks, float tickFrequency)
        {
            Template = template;
            Name = template.Name;
            Target = target;
            //Caster = caster;
            Args = args;
            DamageArgs = damageArgs;
            Ticks = ticks;
            TicksLeft = ticks;
            TickFrequency = tickFrequency;

            DamagePerTick = DamageArgs.Damage / Ticks;

            ActionIndex = new System.Collections.Generic.Dictionary<int, Actions.Action>();

            _currTickCooldown = TickFrequency;
        }

        public override void Tick()
        {
            if (_currTickCooldown <= 0)
            {
                Target.TakeDamage(DamageAction.CreateDamageAction(Args.Caster, DamageArgs.Damage, DamageArgs.SpellPowerRatio, DamageArgs.SpellDamageType, DamageArgs.CritChanceModifier, DamageArgs.CritBonusModifier));
                _currTickCooldown = TickFrequency;
                TicksLeft--;

                ResolveOnTickActions();

                if (TicksLeft <= 0)
                {
                    IsExpired = true;
                    ResolveOnExpireActions();
                }
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