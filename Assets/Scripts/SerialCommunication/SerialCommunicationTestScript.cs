using System;
using System.Collections;
using System.Collections.Generic;
using SerialCommunication;
using Unity.VisualScripting;
using UnityEngine;

public class SerialCommunicationTestScript : MonoBehaviour
{
    private ISerialParser sp;
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            ISerialParser sp = SerialParser.Instance;
            sp.addReader(0x00, new SpeedReader());
        }
        catch (PortNotFoundException e)
        {
            Debug.Log("Port not found");
            Destroy(this);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
