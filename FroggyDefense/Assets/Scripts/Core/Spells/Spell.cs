using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public abstract class Spell
    {
        public string Name;
        public float SpellRange;
        public float EffectRadius;
        public float Cooldown = 1;

        public abstract void GetTarget();
        public abstract bool Cast(Vector2 pos);
    }
}