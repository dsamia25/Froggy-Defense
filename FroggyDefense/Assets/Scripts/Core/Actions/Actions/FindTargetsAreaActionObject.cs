using System;
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

        private void Awake()
        {
            Type = ActionType.FindTargetsArea;
        }
    }

    public class FindTargetsAreaAction : Action
    {
        FindTargetsAreaActionObject Template;

        public FindTargetsAreaAction(FindTargetsAreaActionObject template)
        {
            Template = template;
            ActionId = template.ActionId;
        }

        public override void Resolve(ActionArgs args)
        {
            try
            {
                int targetAmount = ActionUtils.GetTargets(args.Inputs.point1, Template.EffectShape, Template.TargetLayer, args.CollisionList);
                Debug.Log($"Cast: Found {targetAmount} targets. {args.CollisionList.Count} in list.");
                for (int i = 0; i < targetAmount; i++)
                {
                    Collider2D collider = args.CollisionList[i];
                    if (collider == null)
                    {
                        Debug.Log($"Collider is null.");
                        continue;
                    }
                    Debug.Log($"Loop {i} of {targetAmount}");
                    IDestructable target = null;
                    if ((target = collider.gameObject.GetComponent<IDestructable>()) != null)
                    {
                        Debug.Log($"Applying damage to {collider.gameObject.name} (collider {i}).");
                        target.TakeDamage(new DamageAction(args.Caster, Template.Damage, Template.SpellDamageType));

                        foreach (AppliedEffectObject effect in Template.AppliedEffects)
                        {
                            args.Target.ApplyEffect(AppliedEffect.CreateAppliedEffect(effect, args.Caster, args.Target));
                        }
                    }
                    else
                    {
                        Debug.Log($"Could not find IDestructable for {collider.gameObject.name} (collider {i}).");
                    }
                }
            } catch (Exception e)
            {
                Debug.LogWarning($"Error resolving Find Targets Area Action: {e}");
            }
        }
    }
}