using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenu : MonoBehaviour
{
   
    public virtual void OpenSettings(bool active)
    {
        Debug.Log("base method: settings");
    }

    public virtual void Exit()
    {
        Debug.Log("base method: exit");
    }
}
