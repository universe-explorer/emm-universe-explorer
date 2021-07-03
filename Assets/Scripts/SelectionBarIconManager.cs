using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectionBarIconManager : MonoBehaviour
{

    private KeyCode key;

    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI nameText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public KeyCode Key
    {
        get => key;
        set
        {
            key = value;
            string keyString = value.ToString();
            if (keyString.StartsWith("Alpha"))
                keyText.text = keyString.Remove(0, 5);
            
        }
    }
}
