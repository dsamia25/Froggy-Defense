using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public enum SpellType
    {
        AOE,
        Targeted
    }

    [CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/Spells/New Spell")]
    public class SpellObject : ScriptableObject
    {
        [Space]
        [Header("Info")]
        [Space]
        public string Name;             // The spell's name.
        public Sprite Icon;             // The spell's display icon.
        public int SpellId;             // The spell's id.
        public SpellType Type;          // The type of spell cast.

        [Space]
        [Header("Targeting")]
        [Space]
        public LayerMask TargetLayer;   // The layer the targets are on.
        // TODO: Make a Shape class for the effect shape.
        public float EffectRadius;      // How wide of an area the spell effects.
        public float TargetRange;       // How far away the spell can be cast.

        [Space]
        [Header("Damage")]
        [Space]
        public float Damage;            // How much damage the spell does.
        public DamageType SpellDamageType;
        public bool AppliesDot;
        public DamageOverTimeEffectBuilder AppliedOverTimeEffect;    // If the spell applies an overtime effect to the hit enemies.

        public float Cooldown;          // How long until the spell can be used again.
        public float ManaCost;          // How much mana to cast the spell.
    }
}