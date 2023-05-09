using System;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    public class Spell
    {
        public SpellObject Template;

        public SpellType Type => Template.Type;
        public string Name => Template.Name;
        public int SpellId => Template.SpellId;
        public float Cooldown => Template.Cooldown;
        public float ManaCost => Template.ManaCost;

        public float EffectRadius => Template.EffectRadius;
        public float TargetRange => Template.TargetRange;

        public float Damage => Template.Damage;
        public DamageType SpellDamageType => Template.SpellDamageType;
        public bool AppliesDot => Template.AppliesDot;
        public bool CreatesDamageZone => Template.CreatesDamageArea;

        private float _currCooldown;
        public float CurrCooldown { get => _currCooldown; set => _currCooldown = value; }

        public Spell(SpellObject template)
        {
            Template = template;
        }

        /// <summary>
        /// Builds the spell effect using the SpellObject's parameters.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Cast(SpellArgs args)
        {
            if (_currCooldown > 0)
            {
                Debug.Log("Cannot cast spell. " + Name + " still on cooldown. (" + _currCooldown.ToString("0.00") + " seconds remaining).");
                return false;
            }

            if (GameManager.instance.m_Player.m_Mana < ManaCost)
            {
                Debug.Log("Cannot cast spell. " + Name + " needs " + ManaCost + " mana. (" + (ManaCost - GameManager.instance.m_Player.m_Mana).ToString("0.00") + " more needed).");
                return false;
            }

            if (Type == SpellType.AOE)
            {
                var targets = GetTargets(args.Position, EffectRadius, Template.TargetLayer);
                foreach (var collider in targets)
                {
                    IDestructable target = null;
                    if ((target = collider.gameObject.GetComponent<IDestructable>()) != null)
                    {
                        target.TakeDamage(new DamageAction(args.Caster, Damage, SpellDamageType));
                        if (AppliesDot)
                        {
                            target.ApplyDot(new DamageOverTimeEffect(args.Caster, target, Template.AppliedOverTimeEffect.Name, Template.AppliedOverTimeEffect.DamagePerTick, Template.AppliedOverTimeEffect.EffectDamageType, Template.AppliedOverTimeEffect.Ticks, Template.AppliedOverTimeEffect.TickFrequency));
                        }
                    }
                }

                if (CreatesDamageZone)
                {
                    var damageArea = GameObject.Instantiate(GameManager.instance.m_DamageAreaPrefab, args.Position, Quaternion.identity);
                    damageArea.GetComponent<DamageArea>().Init(Template.CreatedDamageArea);
                }

                Debug.Log("Casting " + Name + " as an AOE Spell. Damaged " + targets.Length + " targets.");
            }
            else if (Type == SpellType.Targeted)
            {
                Debug.Log("Casting " + Name + " as a Targeted Spell.");
            } else
            {
                Debug.Log("ERROR: Casting " + Name + " as an unknown spell type (" + Type.ToString() + ").");
                return false;
            }

            _currCooldown = Cooldown;
            return true;
        }

        /// <summary>
        /// Returns all colliders in the area.
        /// Not in any particular order.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="targetLayer"></param>
        /// <returns></returns>
        public static Collider2D[] GetTargets(Vector2 pos, float radius, LayerMask targetLayer)
        {
            return Physics2D.OverlapCircleAll(pos, radius, targetLayer);
        }

        /// <summary>
        /// Returns all colliders in the area up to a specified amount.
        /// Not in any particular order.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="targetLayer"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Collider2D[] GetTargetsCapped(Vector2 pos, float radius, LayerMask targetLayer, int max)
        {
            var colliders = Physics2D.OverlapCircleAll(pos, radius, targetLayer);

            // Return this if less than the max amount.
            if (colliders.Length < max)
            {
                return colliders;
            }

            // Return only the input amount from all the targets.
            Collider2D[] capped = new Collider2D[max];
            Array.Copy(colliders, capped, max);

            return capped;
        }
    }
}