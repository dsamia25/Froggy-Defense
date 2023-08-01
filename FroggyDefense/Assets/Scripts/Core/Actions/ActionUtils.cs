using System;
using System.Collections.Generic;
using UnityEngine;
using ShapeDrawer;

namespace FroggyDefense.Core {
    public static class ActionUtils
    {
        /// <summary>
        /// Returns all colliders in the area.
        /// Not in any particular order.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="targetLayer"></param>
        /// <returns></returns>
        public static int GetTargets(Vector2 pos, Shape shape, LayerMask targetLayer, List<Collider2D> targetList)
        {
            int targets = 0;
            var filter = new ContactFilter2D();
            filter.SetLayerMask(targetLayer);
            filter.useTriggers = true;
            switch (shape.Type)
            {
                case eShape.Circle:
                    targets = Physics2D.OverlapCircle(pos, shape.Dimensions.x, filter, targetList);
                    break;
                case eShape.Rectangle:
                    // TODO: Set angle. (Currently the "0").
                    targets = Physics2D.OverlapBox(pos, shape.Dimensions, 0, filter, targetList);
                    break;
                default:
                    targetList.Clear();
                    break;
            }
            Debug.Log($"GetTargets: Found {targets} targets. {targetList.Count} in list.");
            return targets;
        }

        /// <summary>
        /// Returns all colliders in the area up to a specified amount.
        /// Not in any particular order.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="targetLayer"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Collider2D[] GetTargetsCapped(Vector2 pos, float radius, LayerMask targetLayer, int max)
        {
            var colliders = Physics2D.OverlapCircleAll(pos, radius, targetLayer);

            // Return this if less than the max amount.
            if (colliders.Length < max)
            {
                return colliders;
            }

            // Return only the input amount from all the targets.
            Collider2D[] capped = new Collider2D[max];
            Array.Copy(colliders, capped, max);

            return capped;
        }

        /// <summary>
        /// Fires a physical projectile with a collider.
        /// </summary>
        /// <returns></returns>
        public static bool FirePhysicalProjectile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fires a simulated moving projectile.
        /// </summary>
        /// <returns></returns>
        public static bool FireEffectProjectile()
        {
            throw new NotImplementedException();
        }
    }
}
