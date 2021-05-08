using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using SerialCommunication;
using UnityEngine;

namespace SerialCommunication
{
    // Not tested, just a prototype
    // TODO: Add change listeners and queue 
    public sealed class SerialParser : ISerialParser
    {
        private IArduinoUno arduino;
        private Queue<string> messageQueue = new Queue<string>();

        private const int length = 20;
        private const byte startByte = 0xBE;
        private const byte endByte = 0xED;
        private Thread parserThread;


        private SerialParser(IArduinoUno arduino)
        {
            this.arduino = arduino;
            parserThread = new Thread(Parse);
            parserThread.Start();
        }

        private static readonly Lazy<ISerialParser> serialParser =
            new Lazy<ISerialParser>(() => new SerialParser(ArduinoUno.Instance));

        public static ISerialParser Instance => serialParser.Value;

        private void Parse()
        {
            // TODO: Change condition
            while (true)
            {
                // Check if first byte matches
                if (arduino.ReadByte() != startByte)
                    continue;

                int tmp;

                do
                {
                    tmp = arduino.ReadByte();
                } while (tmp != endByte);

                // TODO: Check hash at the end
            }
        }
    }
}