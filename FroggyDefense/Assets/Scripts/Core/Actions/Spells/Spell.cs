using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Actions.Inputs;
using FroggyDefense.Core.Actions;
using ShapeDrawer;

namespace FroggyDefense.Core.Spells
{
    /// <summary>
    /// Enum of all kinds of damage.
    /// </summary>
    public enum SpellSchool
    {
        Fire,
        Frost,
        Spirit,
        Earth
    }

    public abstract class Spell
    {
        public SpellObject Template;

        public SpellType Type => Template.Type;
        public string Name => Template.Name;
        public int SpellId => Template.SpellId;
        public float Cooldown => Template.Cooldown;
        public float ManaCost => Template.ManaCost;

        public Shape EffectShape => Template.EffectShape;
        public float TargetRange => Template.TargetRange;
        public InputMode TargetMode => Template.TargetMode;

        public SpellAction[] SpellActions => Template.Actions;           // List of actions the spell should take.
        public Dictionary<int, Actions.Action> ActionIndex;

        //public float Damage => Template.Damage;
        //public DamageType SpellDamageType => Template.SpellDamageType;
        //public bool AppliedEffect => Template.AppliedEffects.Length > 0;
        //public bool CreatesDamageZone => Template.CreatesDamageArea;

        private float _currCooldown;
        public float CurrCooldown { get => _currCooldown; set => _currCooldown = value; }

        public bool OnCooldown => (_currCooldown > 0);

        public List<Collider2D> CollisionList; // Reusable list for Physics2D.Overlap[Circle/Box]

        /// <summary>
        /// Creates a Spell object of the correct inherited type.
        /// Inputing a ProjectileSpellObject will create a ProjectileSpell spell,
        /// AreaSpellObject -> SpellArea,
        /// TargetSpellObject -> TargetSpell,
        /// ...
        /// </summary>
        /// <returns></returns>
        public static Spell CreateSpell(SpellObject template)
        {
            try
            {
                Spell spell = null;
                switch (template.Type)
                {
                    case SpellType.Area:
                        spell = new AreaSpell(template);
                        break;
                    case SpellType.Projectile:
                        spell = new ProjectileSpell(template);
                        break;
                    case SpellType.Targeted:
                        spell = new TargetedSpell(template);
                        break;
                    default:
                        Debug.LogWarning($"Error creating spell: Unknown spell type.");
                        return null;
                }
                return spell;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error creating spell: {e}");
                return null;
            }
        }

        /// <summary>
        /// Finds the generated action to use and resolves it.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual IEnumerator ResolveAction(SpellAction action, ActionArgs args)
        {
            Actions.Action ac = GetAction(action.action);
            if (ac != null)
            {
                yield return new WaitForSeconds(action.delayTime);
                ac.Resolve(args);
            }
        }

        /// <summary>
        /// Builds the spell effect using the SpellObject's parameters.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool Cast(ActionArgs args)
        {
            if (args.Caster.Mana < ManaCost)
            {
                Debug.Log("Cannot cast spell. " + Name + " needs " + ManaCost + " mana. (" + (ManaCost - GameManager.instance.m_Player.Mana).ToString("0.00") + " more needed).");
                return false;
            }

            // Foreach Action, create an action Coroutine with the input delay (Can make blocking actions later).
            foreach (SpellAction action in SpellActions)
            {
                //StartCoroutine();
                // TODO: Need to make a way to start a coroutine or equivalent kind of delay.
                Debug.Log($"Starting action \"{action.action.name}\" ({action.action.ActionId}). Delayed {action.delayTime} seconds");
            }

            args.Caster.UseMana(ManaCost);
            _currCooldown = Cooldown;
            return true;
        }

        /// <summary>
        /// Gets the created action for the spell to use.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private Actions.Action GetAction(ActionObject action)
        {
            try
            {
                if (!ActionIndex.ContainsKey(action.ActionId))
                {
                    ActionIndex.Add(action.ActionId, Actions.Action.CreateAction(action));
                }

                return ActionIndex[action.ActionId];
            } catch (Exception e)
            {
                Debug.LogWarning($"Error getting spell action: {e}");
                return null;
            }
        }

        ///// <summary>
        ///// Builds the spell effect using the SpellObject's parameters.
        ///// </summary>
        ///// <param name="args"></param>
        ///// <returns></returns>
        //public virtual bool Cast(ActionArgs args)
        //{
        //    if (args.Caster.Mana < ManaCost)
        //    {
        //        Debug.Log("Cannot cast spell. " + Name + " needs " + ManaCost + " mana. (" + (ManaCost - GameManager.instance.m_Player.Mana).ToString("0.00") + " more needed).");
        //        return false;
        //    }

        //    if (Type == SpellType.Area)
        //    {
        //        // TODO: Make a call to a SplashAction
        //        var targetAmount = ActionUtils.GetTargets(args.Inputs.point1, EffectShape, Template.TargetLayer, _overlapTargetList);
        //        Debug.Log($"Cast: Found {targetAmount} targets. {_overlapTargetList.Count} in list.");
        //                foreach (var collider in _overlapTargetList)
        //                {
        //                    IDestructable target = null;
        //                    if ((target = collider.gameObject.GetComponent<IDestructable>()) != null)
        //                    {
        //                        target.TakeDamage(new DamageAction(args.Caster, Damage, SpellDamageType));
        //                        //if (AppliesDot)
        //                        //{
        //                        //target.ApplyDot(new DamageOverTimeEffect(args.Caster, target, Template.AppliedOverTimeEffect.Name, Template.AppliedOverTimeEffect.DamagePerTick, Template.AppliedOverTimeEffect.EffectDamageType, Template.AppliedOverTimeEffect.Ticks, Template.AppliedOverTimeEffect.TickFrequency));
        //                        //}
        //                        //if (AppliesStatusEffect)
        //                        //{
        //                        //    target.ApplyStatusEffect(new StatusEffect(args.Caster, target, Template.AppliedStatusEffect));
        //                        //}
        //                        // TODO: Do a foreach effect in appliedEffects, apply the effect.
        //                    }
        //}

        //        if (CreatesDamageZone)
        //        {
        //            // OLD WAY
        //            //var damageArea = GameObject.Instantiate(GameManager.instance.m_DamageAreaPrefab, args.Inputs.point1, Quaternion.identity);
        //            //damageArea.GetComponent<DamageArea>().Init(Template.CreatedDamageArea);

        //            // NEW WAY
        //            ActionUtils.CreateDamageArea(args.Inputs.point1, Template.CreatedDamageArea);
        //        }

        //        Debug.Log($"Casting {Name} as an AOE Spell. Damaged {targetAmount} targets.");
        //    }
        //    else if (Type == SpellType.Projectile)
        //    {
        //        Debug.Log($"Casting {Name} as a Projectile Spell.");
        //    }
        //    else if (Type == SpellType.Targeted)
        //    {
        //        Debug.Log($"Casting {Name} as a Targeted Spell.");
        //    }
        //    else
        //    {
        //        Debug.Log($"ERROR: Casting {Name} as an unknown spell type ({Type.ToString()}).");
        //        return false;
        //    }

        //    args.Caster.UseMana(ManaCost);
        //    _currCooldown = Cooldown;
        //    return true;
        //}
    }
}