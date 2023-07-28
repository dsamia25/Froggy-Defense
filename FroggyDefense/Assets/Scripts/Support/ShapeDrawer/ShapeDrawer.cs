using System;
using UnityEngine;

namespace ShapeDrawer
{
    public abstract class ShapeDrawer : MonoBehaviour
    {
        [SerializeField] protected Mesh mesh;

        public Shape shape;

        public float LineWidth;
        public bool isFilled;

        public Vector3[] Vertices;
        public Vector2[] Uvs;
        public int[] Triangles;

        private void Awake()
        {
            CreateMeshFilter();
        }

        protected void CreateMeshFilter()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        public abstract void DrawFilledShape();
        public abstract void DrawHollowShape();
        public abstract void EraseShape();

        protected Vector2[] GetUvs()
        {
            Vector2[] uvs = new Vector2[Vertices.Length];
            for(int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(Vertices[i].x, Vertices[i].y);
            }
            return uvs;
        }
    }

    public enum eShape
    {
        Circle = 32,
        Rectangle = 4
    }

    [Serializable]
    public struct Shape
    {
        public eShape Type;
        public int Sides => (int)Type;
        public Vector2 Dimensions;

        public Shape(eShape type, Vector2 dimensions)
        {
            Type = type;
            //Sides = sides;
            Dimensions = dimensions;
        }
    }
}