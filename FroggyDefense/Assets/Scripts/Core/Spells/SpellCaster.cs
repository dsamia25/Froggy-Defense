using UnityEngine;
using ShapeDrawer;

namespace FroggyDefense.Core.Spells
{
    public class SpellCaster : MonoBehaviour
    {
        public delegate void SpellCasterCallback();
        public SpellCasterCallback SpellUsedCallback;

        public static void Blizzard(SpellArgs args)
        {
            Debug.Log("Spellcaster using Blizzard at (" + args.Position + ").");
        }

        public void Fireball(SpellArgs args)
        {
            Debug.Log("Spellcaster using Fireball at (" + args.Position + ").");
        }

        public void ArcaneMissiles(SpellArgs args)
        {
            Debug.Log("Spellcaster using ArcaneMissiles (" + args.Number + ").");
        }
    }
}