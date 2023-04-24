using UnityEngine;

namespace FroggyDefense.Core
{
    public interface IGroundInteractable
    {
        public void Interact(GameObject user);

        public bool PickUp(GameObject user);

        public void Launch(Vector2 vector);
    }
}