using System;
using UnityEngine;
using FroggyDefense.Weapons;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Fire Projectile Action", menuName = "ScriptableObjects/Actions/New Fire Projectile Action")]
    public class FireProjectileActionObject : ActionObject
    {
        public ProjectileInfo projectileInfo;
        //public Action[] OnHitActions;

        private void Awake()
        {
            Type = ActionType.FireProjectile;
        }
    }

    public class FireProjectileAction : Action
    {
        FireProjectileActionObject Template;

        public FireProjectileAction(FireProjectileActionObject template)
        {
            Template = template;
            ActionId = template.ActionId;
        }

        public override void Resolve(ActionArgs args)
        {
            try
            {
                ActionUtils.FireProjectile(Template.projectileInfo, args.Caster, args.Inputs.point1, args.Inputs.point2);
            } catch (Exception e)
            {
                Debug.LogWarning($"Error resolving Fire Projectile Action: {e}");
            }
        }
    }
}