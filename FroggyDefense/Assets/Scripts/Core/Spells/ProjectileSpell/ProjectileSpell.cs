using System.Collections.Generic;
using Core.Actions;
using UnityEngine;

namespace Core.Spells
{
    public class ProjectileSpell : Spell
    {
        public ProjectileSpell(SpellObject template)
        {
            Template = template;
            CollisionList = new List<Collider2D>();
            ActionIndex = new Dictionary<int, Actions.Action>();
            School = template.School;
        }

        public override bool Cast(ActionArgs args)
        {
            return base.Cast(args);
        }
    }
}