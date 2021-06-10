using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;

    private Image imgCurrentHealth;
    private float maxHealth = 100;
    private float currentHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        imgCurrentHealth = healthBar.GetComponent<Image>();
       
    }

    // Update is called once per frame
    void Update()
    {
        //imgCurrentHealth.fillAmount = imgCurrentHealth.fillAmount - 0.1f*Time.deltaTime;
        imgCurrentHealth.fillAmount = currentHealth / maxHealth;
        Debug.Log(imgCurrentHealth.fillAmount);
        
        if(Input.GetKeyDown(KeyCode.Space))
            TakeDamage(5);
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth - damage >= 0)
        {
            currentHealth -= damage;
        }
    }
}