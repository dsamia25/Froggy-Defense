using UnityEngine;

namespace ShapeDrawer
{
    public class PolygonDrawer : ShapeDrawer
    {
        public override void DrawFilledShape()
        {
            Vertices = GetCircumferencePoints(Sides, Width);
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
            mesh.Clear();
            Vertices = null;
            Triangles = null;
        }

        /// <summary>
        /// Gets the vertex points of a polygon.
        /// More sides should get closer to a circle.
        /// </summary>
        /// <param name="sides"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Vector3[] GetCircumferencePoints(int sides, float radius)
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