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
        foreach (Transform t in selectionBar.transform)
        {
            t.gameObject.GetComponent<TextMeshProUGUI>().text = "test";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
