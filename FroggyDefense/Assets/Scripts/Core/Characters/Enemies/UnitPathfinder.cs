using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinder;

namespace FroggyDefense.Core.Enemies
{
    public class UnitPathfinder : MonoBehaviour
    {
        public bool DrawPath = false;
        public LineRenderer lineRenderer = null;
        public GameObject PathfinderNodePrefab = null;

        public float PathNodeResetTime = .5f;
        private float _pathNodeResetTime = 0f;

        [HideInInspector]
        public GameObject PathNode = null;

        private List<Vector2> path = new List<Vector2>();
        private Enemy _enemy = null;

        private Vector2Int start = Vector2Int.zero;
        private Vector2Int end = Vector2Int.zero;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void Update()
        {
            if (_pathNodeResetTime <= 0)
            {
                if (path != null)
                {
                    if (path.Count > 1)
                    {
                        PathNode.transform.position = path[1];
                    }
                    else if (path.Count > 0)
                    {
                        PathNode.transform.position = path[0];
                    }
                }
                _pathNodeResetTime = PathNodeResetTime;
            } else
            {
                _pathNodeResetTime -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Gets the next position to move towards in the path.
        /// </summary>
        /// <returns></returns>
        public Vector2 FindPath()
        {
            if (path == null || path.Count == 0)
            {
                SetPath();
            }

            try
            {
                if (PathNode == null)
                {
                    return transform.position;
                }
                return PathNode.transform.position;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error finding path: {e}");
                return transform.position;
            }
        }

        /// <summary>
        /// Recalculates the current path.
        /// </summary>
        public void SetPath()
        {
            try
            {
                start = Vector2Int.RoundToInt(transform.position);
                end = Vector2Int.RoundToInt(_enemy.Focus.transform.position);
                path = TilePathfinder.FindShortestPath(BoardManager.instance.PathfinderMap, start, end, _enemy.WalkableTiles);
                if (PathNode == null)
                {
                    if (path.Count > 1)
                    {
                        PathNode = Instantiate(PathfinderNodePrefab, path[1], Quaternion.identity);
                    }
                    else if (path.Count > 0)
                    {
                        PathNode = Instantiate(PathfinderNodePrefab, path[0], Quaternion.identity);
                    }
                }
                if (DrawPath)
                {
                    DrawPathLine();
                }

            } catch (Exception e)
            {
                Debug.LogWarning($"Error setting path: {e}");
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
                lineRenderer.positionCount = 0;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error clearing path markers: {e}");
            }
        }

        /// <summary>
        /// Draws the visual path.
        /// </summary>
        public void DrawPathLine()
        {
            try
            {
                // Clear markers.
                ClearPathMarkers();

                lineRenderer.positionCount = path.Count;

                // Create visual test markers.
                for (int i = 0; i < path.Count; i++)
                {
                    Vector2 pos = path[i];
                    Vector2 adjustedPos = new Vector2(pos.x, pos.y);
                    lineRenderer.SetPosition(i, path[i]);
                }
            } catch (Exception e)
            {
                Debug.LogWarning($"Error drawing path markers: {e}");
            }
        }

        /// <summary>
        /// Moves the path node to move towards when it reaches it.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == PathNode)
            {
                FindPath();

                if (path.Count > 1)
                {
                    PathNode.transform.position = path[1];
                } else if (path.Count > 0)
                {
                    PathNode.transform.position = path[0];
                }
            }
        }

        private void OnDestroy()
        {
            if (PathNode != null)
            {
                Destroy(PathNode);
            }
        }
    }
}