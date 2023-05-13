using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            transform.rotation = Quaternion.Euler(0f, 0f, AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen) - AngleOffset);
        }

        //*******************************************************
        // Methods
        //*******************************************************

        private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }
    }
}
