using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [CreateAssetMenu(fileName = "New Status Effect", menuName = "ScriptableObjects/Spells/New Status Effect")]
    public class AppliedEffectObject : ScriptableObject
    {
        public string Name = "APPLIED EFFECT";
        public float Strength = 1;              // Damage or slow amount.
        public float EffectTime = 1;            // Ticks or seconds.
        public float Frequency = 1;             // Tick Frequency or countdown speed modifier.
        public AppliedEffectType Effect;        // What the intended effect is. (DOT, slow, stun).
        public StatusSchool School;             // Which kind of applied effect. (Curce, bleed, magic...)
        public DamageType DamageType;           // What kind of damage. Only applies for Damage Over Time Effects.
    }
}