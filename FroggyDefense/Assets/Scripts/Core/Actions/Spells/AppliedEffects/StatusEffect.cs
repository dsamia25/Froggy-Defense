using System;
using UnityEngine;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Core.Spells
{
    [Serializable]
    public class StatusEffect: AppliedEffect
    {
        public float TimeLeft { get; private set; }

        public StatusEffect(ActionArgs args, IDestructable target, AppliedEffectObject template)
        {
            Template = template;
            Name = Template.Name;
            EffectTime = Template.EffectTime;
            Frequency = Template.Frequency;
            Effect = template.Effect;
            School = Template.School;

            ActionIndex = new System.Collections.Generic.Dictionary<int, Actions.Action>();

            Args = args;
            Target = target;
            //Caster = caster;
            TimeLeft = template.EffectTime;
        }

        public override void Tick()
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0)
            {
                IsExpired = true;
                ResolveOnExpireActions();
            }
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