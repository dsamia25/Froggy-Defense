using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public class TargettedSpell : Spell
    {
        public override void GetTarget()
        {
            throw new System.NotImplementedException();
        }

        public override bool Cast(Vector2 pos)
        {
            GetTarget();

            return false;
        }
    }
}