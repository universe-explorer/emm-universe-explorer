using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private GameObject currentHealthBar;
    [SerializeField] private GameObject delayedHealthBar;

    private Image imgCurrentHealth;
    private Image imgDelayedHealth;
    private float maxHealth = 100;
    private float currentHealth = 100;
    private float progress = 0;
    [SerializeField] private float delayedHealthBarSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        imgCurrentHealth = currentHealthBar.GetComponent<Image>();
        imgDelayedHealth = delayedHealthBar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        imgCurrentHealth.fillAmount = currentHealth / maxHealth;

        if (imgDelayedHealth.fillAmount > imgCurrentHealth.fillAmount)
        {
            progress += Time.deltaTime;
            imgDelayedHealth.fillAmount = Mathf.Lerp(imgDelayedHealth.fillAmount, imgCurrentHealth.fillAmount, progress*delayedHealthBarSpeed);
        }

        Debug.Log(imgCurrentHealth.fillAmount);
        
        /*
        // TODO: Remove before branch gets merged
        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(5);
        */
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth - damage >= 0)
        {
            currentHealth -= damage;
            progress = 0;
        }
    }
}