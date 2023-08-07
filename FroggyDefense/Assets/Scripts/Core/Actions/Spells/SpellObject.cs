using UnityEngine;
using FroggyDefense.Weapons;
using FroggyDefense.Core.Actions.Inputs;
using FroggyDefense.Core.Actions;
using ShapeDrawer;

namespace FroggyDefense.Core.Spells
{
    public enum SpellType
    {
        Area,
        Projectile,
        Targeted
    }

    [CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/Spells/New Spell")]
    public class SpellObject : ScriptableObject
    {
        public string Name;                             // The spell's name.
        public Sprite Icon;                             // The spell's display icon.
        public int SpellId;                             // The spell's id.
        public SpellType Type;                          // The type of spell cast.
        public float Cooldown;                          // How long until the spell can be used again.
        public float ManaCost;                          // How much mana to cast the spell.

        //public LayerMask TargetLayer;                   // The layer the targets are on.
        public InputMode TargetMode;                    // How the InputController should get the spell inputs.
        public Shape EffectShape;                       // How wide of an area the spell effects.
        public float TargetRange;                       // How far away the spell can be cast.

        
        //public ProjectileInfo Projectile;               // Projectile info for projectile type spells.
        //public float Damage;                            // How much damage the spell does.
        //public DamageType SpellDamageType;

        //public AppliedEffectObject[] AppliedEffects;    // List of applied effects.

        //[Space]
        //[Header("Damage Area")]
        //[Space]
        //public bool CreatesDamageArea;                  // If the spell creates a damage area.
        //public DamageAreaBuilder CreatedDamageArea;     // The damage area the spell creates.

        //[Space]
        //[Header("Actions")]
        //[Space]
        public SpellAction[] Actions;                  // List of actions to take.

        [TextArea(3, 5)]
        public string Description;                      // The spell's description.
    }
}