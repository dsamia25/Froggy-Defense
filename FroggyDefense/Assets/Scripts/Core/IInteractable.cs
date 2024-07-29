using UnityEngine;

namespace Core
{
    public interface IInteractable
    {
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Trigger's the interactable's interact affect.
        /// </summary>
        /// <param name="user"></param>
        public virtual void Interact(Character user)
        {
            if (!IsEnabled) return;
        }

        /// <summary>
        /// Toggles the interactable on and off.
        /// </summary>
        public void ToggleEnabled()
        {
            IsEnabled = !IsEnabled;
        }

        /// <summary>
        /// Turns on being able to interact with the object.
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;
        }
        
        /// <summary>
        /// Turns off being able to interact with the object.
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;
        }
    }
}