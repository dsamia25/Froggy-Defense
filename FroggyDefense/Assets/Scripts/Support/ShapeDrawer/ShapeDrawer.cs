using UnityEngine;

namespace ShapeDrawer
{
    public abstract class ShapeDrawer : MonoBehaviour
    {
        [SerializeField] protected Mesh mesh;

        public int Sides;
        public float Width;
        public float LineWidth;
        public bool isFilled;

        public Vector3[] Vertices;
        public Vector2[] Uvs;
        public int[] Triangles;

        private void Start()
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
}