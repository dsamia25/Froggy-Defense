using System;
using UnityEngine;

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

        public float EffectRadius => Template.EffectRadius;
        public float TargetRange => Template.TargetRange;

        public float Damage => Template.Damage;
        public DamageType SpellDamageType => Template.SpellDamageType;
        public bool AppliesStatusEffect => Template.AppliesStatusEffect;
        public bool AppliesDot => Template.AppliesDot;
        public bool CreatesDamageZone => Template.CreatesDamageArea;

        private float _currCooldown;
        public float CurrCooldown { get => _currCooldown; set => _currCooldown = value; }

        public bool OnCooldown => (_currCooldown > 0);

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
                        //spell = new TargetetedSpell
                        throw new NotImplementedException();
                        break;
                    default:
                        Debug.LogWarning($"Error creating spell: Unknown spell type.");
                        return null;
                }
                return spell;
            } catch (Exception e)
            {
                Debug.Log($"Error creating spell: {e}");
                return null;
            }
        }

        /// <summary>
        /// Start the needed processes to get spell inputs.
        /// </summary>
        public virtual void StartInputProtocol()
        {
            if (_currCooldown > 0)
            {
                Debug.Log($"Cannot cast spell. {Name} still on cooldown. ({_currCooldown.ToString("0.00")} seconds remaining).");
                return;
            }

            Debug.Log($"Looking for input for {Name}.");
        }

        /// <summary>
        /// Builds the spell effect using the SpellObject's parameters.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool Cast(SpellArgs args)
        {
            if (args.Caster.Mana < ManaCost)
            {
                Debug.Log("Cannot cast spell. " + Name + " needs " + ManaCost + " mana. (" + (ManaCost - GameManager.instance.m_Player.Mana).ToString("0.00") + " more needed).");
                return false;
            }

            if (Type == SpellType.Area)
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
                        if (AppliesStatusEffect)
                        {
                            target.ApplyStatusEffect(new StatusEffect(args.Caster, target, Template.AppliedStatusEffect));
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
            else if (Type == SpellType.Projectile)
            {
                Debug.Log("Casting " + Name + " as a Projectile Spell.");
            }
            else if (Type == SpellType.Targeted)
            {
                Debug.Log("Casting " + Name + " as a Targeted Spell.");
            }
            else
            {
                Debug.Log("ERROR: Casting " + Name + " as an unknown spell type (" + Type.ToString() + ").");
                return false;
            }

            args.Caster.UseMana(ManaCost);
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