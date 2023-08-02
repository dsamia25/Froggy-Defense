using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public class AreaSpell : Spell
    {
        public AreaSpell(SpellObject template)
        {
            Template = template;
            _overlapTargetList = new List<Collider2D>();
        }

        public override bool Cast(SpellArgs args)
        {
            return base.Cast(args);
        }
    }
}