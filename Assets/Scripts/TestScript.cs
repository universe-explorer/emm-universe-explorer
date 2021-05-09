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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
