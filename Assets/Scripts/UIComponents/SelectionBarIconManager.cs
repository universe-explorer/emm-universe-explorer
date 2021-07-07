using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Modifies a selection bar icon
/// </summary>
public class SelectionBarIconManager : MonoBehaviour
{
    private KeyCode key;
    private bool selected = false;

    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image frameImage;
    [SerializeField] private Image cooldownImage;
    private Weapon _weapon;

    

    // Update is called once per frame
    void Update()
    {
        if (_weapon)
        {
            if (_weapon.FireRate > 0)
            {
                float currentTime = Time.time;
                cooldownImage.fillAmount = Mathf.Lerp(1, 0, 1 - (_weapon.NextFire - currentTime) / _weapon.FireRate);
            }
        }
    }

    /// <summary>
    /// Gets key code of the icon
    /// Sets the key code to select the icon and shortens long names
    /// </summary>
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

    /// <summary>
    /// Gets display name of the icon
    /// Sets display name of the icon
    /// </summary>
    public string Name
    {
        get => nameText.text;
        set => nameText.text = value;
    }

    /// <summary>
    /// Gets selection bool
    /// Sets selection bool
    /// </summary>
    public bool Selected
    {
        get => selected;
        set
        {
            frameImage.gameObject.SetActive(value);
            selected = value;
        }
    }

    /// <summary>
    /// Sets weapon of the icon
    /// </summary>
    /// <param name="w">Weapon</param>
    public void SetWeapon(Weapon w)
    {
        _weapon = w;
        nameText.text = _weapon.Name.Split(' ').First();
    }
}