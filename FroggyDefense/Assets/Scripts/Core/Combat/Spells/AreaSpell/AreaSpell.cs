using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public class AreaSpell : Spell
    {
        public AreaSpell(SpellObject template)
        {
            Template = template;
        }

        public override void StartInputProtocol()
        {
            Debug.Log($"Starting Area Spell protocol.");
            base.StartInputProtocol();
        }

        public override bool Cast(SpellArgs args)
        {
            return base.Cast(args);
        }
    }
}