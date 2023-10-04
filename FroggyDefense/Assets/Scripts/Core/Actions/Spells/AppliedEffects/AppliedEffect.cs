using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Core.Spells
{
    public enum AppliedEffectType
    {
        Slow,
        Stun,
        DamageOverTime
    }

    public enum StatusSchool
    {
        Magic,
        Curse,
        Bleed,
        Poison
    }

    [Serializable]
    public abstract class AppliedEffect
    {
        protected AppliedEffectObject Template { get; set; }

        public string Name { get; protected set; } = "APPLIED EFFECT";                  // This object's name.
        public DamageActionArgs EffectArgs { get; protected set; }                      // Damage or slow amount, also has crit info and damage type.
        public float EffectTime { get; protected set; } = 1;                            // Ticks or seconds.
        public float Frequency { get; protected set; } = 1;                             // Tick Frequency or countdown speed modifier.
        public AppliedEffectType Effect { get; protected set; }                         // What the intended effect is. (DOT, slow, stun).
        public StatusSchool School { get; protected set; }                              // Which kind of applied effect. (Curce, bleed, magic...)
        public bool IsExpired { get; protected set; } = false;                          // If this effect has expired.
        public ActionArgs Args { get; protected set; }
        public IDestructable Target { get; protected set; }

        public float Strength => EffectArgs.Damage;                                     // The relative strength of this effect. Just uses the Damage number/ slowness amount number. Could have a better way of determining this especially for stuns.

        public SpellAction[] OnTickActions => Template.OnTickActions;                   // List of actions the spell should take.
        public SpellAction[] OnExpireActions => Template.OnExpireActions;               // List of actions the spell should take.
        public Dictionary<int, Actions.Action> ActionIndex;

        public static AppliedEffect CreateAppliedEffect(AppliedEffectObject template, ActionArgs args, IDestructable target)
        {
            try
            {

                AppliedEffect effect = null;
                switch (template.Effect)
                {
                    case AppliedEffectType.DamageOverTime:
                        effect = new DamageOverTimeEffect(args, target, template, template.EffectArgs, Mathf.FloorToInt(template.EffectTime), template.Frequency);
                        break;
                    case AppliedEffectType.Slow:
                        effect = new StatusEffect(args, target, template);
                        break;
                    case AppliedEffectType.Stun:
                        effect = new StatusEffect(args, target, template);
                        break;
                    default:
                        Debug.LogWarning($"Error creating status effect: Unknown effect type.");
                        break;
                }
                return effect;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error creating applied effect: {e}");
                return null;
            }
        }

        /// <summary>
        /// Ticks down how long the effect is active.
        /// </summary>
        public abstract void Tick();

        /// <summary>
        /// Refreshs the ticks left to the full amount.
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Clears the effect.
        /// </summary>
        public abstract void Clear();

        protected virtual void OnTargetDeathEvent()
        {

        }

        /// <summary>
        /// Resolves each of the OnTickActions.
        /// </summary>
        protected virtual void ResolveOnTickActions()
        {
            // Foreach Action, create an action Coroutine with the input delay (Can make blocking actions later).
            foreach (SpellAction action in OnTickActions)
            {
                Actions.Action ac = GetAction(action.action);
                if (ac != null)
                {
                    Debug.Log($"AppliedEffect {Name} on {Target.GetGameObject().name} resolving OnTickEffect.");
                    Args.Caster.StartCoroutine(ActionUtils.ResolveAction(ac, action.delayTime, new ActionArgs(Args.Caster, Target, new Actions.Inputs.InputArgs(Target.GetGameObject().transform.position, Args.Inputs.point2), Args.CollisionList)));
                }
            }
        }

        /// <summary>
        /// Resolve each of the OnExpireActions.
        /// </summary>
        protected virtual void ResolveOnExpireActions()
        {
            // Foreach Action, create an action Coroutine with the input delay (Can make blocking actions later).
            foreach (SpellAction action in OnExpireActions)
            {
                Actions.Action ac = GetAction(action.action);
                if (ac != null)
                {
                    Debug.Log($"AppliedEffect {Name} on {Target.GetGameObject().name} resolving OnExpireEffect.");
                    Args.Caster.StartCoroutine(ActionUtils.ResolveAction(ac, action.delayTime, new ActionArgs(Args.Caster, Target, new Actions.Inputs.InputArgs(Target.GetGameObject().transform.position, Args.Inputs.point2), Args.CollisionList)));
                }
            }
        }

        /// <summary>
        /// Gets the created action for the spell to use.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected Actions.Action GetAction(ActionObject action)
        {
            try
            {
                if (!ActionIndex.ContainsKey(action.ActionId))
                {
                    ActionIndex.Add(action.ActionId, Actions.Action.CreateAction(action));
                }

                return ActionIndex[action.ActionId];
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error getting spell action: {e}");
                return null;
            }
        }
    }
}