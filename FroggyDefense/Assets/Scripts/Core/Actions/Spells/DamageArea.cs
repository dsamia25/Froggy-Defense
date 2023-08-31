using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Actions;
using ShapeDrawer;

namespace FroggyDefense.Core.Spells
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private ShapeDrawer.ShapeDrawer _effectAreaDrawer;

        public DamageAreaBuilder Template;
        public string Name => Template.Name;                                    // The effect's name.
        public int Ticks => Template.Ticks;                                     // The total amount of ticks the effect does.
        public int TicksLeft { get; private set; }                              // The amount of ticks the effect has left.
        public float DamagePerTick => Template.DamagePerTick;                   // The amount of damage applied each tick.
        public float TickFrequency => Template.TickFrequency;                   // How frequent the dot ticks in seconds.
        public DamageType EffectDamageType => Template.EffectDamageType;        // What type of damage the effect does.
        public Shape EffectShape => Template.EffectShape;
        public Character Caster { get; private set; }                           // Who applied the effect.

        public bool AppliesStatusEffect => Template.AppliedEffects.Length > 0;

        public float TotalDamage => Ticks * DamagePerTick;          // The total amount of damage the effect will do over its duration.
        public float DamageLeft => TicksLeft * DamagePerTick;       // The amount of damage the effect will apply over the rest of its duration.

        protected List<Collider2D> _overlapTargetList; // Reusable list for Physics2D.Overlap[Circle/Box]
        private float _currTickCooldown;

        public void Init(DamageAreaBuilder template)
        {
            Template = template;
            TicksLeft = template.Ticks;

            _effectAreaDrawer.shape = template.EffectShape;
            _effectAreaDrawer.DrawFilledShape();

            _currTickCooldown = Template.TickFrequency;
            _overlapTargetList = new List<Collider2D>();
        }

        private void Update()
        {
            Tick();

            if (TicksLeft <= 0)
            {
                // TODO: Do any additional cleanup too.
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Applies the damage and effect to all effected targets.
        /// returns null.
        /// </summary>
        /// <returns></returns>
        public void Tick()
        {
            if (_currTickCooldown <= 0)
            {
                ActionUtils.GetTargets(transform.position, Template.EffectShape, Template.TargetLayer, _overlapTargetList);
                foreach (var collider in _overlapTargetList)
                {
                    IDestructable target = null;
                    if ((target = collider.gameObject.GetComponent<IDestructable>()) != null)
                    {
                        target.TakeDamage(new DamageAction(Caster, DamagePerTick, EffectDamageType, false));
                        
                        foreach (AppliedEffectObject effect in Template.AppliedEffects)
                        {
                            // TODO: Replace this apply with args. Make args saved in DamageArea.
                            //target.ApplyEffect(AppliedEffect.CreateAppliedEffect(effect, Caster, target));
                        }
                    }
                }
                _currTickCooldown = TickFrequency;
                TicksLeft--;
            }
            else
            {
                _currTickCooldown -= Time.deltaTime;
            }
        }
    }

    [Serializable]
    public class DamageAreaBuilder
    {
        public string Name = "DAMAGE AREA";
        public int Ticks = 1;
        public float DamagePerTick = 1;
        public float TickFrequency = 1;
        public DamageType EffectDamageType;
        public Shape EffectShape;
        public LayerMask TargetLayer;           // The layer the targets are on.

        //public bool AppliesStatusEffect;
        //public StatusEffectBuilder AppliedStatusEffect;             // List of applied status effects.
        public AppliedEffectObject[] AppliedEffects;
    }
}