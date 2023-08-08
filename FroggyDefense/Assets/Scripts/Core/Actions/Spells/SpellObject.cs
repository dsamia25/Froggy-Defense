using UnityEngine;
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

        public InputMode TargetMode;                    // How the InputController should get the spell inputs.
        public Shape EffectShape;                       // How wide of an area the spell effects.
        public float TargetRange;                       // How far away the spell can be cast.

        public SpellAction[] Actions;                  // List of actions to take.

        [TextArea(3, 5)]
        public string Description;                      // The spell's description.
    }
}