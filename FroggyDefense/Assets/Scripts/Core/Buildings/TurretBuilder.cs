using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Buildings {
    public class TurretBuilder : MonoBehaviour
    {   
        [SerializeField] private BoardManager _boardManager;                        // The board manager holding board info.
        [SerializeField] private bool _builderViewActive = false;                   // If the builder view is currently active.
        [SerializeField] private GameObject _selectedTurretPrefab;                  // Which turret the player selected in the build menu.

        public bool BuilderViewActive { get => _builderViewActive; }                // If the builder view is currently active.
        public GameObject SelectedTurretPrefab { get => _selectedTurretPrefab; }    // Which turret the player selected in the build menu.

        private void Update()
        {
            if (_builderViewActive)
            {
                DrawTurretPreview();
            }
        }

        /// <summary>
        /// Builds the given turret at the given location.
        /// </summary>
        /// <param name="prefab"></param>
        public void BuildTurret(GameObject prefab, Vector2 pos)
        {

        }

        /// <summary>
        /// Draws an image of the turret preview over the mouse position.
        /// </summary>
        public void DrawTurretPreview()
        {

        }

        /// <summary>
        /// Toggles the builder view on or off.
        /// </summary>
        public void ToggleBuilderView()
        {

        }

        /// <summary>
        /// Turns on builder view.
        /// </summary>
        public void EnableBuilderView()
        {

        }

        /// <summary>
        /// Turns off builder view.
        /// </summary>
        public void DisableBuilderView()
        {

        }
    }
}