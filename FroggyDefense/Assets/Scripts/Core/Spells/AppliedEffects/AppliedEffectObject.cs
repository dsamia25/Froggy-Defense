using UnityEngine;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Core.Spells
{
    [CreateAssetMenu(fileName = "New Status Effect", menuName = "ScriptableObjects/Spells/New Status Effect")]
    public class AppliedEffectObject : ScriptableObject
    {
        public string Name = "APPLIED EFFECT";
        public DamageActionArgs EffectArgs;     // Damage or slow amount, also has crit info and damage type.
        public float EffectTime = 1;            // Ticks or seconds.
        public float Frequency = 1;             // Tick Frequency or countdown speed modifier.
        public AppliedEffectType Effect;        // What the intended effect is. (DOT, slow, stun).
        public StatusSchool School;             // Which kind of applied effect. (Curce, bleed, magic...)

        public SpellAction[] OnTickActions;      // Special actions to take on every tick.
        public SpellAction[] OnExpireActions;    // Special actions to take when expired.
    }
}