using UnityEngine;
using FroggyDefense.Core.Spells;
using ShapeDrawer;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Find Targets Area Action", menuName = "ScriptableObjects/Actions/New Find Targets Area Action")]
    public class FindTargetsAreaActionObject : ActionObject
    {
        public LayerMask TargetLayer;                   // The layer the targets are on.
        public Shape EffectShape;                       // How wide of an area the spell effects.
        public float Damage;                            // How much damage the spell does.
        public DamageType SpellDamageType;              // What kind of damage is applied (If applicable).
        public AppliedEffectObject[] AppliedEffects;    // List of applied effects.
    }
}