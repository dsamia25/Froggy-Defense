using UnityEngine;
using FroggyDefense.Interactables;
using UnityEngine.Events;

namespace FroggyDefense.Core
{
    public class GemManager : MonoBehaviour
    {
        public GameObject GemDropperPrefab;

        [Space]
        [Header("Gems")]
        [Space]
        public GemValueChartObject GemChart;
        [SerializeField] private int _gems = 0;
        public int Gems
        {
            get => _gems;
            set
            {
                if (value < 0)
                {
                    _gems = 0;
                }
                _gems = value;
                GemsChangedEvent?.Invoke(_gems);
            }
        }

        [Space]
        [Header("Events")]
        public UnityEvent<int> GemsChangedEvent;

        private void Start()
        {
            // Subscribe to events.
            Gem.PickedUpEvent += OnGemPickedUpEvent;
            DropGems.DropGemsEvent += OnDropGemsEvent;
        }

        private void OnGemPickedUpEvent(GemEventArgs args)
        {
            Gems += args.value;
        }

        private void OnDropGemsEvent(DropGemsEventArgs args)
        {
            Debug.Log("Spawning GemDropper.");
            GameObject dropper = Instantiate(GemDropperPrefab, args.pos, Quaternion.identity);
            dropper.transform.SetParent(transform);
            dropper.GetComponent<GemDropper>().Drop(args.gems);
        }
    }
}