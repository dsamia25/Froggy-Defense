using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public class Spell
    {
        public SpellObject Template;

        public string Name => Template.Name;
        public int SpellId => Template.SpellId;
        public float EffectRadius => Template.EffectRadius;
        public float TargetRange => Template.TargetRange;
        public float Cooldown => Template.Cooldown;

        private float _currCooldown;
        public float CurrCooldown { get => _currCooldown; }

        public delegate void SpellEffect();
        public SpellEffect _spellEffect = null;

        public Spell(SpellObject template)
        {
            Template = template;
        }

        /*
         * TODO: Make a spell-builder style of Cast using the SpellType enum
         * in SpellObject and all the parameters set in the scriptableobject.
         * 
         */
        public virtual bool Cast(Vector2 pos)
        {
            if (_currCooldown > 0)
            {
                Debug.Log("Cannot cast spell. " + Name + " still on cooldown. (" + _currCooldown.ToString("0.00") + " seconds remaining)");
                return false;
            }

            _spellEffect?.Invoke();
            _currCooldown = Cooldown;
            return true;
        }

        ///// <summary>
        ///// Creates the appropriate spell.
        ///// </summary>
        ///// <param name="template"></param>
        ///// <returns></returns>
        //public static Spell CreateSpell(SpellObject template)
        //{
        //    if (template == null) return null;

        //    switch (template.Name)
        //    {
        //        case "Blizzard":
        //            return new Blizzard(template);
        //        case "Fireball":
        //            return new Fireball(template);
        //        default:
        //            Debug.Log("Unknown spell.");
        //            return null;
        //    }
        //}
    }
}