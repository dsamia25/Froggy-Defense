using UnityEngine;
using UnityEngine.UI;

namespace FroggyDefense.Core
{
    public class TimeBar : MonoBehaviour
    {
        [SerializeField] private RawImage fill;
        [SerializeField] private float offSet = .5f;

        private void Start()
        {
            UpdateFill();
        }

        private void Update()
        {
            UpdateFill();
        }

        /// <summary>
        /// Offsets the fill image to move the gradient along.
        /// </summary>
        private void UpdateFill()
        {
            var rect = fill.uvRect;
            rect.x = DayCycle.instance.TotalTime / (2 * DayCycle.instance.DayCycleTime) + offSet;
            fill.uvRect = rect;
        }
    }
}
