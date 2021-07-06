using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base menu class which offers basic methods for menus
/// </summary>
public class BaseMenu : MonoBehaviour
{

    [SerializeField] private GameObject settings;
    private List<GameObject> _activeGameObjects = new List<GameObject>();
    private bool registerInput = true;

    /// <summary>
    /// Enable or disable hotkey to open/close menu
    /// </summary>
    public bool RegisterInput
    {
        get => registerInput;
        set => registerInput = value;
    }

    // TODO: Remove parameter
    /// <summary>
    /// Open settings and save active menu ui objects in a list and deactivate them temporarily
    /// </summary>
    public virtual void OpenSettings()
    {
        foreach (Transform t in gameObject.transform)
        {
            if(t.gameObject.activeSelf)
                _activeGameObjects.Add(t.gameObject);
            
            t.gameObject.SetActive(false);
        }
        
        settings.SetActive(true);
    }
    
    /// <summary>
    /// Restore deactivated menu ui objects
    /// </summary>
    public virtual void RestoreMenu()
    {
        _activeGameObjects.ForEach(g => g.SetActive(true));
    }

    /// <summary>
    /// Exit game
    /// </summary>
    public virtual void Exit()
    {
        Application.Quit(0); // Gets ignored in editor
    }
}
