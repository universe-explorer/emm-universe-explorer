using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenu : MonoBehaviour
{

    [SerializeField] private GameObject settings;
    private List<GameObject> _activeGameObjects = new List<GameObject>();
    
    // TODO: Remove parameter
    public virtual void OpenSettings(bool active)
    {
        Debug.Log("base method: settings");
        
        foreach (Transform t in gameObject.transform)
        {
            if(t.gameObject.activeSelf)
                _activeGameObjects.Add(t.gameObject);
            
            t.gameObject.SetActive(false);
        }
        
        settings.SetActive(true);
    }
    
    public virtual void RestoreMenu()
    {
        _activeGameObjects.ForEach(g => g.SetActive(true));
    }

    public virtual void Exit()
    {
        Debug.Log("base method: exit");
        Application.Quit(0); // Gets ignored in editor
    }
}
