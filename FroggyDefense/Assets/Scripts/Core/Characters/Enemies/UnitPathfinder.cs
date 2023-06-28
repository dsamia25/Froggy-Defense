using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinder;

namespace FroggyDefense.Core.Enemies
{
    public class UnitPathfinder : MonoBehaviour
    {
        public bool DrawPath = false;
        public GameObject _pathMarkerPrefab = null;
        public LineRenderer lineRenderer = null;

        private List<Vector2> path = new List<Vector2>();
        private List<GameObject> pathMarkers = new List<GameObject>();
        private Enemy _enemy = null;

        private Vector2Int start = Vector2Int.zero;
        private Vector2Int end = Vector2Int.zero;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        /// <summary>
        /// Recalculates the current path.
        /// </summary>
        public void FindPath()
        {
            try
            {
                start = Vector2Int.RoundToInt(transform.position);
                end = Vector2Int.RoundToInt(_enemy.Focus.transform.position);
                path = TilePathfinder.FindShortestPath(BoardManager.instance.PathfinderMap, start, end, _enemy.WalkableTiles);
                if (DrawPath)
                {
                    DrawPathMarkers();
                }

            } catch (Exception e)
            {
                Debug.LogWarning($"Error finding path: {e}");
                if (path == null)
                {
                    path = new List<Vector2>();
                }
                path.Clear();
            }
        }

        /// <summary>
        /// Clears the visual path.
        /// </summary>
        public void ClearPathMarkers()
        {
            try
            {
                if (pathMarkers != null)
                {
                    foreach (GameObject obj in pathMarkers)
                    {
                        Destroy(obj);
                    }
                }
                lineRenderer.positionCount = 0;
                pathMarkers = null;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error clearing path markers: {e}");
            }
        }

        /// <summary>
        /// Draws the visual path.
        /// </summary>
        public void DrawPathMarkers()
        {
            try
            {
                // Clear markers.
                ClearPathMarkers();
                pathMarkers = new List<GameObject>();

                lineRenderer.positionCount = path.Count;

                // Create visual test markers.
                for (int i = 0; i < path.Count; i++)
                {
                    Vector2 pos = path[i];
                    Vector2 adjustedPos = new Vector2(pos.x, pos.y);
                    pathMarkers.Add(Instantiate(_pathMarkerPrefab, adjustedPos, Quaternion.identity));
                    lineRenderer.SetPosition(i, path[i]);
                }
            } catch (Exception e)
            {
                Debug.LogWarning($"Error drawing path markers: {e}");
            }
        }
    }
}