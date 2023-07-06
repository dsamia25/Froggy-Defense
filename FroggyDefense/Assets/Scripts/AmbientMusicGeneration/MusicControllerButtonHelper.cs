using UnityEditor;
using UnityEngine;

namespace AmbientMusicGenerator
{
    [CustomEditor(typeof(MusicController))]
    public class MusicControllerButtonHelper : Editor
    {
        private string presetName;
        private string savePath;

        private void Awake()
        {
            MusicController controller = (MusicController)target;

            // If there was a previously set name and path then use those.
            if (controller.lastInputPresetName.Equals(""))
            {
                presetName = "New Preset";
            } else
            {
                presetName = controller.lastInputPresetName;
            }

            if (controller.lastInputPresetName.Equals(""))
            {
                savePath = "Assets/ScriptableObjects/AmbientMusicGenerator";
            } else
            {
                savePath = controller.lastInputSavePresetPath;
            }
        }

        public override void OnInspectorGUI()
        {
            MusicController controller = (MusicController)target;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Preset Name");
            presetName = GUILayout.TextField(presetName, GUILayout.Width(300));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Save Path");
            savePath = GUILayout.TextField(savePath, GUILayout.Width(300));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Save Preset"))
            {
                controller.SavePreset(presetName, savePath);
            }

            base.OnInspectorGUI();
        }
    }
}
