using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Core.Spells
{
    public class ProjectileSpell : Spell
    {
        public ProjectileSpell(SpellObject template)
        {
            Template = template;
            _overlapTargetList = new List<Collider2D>();
        }

        public override bool Cast(ActionArgs args)
        {
            return base.Cast(args);
        }
    }
}