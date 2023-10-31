using System;
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

    [Serializable]
    public abstract class Spell
    {
        public SpellObject Template;

        public SpellType Type => Template.Type;
        public SpellSchool School { get; protected set; }
        public string Name => Template.Name;
        public int SpellId => Template.SpellId;
        public float Cooldown => Template.Cooldown;
        public float ManaCost => Template.ManaCost;
        public int SpellCardCharges => Template.SpellCardCharges;
        public float SpellCardChargeExpirationTime => Template.SpellCardChargeExpirationTime;

        public Shape EffectShape => Template.EffectShape;
        public float TargetRange => Template.TargetRange;
        public InputMode TargetMode => Template.TargetMode;

        public SpellAction[] SpellActions => Template.Actions;           // List of actions the spell should take.
        public Dictionary<int, Actions.Action> ActionIndex;

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
                Actions.Action ac = GetAction(action.action);
                if (ac != null)
                {
                    args.Caster.StartCoroutine(ActionUtils.ResolveAction(ac, action.delayTime, args));
                }
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
    }
}