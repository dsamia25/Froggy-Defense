using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense
{
    public class PointAtMouse : MonoBehaviour
    {
        public float AngleOffset = 90;

        //*******************************************************
        // Update
        //*******************************************************

        private void FixedUpdate()
        {
            Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            transform.rotation = Quaternion.Euler(0f, 0f, ActionUtils.AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen, AngleOffset));
        }
    }
}
