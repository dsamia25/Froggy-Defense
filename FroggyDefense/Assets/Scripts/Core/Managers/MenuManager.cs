using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool UseDefaultMenus = false;
    public GameObject[] DefaultMenus;

    private GameObject _openMenu = null;
    private bool _defaultMenusOpen = false;

    private void Start()
    {
        _openMenu = null;
        if (UseDefaultMenus)
        {
            SetDefaultMenus(true);
        }
    }

    /// <summary>
    /// Opens a new menu. If another menu is open, it will be closed first.
    /// </summary>
    /// <param name="menuObject"></param>
    public void OpenMenu(GameObject menuObject)
    {
        try
        {
            CloseMenu();
            menuObject.SetActive(true);
            _openMenu = menuObject;
        } catch (Exception e)
        {
            Debug.LogWarning($"Error opening menu: {e}");
        }
    }

    /// <summary>
    /// Closes the current active menu.
    /// </summary>
    public void CloseMenu()
    {
        try
        {
            if (UseDefaultMenus && _defaultMenusOpen)
                SetDefaultMenus(false);

            if (_openMenu == null)
                return;

            _openMenu.SetActive(false);
            _openMenu = null;

            if (UseDefaultMenus)
                SetDefaultMenus(true);

        } catch (Exception e)
        {
            Debug.LogWarning($"Error closing menu: {e}");
        }
    }

    /// <summary>
    /// Toggles the default menu on or off.
    /// </summary>
    /// <param name="toggle"></param>
    private void SetDefaultMenus(bool toggle)
    {
        try
        {
            if (DefaultMenus.Length > 0)
            {
                foreach (GameObject obj in DefaultMenus)
                {
                    obj.SetActive(toggle);
                }
                _defaultMenusOpen = toggle;
            }
        } catch (Exception e)
        {
            Debug.LogWarning($"Error loading default menu: {e}");
        }
    }
}
