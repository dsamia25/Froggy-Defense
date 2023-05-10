using System;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public enum StatusEffectType
    {
        Slow,
        Stun
    }

    [Serializable]
    public class StatusEffect
    {
        private StatusEffectBuilder Template { get; set; }
        public string Name => Template.Name;
        public float EffectStrength => Template.EffectStrength;
        public StatusEffectType StatusType => Template.StatusType;
        public float TotalTime => Template.EffectTime;

        public float TimeLeft { get; private set; }
        public Character Caster { get; private set; }
        public IDestructable Target { get; private set; }

        public StatusEffect(Character caster, IDestructable target, StatusEffectBuilder template)
        {
            Template = template;
            Target = target;
            Caster = caster;
            TimeLeft = template.EffectTime;
        }

        public void Tick()
        {
            TimeLeft -= Time.deltaTime;
        }

        public void Refresh()
        {
            TimeLeft = TotalTime;
        }
    }

    [Serializable]
    public class StatusEffectBuilder
    {
        public string Name = "STATUS EFFECT";
        public float EffectStrength = 1;
        public StatusEffectType StatusType;
        public float EffectTime = 1;
    }
}