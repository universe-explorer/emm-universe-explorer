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
    private ISerialCommunicationManager sp;
    [SerializeField] private SettingsManager _settingsManager;

    void ReloadSettings()
    {
        SerialCommunicationManager.Instance.ReadData = (PlayerPrefs.GetInt("ToggleMicrocontroller", 1) != 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            ISerialCommunicationManager sp = SerialCommunicationManager.Instance;

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
        try
        {
            SerialCommunicationManager.Instance.exit();
        }
        catch (PortNotFoundException e)
        {
        }
    }
}