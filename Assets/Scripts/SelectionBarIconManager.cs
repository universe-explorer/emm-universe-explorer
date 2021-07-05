using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBarIconManager : MonoBehaviour
{
    private KeyCode key;
    private bool selected = false;

    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image frameImage;
    [SerializeField] private Image cooldownImage;
    private Weapon _weapon;


    // Start is called before the first frame update
    void Start()
    {
    }

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
        set
        {
            frameImage.gameObject.SetActive(value);
            selected = value;
        }
    }

    public void SetWeapon(Weapon w)
    {
        _weapon = w;
        nameText.text = _weapon.Name.Split(' ').First();
    }
}