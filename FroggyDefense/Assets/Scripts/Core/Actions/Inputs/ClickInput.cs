using System;
using UnityEngine;
using FroggyDefense.Core.Spells;
using ShapeDrawer;

namespace FroggyDefense.Core.Actions
{
    public class ClickInput : ActionInput
    {
        // Shape drawers to show the ability range and selected area when looking for inputs.
        [SerializeField] private PolygonDrawer RangeOverlay;
        [SerializeField] private PolygonDrawer TargetOverlay;

        // The area where the input is recorded.
        protected Vector3 ClickPos;

        private void Update()
        {
            if (!IsActive) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            MoveTargetOverlay(mousePos, SelectedSpell.TargetRange);

            // Listen for inputs
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"Confirmed Spell at ({TargetOverlay.transform.position}).");
                ClickPos = TargetOverlay.transform.position;
                Confirm();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Debug.Log($"Cancelled Spell.");
                Cancel();
            }
        }

        public override bool Activate(Spell spell, InputCallBack callBack)
        {
            if (InputListenerActive && !IsActive)
            {
                return false;
            }

            SelectedSpell = spell;
            IsActive = true;
            InputListenerActive = true;
            ConfirmInputCallBack = callBack;

            DrawInputRangeOverlay();
            DrawInputTargetOverlay();

            return true;
        }

        public override void Confirm()
        {
            if (IsActive)
            {
                ClearOverlays();
                IsActive = false;
                InputListenerActive = false;
                SelectedSpell = null;
                ConfirmInputCallBack?.Invoke(new InputArgs(ClickPos, Vector3.zero));
                ConfirmInputCallBack = null;
            }
        }

        public override void Cancel()
        {
            if (IsActive)
            {
                ClearOverlays();
                IsActive = false;
                InputListenerActive = false;
                SelectedSpell = null;
                ConfirmInputCallBack = null;
            }
        }

        /// <summary>
        /// Draws the ability range around the player.
        /// </summary>
        private void DrawInputRangeOverlay()
        {
            try
            {
                RangeOverlay.shape = new Shape(eShape.Circle, new Vector2(SelectedSpell.TargetRange, SelectedSpell.TargetRange));
                RangeOverlay.DrawFilledShape();
            } catch (NullReferenceException e)
            {
                Debug.LogWarning($"Error loading spell range preview: {e}");
            }
        }

        /// <summary>
        /// Draws the ability effect shape.
        /// </summary>
        private void DrawInputTargetOverlay()
        {
            try {
                TargetOverlay.shape = SelectedSpell.EffectShape;
                TargetOverlay.DrawFilledShape();
            } catch (NullReferenceException e)
            {
                Debug.LogWarning($"Error loading spell target preview: {e}");
            }
        }

        /// <summary>
        /// Moves the ability effect preview to the input position.
        /// </summary>
        private void MoveTargetOverlay(Vector3 pos, float maxDistance)
        {
            float currDistance = Vector2.Distance(pos, transform.position);
            if (currDistance > maxDistance)
            {
                // Outside range area.
                TargetOverlay.transform.position = Vector2.Lerp(transform.position, pos, maxDistance / currDistance);
            }
            else
            {
                // Inside the range bounds.
                TargetOverlay.transform.position = pos;
            }
        }

        /// <summary>
        /// Erases the drawn overlay shapes.
        /// </summary>
        private void ClearOverlays()
        {
            RangeOverlay.EraseShape();
            TargetOverlay.EraseShape();
        }
    }
}