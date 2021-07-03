using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SelectionBarManager : MonoBehaviour
{

    [SerializeField] private GameObject selectionBar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        KeyCode key = KeyCode.Alpha1;
        
        
        foreach (Transform t in selectionBar.transform)
        {

            // Throw exception if key code is higher than key code of the highest number
            if (key > KeyCode.Alpha9)
                throw new Exception("Current KeyCode is higher than KeyCode of the key 9");
            
            t.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = key.ToString().Remove(0, 5);
            key++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
