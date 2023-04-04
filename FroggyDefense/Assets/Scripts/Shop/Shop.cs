using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Core;

namespace FroggyDefense.Shop
{
    public class Shop : MonoBehaviour, IInteractable
    {
        public GameObject _storeItemUIPrefab = null;

        [Space]
        [Header("Interact Events")]
        [Space]
        public UnityEvent InteractEvent;

        public void Interact()
        {
            InteractEvent?.Invoke();
        }
    }
}