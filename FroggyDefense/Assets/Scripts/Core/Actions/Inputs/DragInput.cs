using System;
using Core.Spells;
using UnityEngine;
using ShapeDrawer;

namespace Core.Actions.Inputs
{
    public class DragInput : ActionInput
    {
        // Shape drawers to show the ability range and selected area when looking for inputs.
        [SerializeField] private PolygonDrawer RangeOverlay;
        [SerializeField] private PolygonDrawer TargetOverlay;

        // How long the drag command has been held down.
        protected float HoldTime = 0;

        // Locations of where the drag started and ended.
        protected Vector3 StartPos;
        protected Vector3 EndPos;

        private bool StartedDrag = false;

        private void Update()
        {
            if (!IsActive) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // TODO: Update target overlay.
            //MoveTargetOverlay();

            if (Input.GetMouseButtonUp(0))
            {
                // Drag ending.
                Debug.Log($"Confirmed input drag at ({TargetOverlay.transform.position}).");
                EndPos = TargetOverlay.transform.position;
                Confirm();
            } else if (Input.GetMouseButtonDown(0))
            {
                // Drag starting.
                StartedDrag = true;
                Debug.Log($"Starting input drag at ({TargetOverlay.transform.position}).");
                StartPos = TargetOverlay.transform.position;
            } else if (Input.GetMouseButtonDown(1))
            {
                // Cancel drag.
                Debug.Log($"Cancelled input drag.");
                Cancel();
            }
        }

        public override bool Activate(Spell spell, InputCallBack callBack)
        {
            if (InputListenerActive && !IsActive)
            {
                return false;
            }

            StartedDrag = false;
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
                ConfirmInputCallBack?.Invoke(new InputArgs(StartPos, EndPos));
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
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning($"Error loading spell range preview: {e}");
            }
        }

        /// <summary>
        /// Draws the ability effect shape.
        /// </summary>
        private void DrawInputTargetOverlay()
        {
            try
            {
                //TargetOverlay.shape = SelectedSpell.EffectShape;
                TargetOverlay.shape = new Shape(eShape.Rectangle, SelectedSpell.EffectShape.Dimensions);
                TargetOverlay.DrawFilledShape();
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning($"Error loading spell target preview: {e}");
            }
        }

        /// <summary>
        /// Moves the ability effect preview to the input position.
        /// </summary>
        private void MoveTargetOverlay(Vector3 pos, float maxDistance)
        {
            if (StartedDrag)
            {
                TargetOverlay.shape = new Shape(eShape.Rectangle, SelectedSpell.EffectShape.Dimensions);

                TargetOverlay.transform.rotation = Quaternion.Euler(0f, 0f, ActionUtils.AngleBetweenTwoPoints(pos, transform.position));
            }

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