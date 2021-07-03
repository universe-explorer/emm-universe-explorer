using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectionBarIconManager : MonoBehaviour
{

    private KeyCode key;
    private bool selected = false;

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
                keyString = keyString.Substring(5);

            keyText.text = keyString;
        }
    }

    public string Name
    {
        get => nameText.text;
        set => nameText.text = value;
    }

    public bool Selected
    {
        get => selected;
        set => selected = value;
    }
}
