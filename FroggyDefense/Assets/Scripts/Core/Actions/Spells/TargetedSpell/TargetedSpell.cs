using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Core.Spells
{
    public class TargetedSpell : Spell
    {
        public TargetedSpell(SpellObject template)
        {
            Template = template;
            CollisionList = new List<Collider2D>();
            ActionIndex = new Dictionary<int, Actions.Action>();
        }

        public override bool Cast(ActionArgs args)
        {
            return base.Cast(args);
        }
    }
}