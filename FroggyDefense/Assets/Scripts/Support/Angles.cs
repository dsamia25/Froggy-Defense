using UnityEngine;

namespace FroggyDefense.Support
{
    public static class Angles
    {
        //private void Update()
        //{
        //    Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //    Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //    transform.rotation = Quaternion.Euler(0f, 0f, AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen));
        //}

        public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        public static float UnitVectorBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return -1f;
        }
        
    }
}