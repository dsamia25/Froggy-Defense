using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public class ProjectileSpell : Spell
    {
        public ProjectileSpell(SpellObject template)
        {
            Template = template;
            _overlapTargetList = new List<Collider2D>();
        }

        public override void StartInputProtocol()
        {
            Debug.Log($"Starting Projectile Spell protocol.");
            base.StartInputProtocol();
        }

        public override bool Cast(SpellArgs args)
        {
            return base.Cast(args);
        }
    }
}