using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    /// <summary>
    /// Structure to feed use info into a spell.
    /// </summary>
    public struct SpellArgs
    {
        public Vector3 Position;

        public SpellArgs(Vector3 pos)
        {
            Position = pos;
        }
    }
}