using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Weapons;
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
        /// Fires a projectile.
        /// </summary>
        /// <returns></returns>
        public static bool FireProjectile(ProjectileInfo projectile, Character caster, Vector2 fireLoc, Vector2 fireDir)
        {
            try
            {
                Projectile proj = ProjectileManager.instance.GetProjectile(projectile);
                proj.transform.position = fireLoc;
                proj.Shoot(caster, fireDir);
                return true;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error firing projectile: {e}");
                return false;
            }
        }

        public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b, float angleOffset)
        {
            return (Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg) - angleOffset;
        }
    }
}
