using System;
using System.Collections.Generic;
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
        public int ActionId = -1;

        public abstract void Resolve(ActionArgs args);

        /// <summary>
        /// Builds an action object.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static Action CreateAction(ActionObject template)
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
        public List<Collider2D> CollisionList;

        public ActionArgs(Character caster, Character target, InputArgs inputs, List<Collider2D> list)
        {
            Caster = caster;
            Target = target;
            Inputs = inputs;
            CollisionList = list;
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