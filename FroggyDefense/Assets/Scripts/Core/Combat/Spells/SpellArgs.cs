using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    /// <summary>
    /// Structure to feed use info into a spell.
    /// </summary>
    public struct SpellArgs
    {
        public Character Caster;
        public Vector3 Position;

        public SpellArgs(Character caster, Vector3 pos)
        {
            Caster = caster;
            Position = pos;
        }
    }
}