using System;
using System.Collections;
using System.Collections.Generic;
using SerialCommunication;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello world :)");
        ISerialParser sp = SerialParser.Instance;
        sp.addReader(0x00, new SpeedReader());

        sp.addReader(0x01, JoystickReader.Instance);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        SerialParser.Instance.exit();
    }
}