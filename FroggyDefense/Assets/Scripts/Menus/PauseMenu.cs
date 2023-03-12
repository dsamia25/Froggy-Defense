using UnityEngine;
using UnityEngine.SceneManagement;

namespace FroggyDefense.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GamePaused { get; private set; }     // If the game is paused.

        public void Pause()
        {
            Time.timeScale = 0f;
            GamePaused = true;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            GamePaused = false;
        }

        public void LoadMenu()
        {
            try
            {
                Resume();
                SceneManager.LoadScene("MainMenu");
            }
            catch
            {
                Debug.LogWarning("Scene \"" + name + "\" could not be found.");
            }
        }

        public void Restart()
        {
            try
            {
                Resume();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            catch
            {
                Debug.LogWarning("Scene \"" + name + "\" could not be found.");
            }
        }
    }
}