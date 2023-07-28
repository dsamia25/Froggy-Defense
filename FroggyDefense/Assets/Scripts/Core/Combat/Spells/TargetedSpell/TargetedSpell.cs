using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public class TargetedSpell : Spell
    {
        public TargetedSpell(SpellObject template)
        {
            Template = template;
            _overlapTargetList = new List<Collider2D>();
        }

        public override void StartInputProtocol()
        {
            Debug.Log($"Starting Targeted Spell protocol.");
            base.StartInputProtocol();
        }

        public override bool Cast(SpellArgs args)
        {
            return base.Cast(args);
        }
    }
}