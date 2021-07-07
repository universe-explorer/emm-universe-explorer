using System;
using System.Collections;
using System.Collections.Generic;
using SerialCommunication;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manage serial parser
/// </summary>
public class SerialCommunicationTestScript : MonoBehaviour
{
    private ISerialParser sp;
    [SerializeField] private SettingsManager _settingsManager;

    void ReloadSettings()
    {
        SerialParser.Instance.ReadData = (PlayerPrefs.GetInt("ToggleMicrocontroller", 1) != 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            ISerialParser sp = SerialParser.Instance;

            sp.addReader(0x00, new VelocityReader());
            sp.addReader(0x01, JoystickReader.Instance);

            _settingsManager.addSettingsEventListener(ReloadSettings);
        }
        catch (PortNotFoundException e)
        {
            Debug.Log("Port not found");
            Destroy(this);
        }
    }


    private void OnDestroy()
    {
        Debug.Log("DESTROYED");
        /*
        if(!sp.IsUnityNull())
            sp.exit();
          
        if(sp != null)
            sp.exit();
        */

        try
        {
            SerialParser.Instance.exit();
        }
        catch (PortNotFoundException e)
        {
        }
    }
}