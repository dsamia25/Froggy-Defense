using UnityEngine;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Apply Effect Action", menuName = "ScriptableObjects/Actions/New Apply Effect Action")]
    public class ApplyEffectActionObject : ActionObject
    {
        public AppliedEffectObject Effect;

        private void Awake()
        {
            Type = ActionType.ApplyEffect;
        }
    }

    public class ApplyEffectAction : Action
    {
        ApplyEffectActionObject Template;

        public ApplyEffectAction (ApplyEffectActionObject template)
        {
            Template = template;
        }

        public override void Resolve(ActionArgs args)
        {
            if (args.Target == null)
            {
                Debug.LogWarning($"Error resolving Apply Effect Action: Target cannot be null.");
                return;
            }
            args.Target.ApplyEffect(AppliedEffect.CreateAppliedEffect(Template.Effect, args.Caster, args.Target));
        }
    }
}