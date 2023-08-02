using System;
using UnityEngine;

namespace ShapeDrawer
{
    public class PolygonDrawer : ShapeDrawer
    {
        public float degreeOffset = 45;

        public override void DrawFilledShape()
        {
            Vertices = GetVertices(shape);
            Uvs = GetUvs();
            Triangles = DrawFilledTriangles(Vertices);

            if (mesh == null)
            {
                CreateMeshFilter();
            }

            mesh.Clear();
            mesh.vertices = Vertices;
            mesh.uv = Uvs;
            mesh.triangles = Triangles;
        }

        public override void DrawHollowShape()
        {
            throw new System.NotImplementedException();
        }

        public override void EraseShape()
        {
            try
            {
                mesh.Clear();
                Vertices = null;
                Triangles = null;
            }
            catch (Exception e)
            {
                Debug.Log($"Error erasing shape: {e}");
            }
        }

        /// <summary>
        /// Gets the vertices for shape type.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        private Vector3[] GetVertices(Shape shape)
        {
            Vector3[] points = new Vector3[shape.Sides];
            switch (shape.Type)
            {
                case eShape.Circle:
                    points = GetCirclePoints(shape.Sides, shape.Dimensions.x);
                    break;
                case eShape.Rectangle:
                    points = GetRectanglePoints(shape.Sides, shape.Dimensions);
                    break;
                default:
                    Debug.LogWarning($"Error getting vertices: Unknown shape.");
                    break;
            }
            return points;
        }

        /// <summary>
        /// Gets the vertex points of a polygon.
        /// More sides should get closer to a circle.
        /// </summary>
        /// <param name="sides"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Vector3[] GetRectanglePoints(int sides, Vector2 dimensions)
        {
            Vector3[] points = new Vector3[sides];
            points[0] = new Vector3(dimensions.x, dimensions.y);
            points[1] = new Vector3(dimensions.x, -dimensions.y);
            points[2] = new Vector3(-dimensions.x, -dimensions.y);
            points[3] = new Vector3(-dimensions.x, dimensions.y);

            return points;
        }

        /// <summary>
        /// Gets the vertex points of a polygon.
        /// More sides should get closer to a circle.
        /// </summary>
        /// <param name="sides"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Vector3[] GetCirclePoints(int sides, float radius)
        {
            Vector3[] points = new Vector3[sides];
            float radians = 2 * Mathf.PI;

            for (int i = 0; i < sides; i++)
            {
                float a = (float)i / sides * radians;
                points[i] = new Vector3(radius * Mathf.Cos(a), radius * Mathf.Sin(a), 0);
            }

            return points;
        }

        /// <summary>
        /// Finds the triangle vertices.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        private int[] DrawFilledTriangles(Vector3[] vertices)
        {
            int amount = vertices.Length - 2;
            int[] triangleCorners = new int[3 * amount];
            int index = 0;
            for (int i = 0; i < amount; i++)
            {
                triangleCorners[index++] = 0;
                triangleCorners[index++] = i + 2;
                triangleCorners[index++] = i + 1;
            }
            return triangleCorners;
        }
    }
}