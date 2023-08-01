using UnityEngine;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core.Actions
{
    /// <summary>
    /// Struct for passing inputs for actions.
    /// </summary>
    public struct InputArgs
    {
        public Vector3 point1;
        public Vector3 point2;

        public InputArgs(Vector3 p1, Vector3 p2)
        {
            point1 = p1;
            point2 = p2;
        }
    }

    public abstract class ActionInput : MonoBehaviour
    {
        // If there is any currently active input listener.
        public static bool InputListenerActive { get; protected set; }

        // If this specific input listener is currently active.
        public bool IsActive { get; protected set; }

        // The spell currently looking for inputs.
        public Spell SelectedSpell { get; protected set; }

        // Callback to confirm the inputs.
        public delegate void InputCallBack(InputArgs args);
        protected InputCallBack ConfirmInputCallBack;

        /// <summary>
        /// Activates the action input to listen for player inputs.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public abstract bool Activate(Spell spell, InputCallBack callback);

        /// <summary>
        /// Confirms the inputs and returns the mto the callback.
        /// </summary>
        public abstract void Confirm();

        /// <summary>
        /// Cancels looking for an input.
        /// </summary>
        /// <returns></returns>
        public abstract void Cancel();
    }
}