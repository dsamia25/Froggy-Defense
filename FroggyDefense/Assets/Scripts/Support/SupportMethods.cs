using UnityEngine;

namespace FroggyDefense.Support
{
    public static class SupportMethods
    {
        /// <summary>
        /// Returns the first collider found at the position, in the radius, in the layer.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static Collider2D GetCollider(Vector2 pos, float radius, LayerMask layer)
        {
            // Cast circle around click to find interactables -> then select object if found one.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, layer);

            if (colliders.Length > 0)
            {
                return colliders[0];
            }
            return null;
        }

        /// <summary>
        /// Gets the mouse position.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetMousePosition()
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            return pos;
        }

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