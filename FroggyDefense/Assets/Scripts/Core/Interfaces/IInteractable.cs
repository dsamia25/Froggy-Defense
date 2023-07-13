using UnityEngine;

namespace FroggyDefense.Core
{
    public interface IInteractable
    {
        public bool IsInteractable { get; set; }

        // TODO: Fix a lot of the user's of this so that they pass in the client's player.
        public virtual void Interact(GameObject user) { }
    }
}