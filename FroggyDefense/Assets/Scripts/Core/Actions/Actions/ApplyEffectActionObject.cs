using UnityEngine;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Apply Effect Action", menuName = "ScriptableObjects/Actions/New Apply Effect Action")]
    public class ApplyEffectActionObject : ActionObject
    {
        public AppliedEffectObject[] Effects;

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
            ActionId = template.ActionId;
        }

        public override void Resolve(ActionArgs args)
        {
            if (args.Target == null)
            {
                Debug.LogWarning($"Error resolving Apply Effect Action: Target cannot be null.");
                return;
            }

            foreach (AppliedEffectObject effect in Template.Effects)
            {
                args.Target.ApplyEffect(AppliedEffect.CreateAppliedEffect(effect, args, args.Target));
            }
        }
    }
}