using UnityEngine;
using UnityEngine.SceneManagement;

namespace FroggyDefense.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void LoadLevel(string name)
        {
            try
            {
                SceneManager.LoadScene(name);
            }
            catch
            {
                Debug.LogWarning("Scene \"" + name + "\" could not be found.");
            }
        }
    }
}