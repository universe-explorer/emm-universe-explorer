using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBarManager : MonoBehaviour
{

    [SerializeField] private GameObject selectionBar;
    [SerializeField] private WeaponController weaponController;

    private List<SelectionBarIconManager> uiSelectionBarSlots = new List<SelectionBarIconManager>();
    
    // Start is called before the first frame update
    void Start()
    {
        
        KeyCode key = KeyCode.Alpha1;

        IList<Weapon> weapons = weaponController.Weapons.AsReadOnlyList();
        
        
        // min 5 slots, selection bar looks kinda bad with less slots; TODO: Mark other slots as unavailable

        var counter = 0;
        
        // TODO: Optimize loop, so we don't have to increase so many counters etc
        foreach (Transform t in selectionBar.transform)
        {

            // Throw exception if key code is higher than key code of the highest number
            if (key > KeyCode.Alpha9)
                throw new Exception("Current KeyCode is higher than KeyCode of the key 9");

            uiSelectionBarSlots.Add(t.GetComponent<SelectionBarIconManager>());
            
            t.GetComponent<SelectionBarIconManager>().Key = key;
            if (counter < weapons.Count)
            {
                t.GetComponent<SelectionBarIconManager>().Name = weapons[counter].Name.Split(' ').First();
                if (counter == weaponController.ActiveWeaponIndex)
                {
                    t.GetComponent<SelectionBarIconManager>().Selected = true;
                }
            }
            else
            {
                t.GetComponent<SelectionBarIconManager>().Name = "Empty";
            }
            
            
            /*
            t.Find("key").GetComponent<TextMeshProUGUI>().text = key.ToString().Remove(0, 5); // Remove alpha part from KeyCode.ToString
            // t.gameObject.GetComponentInChildren<Image>() TODO: Implement ISelectionBarItem interface/extra method that returns an icon of the item
            
            // Temporary text field
            // TODO: Unsafe access
            t.Find("name").GetComponent<TextMeshProUGUI>().text = weapons[counter].WeaponType.ToString(); // TODO: Add weapon name, so we don't have to use the type
            */
           
            key++;
            counter++;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach(SelectionBarIconManager slot in uiSelectionBarSlots)
        {
            if (Input.GetKeyDown(slot.Key))
            {
                uiSelectionBarSlots.ForEach(e => e.Selected=false);
                slot.Selected = true;
                weaponController.ActiveWeaponIndex = slot.
            }
        }
        */

        for (var i = 0; i < uiSelectionBarSlots.Count; i++)
        {
            if (Input.GetKeyDown(uiSelectionBarSlots[i].Key))
            {
                if (weaponController.SwitchWeapon(i))
                {
                    uiSelectionBarSlots.ForEach(e => e.Selected=false);
                    uiSelectionBarSlots[i].Selected = true; 
                }
                
                
            }
        }
    }
}
