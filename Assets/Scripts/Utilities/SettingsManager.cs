using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Manages settings and notifies other classes
/// </summary>
public class SettingsManager : MonoBehaviour
{
    private UnityEvent settingsEvent = new UnityEvent();

    [SerializeField] private GameObject options;

    [SerializeField] private Toggle toggleAudio, togglePostprocessing, toggleMicrocontroller;
    [SerializeField] private Slider sliderMinimapView;
    [SerializeField] private BaseMenu _baseMenu;

    // private bool microcontroller, minimap, postprocessing;

    // Start is called before the first frame update
    void Start()
    {
        settingsEvent.AddListener(() => Debug.Log("Settings have been changed"));
        LoadUI();
    }



    private void OnEnable()
    {
        _baseMenu.RegisterInput = false;
        LoadUI();
    }

    private void OnDisable()
    {
        _baseMenu.RegisterInput = true;
    }

    /// <summary>
    /// Save settings and return
    /// </summary>
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

        PlayerPrefs.SetInt(toggleAudio.name, toggleAudio.isOn ? 1 : 0);
        PlayerPrefs.SetInt(togglePostprocessing.name, togglePostprocessing.isOn ? 1 : 0);
        PlayerPrefs.SetInt(toggleMicrocontroller.name, toggleMicrocontroller.isOn ? 1 : 0);
        PlayerPrefs.SetFloat(sliderMinimapView.name, sliderMinimapView.value);

        PlayerPrefs.Save();

        settingsEvent.Invoke();
        _baseMenu.RestoreMenu();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Update settings ui
    /// </summary>
    public void LoadUI()
    {
        toggleAudio.isOn = PlayerPrefs.GetInt(toggleAudio.name, 1) != 0;
        togglePostprocessing.isOn = PlayerPrefs.GetInt(togglePostprocessing.name, 1) != 0;
        toggleMicrocontroller.isOn = PlayerPrefs.GetInt(toggleMicrocontroller.name, 1) != 0;
        sliderMinimapView.value = PlayerPrefs.GetFloat(sliderMinimapView.name, 1);
    }

    /// <summary>
    /// Discard changes and return
    /// </summary>
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


    /// <summary>
    /// Add listener settingsEvent.
    /// Notifies all listeners when settings have been changed.
    /// </summary>
    /// <param name="unityAction">Unity action</param>
    public void addSettingsEventListener(UnityAction unityAction)
    {
        settingsEvent.AddListener(unityAction);
    }
}