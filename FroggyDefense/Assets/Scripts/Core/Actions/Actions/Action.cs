using System;
using UnityEngine;
using FroggyDefense.Core.Actions.Inputs;

namespace FroggyDefense.Core.Actions
{
    public enum ActionType
    {
        FindTargetsArea,
        FireProjectile,
        CreateDamageZone,
        ApplyEffect        
    }

    public abstract class Action
    {
        public abstract void Resolve(ActionArgs args);

        /// <summary>
        /// Builds an action object.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public Action CreateAction(ActionObject template)
        {
            try
            {
                Action action = null;
                switch (template.Type)
                {
                    case ActionType.FindTargetsArea:
                        action = new FindTargetsAreaAction(template as FindTargetsAreaActionObject);
                        break;
                    case ActionType.FireProjectile:
                        action = new FireProjectileAction(template as FireProjectileActionObject);
                        break;
                    case ActionType.CreateDamageZone:
                        action = new CreateDamageZoneAction(template as CreateDamageZoneActionObject);
                        break;
                    case ActionType.ApplyEffect:
                        action = new ApplyEffectAction(template as ApplyEffectActionObject);
                        break;
                }
                return action;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error creating action: {e}");
                return null;
            }
        }
    }

    /// <summary>
    /// Structure to feed use info into a spell.
    /// </summary>
    public struct ActionArgs
    {
        public Character Caster;
        public Character Target;
        public InputArgs Inputs;

        public ActionArgs(Character caster, Character target, InputArgs inputs)
        {
            Caster = caster;
            Target = target;
            Inputs = inputs;
        }
    }

    [Serializable]
    public struct SpellAction
    {
        public ActionObject action;
        public float delayTime;

        public SpellAction(ActionObject action, float delay)
        {
            this.action = action;
            delayTime = delay;
        }
    }
}