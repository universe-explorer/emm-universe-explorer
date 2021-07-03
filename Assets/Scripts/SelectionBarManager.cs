using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBarManager : MonoBehaviour
{

    [SerializeField] private GameObject selectionBar;
    [SerializeField] private WeaponController weaponController;
    
    // Start is called before the first frame update
    void Start()
    {
        
        KeyCode key = KeyCode.Alpha1;

        Weapon[] weapons = weaponController.Weapons;
        
        
        // min 5 slots, selection bar looks kinda bad with less slots; TODO: Mark other slots as unavailable

        var counter = 0;
        
        // TODO: Optimize loop, so we don't have to increase so many counters etc
        foreach (Transform t in selectionBar.transform)
        {

            // Throw exception if key code is higher than key code of the highest number
            if (key > KeyCode.Alpha9)
                throw new Exception("Current KeyCode is higher than KeyCode of the key 9");
            
            t.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = key.ToString().Remove(0, 5); // Remove alpha part from KeyCode.ToString
            // t.gameObject.GetComponentInChildren<Image>() TODO: Implement ISelectionBarItem interface/extra method that returns an icon of the item
            
            // Temporary text field
            // TODO: Unsafe access
            t.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapons[counter].WeaponType.ToString(); // TODO: Add weapon name, so we don't have to use the type
            
            key++;
            counter++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
