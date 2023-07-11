using UnityEditor;
using UnityEngine;

namespace AmbientMusicGenerator.EditorTools
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
                SavePreset(presetName, savePath, controller);
            }

            base.OnInspectorGUI();
        }

        /// <summary>
        /// Saves the current Sound values as a new preset.
        /// </summary>
        public void SavePreset(string presetName, string savePath, MusicController controller)
        {
            // Save last used values to display in inspector.
            controller.lastInputPresetName = presetName;
            controller.lastInputSavePresetPath = savePath;

            Debug.Log($"Saving a new preset.");
            MusicPresetObject preset = ScriptableObject.CreateInstance<MusicPresetObject>();

            // Make the combined asset path.
            string path = savePath + "/" + presetName;

            // Check if there is already as asset with that name. If so iterate up numbers until there is a unique name.
            if (AssetDatabase.LoadAssetAtPath(path + ".asset", typeof(MusicPresetObject)) != null)
            {
                int i = 0;

                // Iterate up the count until there is a unique number.
                while (AssetDatabase.LoadAssetAtPath(path + (++i).ToString() + ".asset", typeof(MusicPresetObject)) != null) ;

                // Add the unique number to the name.
                presetName += i.ToString();
                path += i.ToString();
            }

            try
            {
                // Create the new ScriptableObject in the database.
                AssetDatabase.CreateAsset(preset, path + ".asset");
            }
            catch (UnityException e)
            {
                Debug.LogWarning($"Error saving preset. Make sure the path exists: {e}");
                return;
            }

            preset.Name = presetName;
            preset.Mixer = controller.Mixer;

            // Save the information about the asset.
            foreach (Sound sound in controller.Sounds)
            {
                Debug.Log($"Sound {sound.Name}.");
                preset.Sounds.Add(new Sound(sound));
            }

            // Save the modified asset.
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
