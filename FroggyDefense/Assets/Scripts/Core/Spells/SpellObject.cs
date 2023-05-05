using UnityEngine;
using UnityEngine.Events;

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
        public string Name;         // The spell's name.
        public Sprite Icon;         // The spell's display icon.
        public int SpellId;

        // TODO: Make a Shape class for the effect shape.
        public float EffectRadius;  // How wide of an area the spell effects.
        public float TargetRange;   // How far away the spell can be cast.

        public float Cooldown;      // How long until the spell can be used again.
        public float ManaCost;      // How much mana to cast the spell.

        /*
         * TODO: Add more parameters to have a spell-builder system.
         */
    }
}