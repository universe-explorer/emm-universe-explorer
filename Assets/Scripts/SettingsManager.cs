using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{


    [SerializeField] private GameObject options;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void Save()
    {
        
        // PlayerPrefs saves data in registry
        
        foreach (var t in options.GetComponentsInChildren<Toggle>())
        {
            string name = t.gameObject.name;
            bool active = t.isOn;
            Debug.Log(name + ": " + active);
            PlayerPrefs.SetInt(name, active ? 1 : 0);
        }

        foreach (var s in options.GetComponentsInChildren<Slider>())
        {
            string name = s.gameObject.name;
            var value = s.value;
            Debug.Log(name + ": " + value);
            PlayerPrefs.SetFloat(name, value);
        }
        
        PlayerPrefs.Save();
    }

    public void Discard()
    {
        
    }
}
