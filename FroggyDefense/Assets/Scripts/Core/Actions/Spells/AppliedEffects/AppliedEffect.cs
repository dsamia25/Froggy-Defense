using System;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public enum AppliedEffectType
    {
        Slow,
        Stun,
        DamageOverTime
    }

    public enum StatusSchool
    {
        Magic,
        Curse,
        Bleed,
        Poison
    }

    [Serializable]
    public abstract class AppliedEffect
    {
        protected AppliedEffectObject Template { get; set; }

        public string Name = "APPLIED EFFECT";
        public float Strength = 1;              // Damage or slow amount.
        public float EffectTime = 1;            // Ticks or seconds.
        public float Frequency = 1;             // Tick Frequency or countdown speed modifier.
        public AppliedEffectType Effect;        // What the intended effect is. (DOT, slow, stun).
        public StatusSchool School;             // Which kind of applied effect. (Curce, bleed, magic...)

        public static AppliedEffect CreateAppliedEffect(AppliedEffectObject template, Character caster, IDestructable target)
        {
            try
            {

                AppliedEffect effect = null;
                switch (template.Effect)
                {
                    case AppliedEffectType.DamageOverTime:
                        effect = new DamageOverTimeEffect(caster, target, template, template.Strength, template.DamageType, Mathf.FloorToInt(template.EffectTime), template.Frequency);
                        break;
                    case AppliedEffectType.Slow:
                        effect = new StatusEffect(caster, target, template);
                        break;
                    case AppliedEffectType.Stun:
                        effect = new StatusEffect(caster, target, template);
                        break;
                    default:
                        Debug.LogWarning($"Error creating status effect: Unknown effect type.");
                        break;
                }
                return effect;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error creating applied effect: {e}");
                return null;
            }
        }

        /// <summary>
        /// Ticks down how long the effect is active.
        /// </summary>
        public abstract void Tick();

        /// <summary>
        /// Refreshs the ticks left to the full amount.
        /// </summary>
        public abstract void Refresh();
    }

    // OLD WAY. Made the effect directly builable in the inspector.
    //[Serializable]
    //public class AppliedEffectBuilder
    //{
    //    public string Name = "APPLIED EFFECT";
    //    public float Strength = 1;              // Damage or slow amount.
    //    public float Time = 1;                  // Ticks or seconds.
    //    public float Frequency = 1;             // Tick Frequency or countdown speed modifier.
    //    public AppliedEffectType Effect;        // What the intended effect is. (DOT, slow, stun).
    //    public StatusSchool School;                 // Which kind of applied effect. (Curce, bleed, magic...)
    //}
}