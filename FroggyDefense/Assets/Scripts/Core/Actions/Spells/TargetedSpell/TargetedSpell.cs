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
            _overlapTargetList = new List<Collider2D>();
        }

        //public override void StartInputProtocol()
        //{
        //    Debug.Log($"Starting Targeted Spell protocol.");
        //    base.StartInputProtocol();
        //}

        public override bool Cast(ActionArgs args)
        {
            return base.Cast(args);
        }
    }
}