using UnityEngine;

namespace Support
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
        where T : SingletonMonoBehaviour<T>
    {
        public static T Instance { get; private set; }
    
        protected void Awake()
        {
            if (Instance == null)
            {
                Debug.Log($"{this.GetType().Name}: Initializing singleton.");
                Instance = (T)this;
            }
            else
            {
                Debug.Log($"{this.GetType().Name}: Already an instance of this singleton.");
                Destroy(this);
            }
        }
    }
}