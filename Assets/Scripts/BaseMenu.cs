using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenu : MonoBehaviour
{

    [SerializeField] private GameObject settings;
    public virtual void OpenSettings(bool active)
    {
        Debug.Log("base method: settings");
        foreach (Transform t in gameObject.transform)
        {
            t.gameObject.SetActive(false);
        }
        settings.SetActive(true);
        
    }

    public virtual void Exit()
    {
        Debug.Log("base method: exit");
        Application.Quit(0); // Gets ignored in editor
    }
}
