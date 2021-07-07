using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : MonoBehaviour
{

    [SerializeField] private SettingsManager _settingsManager;

    void ReloadSettings()
    {
        Volume volume = GetComponent<Volume>();
        if (PlayerPrefs.GetInt("TogglePostprocessing", 1) != 0)
        {
            volume.weight = 1f;
        }
        else
        {
            volume.weight = 0f;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ReloadSettings();
        _settingsManager.addSettingsEventListener(ReloadSettings);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
