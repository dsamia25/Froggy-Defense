using System;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [Serializable]
    public class StatusEffect: AppliedEffect
    {
        public float TimeLeft { get; private set; }
        public Character Caster { get; private set; }
        public IDestructable Target { get; private set; }

        public StatusEffect(Character caster, IDestructable target, AppliedEffectObject template)
        {
            Template = template;
            Name = Template.Name;
            Strength = template.Strength;
            EffectTime = Template.EffectTime;
            Frequency = Template.Frequency;
            Effect = template.Effect;
            School = Template.School;

            Target = target;
            Caster = caster;
            TimeLeft = template.EffectTime;
        }

        public override void Tick()
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0) IsExpired = true;
        }

        public override void Refresh()
        {
            TimeLeft = EffectTime;
            IsExpired = false;
        }

        public override void Clear()
        {
            IsExpired = true;
            TimeLeft = 0;
            Target = null;
        }
    }
}