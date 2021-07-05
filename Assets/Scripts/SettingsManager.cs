using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private UnityEvent settingsEvent = new UnityEvent();

    [SerializeField] private GameObject options;

    [SerializeField] private Toggle toggleMinimap, togglePostprocessing, toggleMicrocontroller;
    [SerializeField] private Slider sliderMinimapView;
    [SerializeField] private BaseMenu _baseMenu;

    // private bool microcontroller, minimap, postprocessing;

    // Start is called before the first frame update
    void Start()
    {
        settingsEvent.AddListener(() => Debug.Log("Settings have been changed"));
        LoadUI();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        LoadUI();
    }

    public void Save()
    {
        // PlayerPrefs saves data in registry

        /*
        foreach (var t in options.GetComponentsInChildren<Toggle>())
        {
            string name = t.gameObject.name;
            bool active = t.isOn;
            Debug.Log(name + ": " + active);
            PlayerPrefs.SetInt(name, active ? 1 : 0);
        }

        foreach (var s in options.GetComponentsInChildren<Slider>())
        {
            string name = s.gameObject.name;
            var value = s.value;
            Debug.Log(name + ": " + value);
            PlayerPrefs.SetFloat(name, value);
        }
        */

        PlayerPrefs.SetInt(toggleMinimap.name, toggleMinimap.isOn ? 1 : 0);
        PlayerPrefs.SetInt(togglePostprocessing.name, togglePostprocessing.isOn ? 1 : 0);
        PlayerPrefs.SetInt(toggleMicrocontroller.name, toggleMicrocontroller.isOn ? 1 : 0);
        PlayerPrefs.SetFloat(sliderMinimapView.name, sliderMinimapView.value);

        PlayerPrefs.Save();

        settingsEvent.Invoke();
        _baseMenu.RestoreMenu();
        gameObject.SetActive(false);
    }

    public void LoadUI()
    {
        toggleMinimap.isOn = PlayerPrefs.GetInt(toggleMinimap.name, 1) != 0;
        togglePostprocessing.isOn = PlayerPrefs.GetInt(togglePostprocessing.name, 1) != 0;
        toggleMicrocontroller.isOn = PlayerPrefs.GetInt(toggleMicrocontroller.name, 1) != 0;
        sliderMinimapView.value = PlayerPrefs.GetFloat(sliderMinimapView.name, 1);
    }

    public void Discard()
    {
        _baseMenu.RestoreMenu();
        gameObject.SetActive(false);
    }

    /*
    public bool Microcontroller => microcontroller;

    public bool Minimap => minimap;

    public bool Postprocessing => postprocessing;
    */


    /*
     * Add listener settingsEvent
     * Notifies all listeners when settings have been changed
     */
    public void addSettingsEventListener(UnityAction unityAction)
    {
        settingsEvent.AddListener(unityAction);
    }
}