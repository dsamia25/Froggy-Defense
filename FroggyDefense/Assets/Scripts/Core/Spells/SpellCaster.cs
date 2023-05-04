using UnityEngine;
using ShapeDrawer;

namespace FroggyDefense.Core.Spells
{
    public class SpellCaster : MonoBehaviour
    {
        public Spell SelectedSpell;
        public SpellCasterCallback SpellUsedCallback;

        public PolygonDrawer RangeShape;
        public PolygonDrawer EffectShape;

        public delegate void SpellCasterCallback();

        private void Update()
        {
            if (SelectedSpell != null)
            {
                UpdateTargetInputUI();

                if (Input.GetMouseButtonDown(0))
                {
                    SelectedSpell.Cast(Input.mousePosition);
                    SpellUsedCallback?.Invoke();
                }
            }    
        }

        /// <summary>
        /// Starts getting inout for the spell
        /// </summary>
        /// <param name="spell"></param>
        public void Cast(Spell spell, SpellCasterCallback callback)
        {
            SelectedSpell = spell;
            SpellUsedCallback = callback;
            CreateTargetInputUI();
        }

        public void CreateTargetInputUI()
        {
            RangeShape.Width = SelectedSpell.SpellRange;
            EffectShape.Width = SelectedSpell.EffectRadius;
        }

        public void UpdateTargetInputUI()
        {
            // TODO: Make the effect follow the mouse but get clamped into the SpellRange.
            Vector3 pos = Input.mousePosition;
            pos.z = 0;
            EffectShape.transform.position = pos;
        }
    }
}