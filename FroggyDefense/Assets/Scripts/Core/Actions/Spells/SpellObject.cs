using UnityEngine;
using FroggyDefense.Weapons;
using FroggyDefense.Core.Actions.Inputs;
using ShapeDrawer;

namespace FroggyDefense.Core.Spells
{
    public enum SpellType
    {
        Area,
        Projectile,
        Targeted
    }

    // TODO: Abstractify this.
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
        public float Cooldown;          // How long until the spell can be used again.
        public float ManaCost;          // How much mana to cast the spell.

        [Space]
        [Header("Targeting")]
        [Space]
        public LayerMask TargetLayer;   // The layer the targets are on.
        public float TargetRange;       // How far away the spell can be cast.
        public InputMode TargetMode;    // How the InputController should get the spell inputs.

        [Space]
        [Header("Intial Damage")]
        [Space]
        public ProjectileInfo Projectile;       // Projectile info for projectile type spells.
        public float Damage;                    // How much damage the spell does.
        public DamageType SpellDamageType;
        public Shape EffectShape;              // How wide of an area the spell effects.

        [Space]
        [Header("Status Effect")]
        [Space]
        public bool AppliesStatusEffect;                            // If the spell applies status effects.
        public StatusEffectBuilder AppliedStatusEffect;             // List of applied status effects.

        [Space]
        [Header("Dot Effect")]
        [Space]
        public bool AppliesDot;                                     // If the spell applies an overtime effect to the hit enemies.
        public DamageOverTimeEffectBuilder AppliedOverTimeEffect;   // The dot this spell applies.

        [Space]
        [Header("Damage Area")]
        [Space]
        public bool CreatesDamageArea;                              // If the spell creates a damage area.
        public DamageAreaBuilder CreatedDamageArea;                 // The damage area the spell creates.
    }
}