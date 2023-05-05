using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/Spells/New Spell")]
    public class SpellObject : ScriptableObject
    {
        public bool Blank = true;   // If the spell has not been set up. (Serializes fields cannot be null and so this is checked instead).
        public string Name;         // The spell's name.
        public Sprite Icon;         // The spell's display icon.

        // TODO: Make a Shape class for the effect shape.
        public float EffectRadius;  // How wide of an area the spell effects.
        public float TargetRange;   // How far away the spell can be cast.

        public float Cooldown;      // How long until the spell can be used again.

    }
}