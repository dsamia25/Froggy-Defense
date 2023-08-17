using UnityEngine;

namespace FroggyDefense.Dungeons
{
    public class SpawnSigil : MonoBehaviour
    {
        public Animator animator;
        public Material randomName;
        public float fadeTime = .5f;                    // Time to fade in and out.
        public float burnAnimationDelay = 1f;           // Delay before the burn animation.
        public float burnAnimationTime = 1f;            // Time for burn animation to complete.
        public Vector2 chalkScaleRange = new Vector2(25f, 35f);
        public Vector2 burnScaleRange = new Vector2(4.5f, 5.5f);

        private void Awake()
        {
            Debug.Log($"Awake Sigil Start");
            transform.Rotate(new Vector3(50, 0, 0), Space.World);
        }

        public void CleanUp()
        {
            Destroy(gameObject);
        }
    }
}
