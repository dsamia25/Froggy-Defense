using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [CreateAssetMenu(fileName = "New Projectile Spell", menuName = "ScriptableObjects/Spells/New Projectile Spell")]
    public class ProjectileSpellObject : SpellObject
    {
        // TODO: Probably don't need this.
        public AppliedEffect InitialDamageAppliedEffect;
    }
}
