using System;
using UnityEngine;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Create Damage Zone Action", menuName = "ScriptableObjects/Actions/New Create Damage Zone Action")]
    public class CreateDamageZoneActionObject : ActionObject
    {
        // TODO: Probably make this a scriptable object like everything else.
        public DamageAreaBuilder Area;

        private void Awake()
        {
            Type = ActionType.CreateDamageZone;
        }
    }

    public class CreateDamageZoneAction : Action
    {
        CreateDamageZoneActionObject Template;

        public CreateDamageZoneAction(CreateDamageZoneActionObject template)
        {
            Template = template;
        }

        public override void Resolve(ActionArgs args)
        {
            try
            {
                ActionUtils.CreateDamageArea(args.Inputs.point1, Template.Area);
            } catch (Exception e)
            {
                Debug.LogWarning($"Error resolving Create Damage Zone Action: {e}");
            }
        }
    }
}