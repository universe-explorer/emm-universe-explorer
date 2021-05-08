using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using SerialCommunication;
using UnityEngine;

namespace SerialCommunication
{
    public sealed class ArduinoUno : IArduinoUno
    {
        private const int baudRate = 9600;
        private SerialPort sp;

        private ArduinoUno()
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                Debug.Log(portName);
                // TODO: Detect correct port
            }
        }

        private static readonly Lazy<IArduinoUno> arduino = new Lazy<IArduinoUno>(() => new ArduinoUno());

        public static IArduinoUno Instance => arduino.Value;

        public int ReadByte()
        {
            return sp.ReadByte();
        }

        public string ReadLine()
        {
            return sp.ReadLine();
        }
    }
}