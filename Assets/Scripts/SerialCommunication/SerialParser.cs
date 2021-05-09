using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using SerialCommunication;
using UnityEditor;
using UnityEngine;

namespace SerialCommunication
{
    // Just a prototype
    // TODO: Add listeners, properties and/or queue?  
    // TODO: What happens when user disconnects device form port -> handle errors
    public sealed class SerialParser : ISerialParser
    {
        private const int baudRate = 9600;
        private SerialPort sp;

        private const byte startByte = 0xBE;
        private const byte endByte = 0xED;
        private Thread parserThread;
        private const int maxBytes = 50;
        
        private SerialParser()
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                Debug.Log(portName);

                try
                {
                    sp = new SerialPort(portName, baudRate);
                    sp.Open();
                    sp.ReadTimeout = 10; // Timeout if we don't receive data within 10 milliseconds. Might be too low, but it never failed so far.
                    
                    // Read amount of bytes and check if we find startByte within these bytes. Need to implement more complex check in the future, e.g. verify one packet.
                    for (var i = 0; i < maxBytes; i++)
                    {
                        int tmp = sp.ReadByte();
                        Debug.Log(String.Format("Byte {0}: {1}", i, tmp));
                        
                        // TODO: Just checking for one byte is unreliable, but enough for now
                        if (tmp == startByte)
                        {
                            Debug.Log("Found port: " + portName);
                            parserThread = new Thread(Parse);
                            parserThread.Start();
                            return;
                        }
                    }
                    sp.Close();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }

            throw new PortNotFoundException();
        }

        private static readonly Lazy<ISerialParser> serialParser =
            new Lazy<ISerialParser>(() => new SerialParser());

        public static ISerialParser Instance => serialParser.Value;
        
        private void Parse()
        {
            
            // TODO: Change condition
            while (true)
            {
                // Check if first byte matches startByte
                if (sp.ReadByte() != startByte)
                    continue;

                int length = sp.ReadByte(); // TODO: One byte might not be large enough in the future 

                
                // TODO: Parse actual data
                for (var i = 0; i < length; i++)
                {
                    int type = sp.ReadByte();
                }
                
                // Check if byte matches endByte
                if(sp.ReadByte() != endByte)
                    continue;
                
                // TODO: Check hash at the end
            }
        }
    }
}