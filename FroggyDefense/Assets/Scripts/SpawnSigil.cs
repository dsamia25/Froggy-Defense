using UnityEngine;

namespace FroggyDefense.Dungeons
{
    public class SpawnSigil : MonoBehaviour
    {
        public Animator animator = null;
        public Material burnMaterial = null;
        public float burnAnimationDelay = 1f;
        public float burnAnimationTime = 1f;
        public Vector2 chalkScaleRange = new Vector2(25f, 35f);
        public Vector2 burnScaleRange = new Vector2(4.5f, 5.5f);

        private void Awake()
        {
            Debug.Log($"Awake Sigil Start");
            transform.rotation = new Quaternion(30, 0, 0, 0);
        }

        public void CleanUp()
        {
            Destroy(gameObject);
        }
    }
}
