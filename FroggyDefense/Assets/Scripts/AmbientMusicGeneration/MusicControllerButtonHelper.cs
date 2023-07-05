using UnityEditor;
using UnityEngine;

namespace AmbientMusicGenerator.Support
{
    [CustomEditor(typeof(MusicController))]
    public class MusicControllerButtonHelper : Editor
    {
        public override void OnInspectorGUI()
        {
            MusicController controller = (MusicController)target;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Preset Name");
            string presetName = "New Preset";
            presetName = GUILayout.TextField(presetName, GUILayout.Width(300));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Save Path");
            string savePath = "Assets/ScriptableObjects/AmbientMusicGenerator";
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
