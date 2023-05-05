using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public class Spell
    {
        public SpellObject Template;

        public bool Blank => Template.Blank;
        public string Name => Template.Name;
        public float EffectRadius => Template.EffectRadius;
        public float TargetRange => Template.TargetRange;
        public float Cooldown => Template.Cooldown;

        private float _currCooldown;
        public float CurrCooldown { get => _currCooldown; }


        public Spell(SpellObject template)
        {
            Template = template;
        }

        public virtual void GetTarget()
        {

        }

        public virtual bool Cast(Vector2 pos)
        {
            if (_currCooldown > 0)
            {
                Debug.Log("Cannot cast spell. " + Name + " still on cooldown. (" + _currCooldown.ToString("0.00") + " seconds remaining)");
                return false;
            }

            // TODO: Make a mana check that returns false if not enough mana.
            _currCooldown = Cooldown;
            return true;
        }
    }
}