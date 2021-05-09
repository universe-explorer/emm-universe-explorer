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
    // In development
    // TODO: Add listeners, properties and/or queue?  
    // TODO: What happens when user disconnects device form port -> handle errors
    /**
     * Parses incoming bytes and updates data accordingly
     */
    public sealed class SerialParser : ISerialParser
    {
        private const int baudRate = 9600;
        private SerialPort sp;

        private const byte startByte = 0xBE; // byte sequence in the future?
        private const byte endByte = 0xED;
        private Thread readerThread; // TODO: Abort thread on close
        private const int maxBytes = 50;
        private const int maxInvalidChecksums = 10;

        private const byte speedByte = 0x00;
        private const byte rotationByte = 0x01;

        /**
         * SerialParser constructor gets only called by the class itself -> singleton
         * Finds and opens correct serial port of our connected device (Arduino in our case)
         * 
         * <exception cref="PortNotFoundException">Thrown when port of the device hasn't been found</exception>
         */
        private SerialParser()
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                Debug.Log(portName);

                try
                {
                    sp = new SerialPort(portName, baudRate);
                    // TODO: Maybe add an isOpen check?
                    sp.Open();

                    // Timeout if we don't receive data within 10 milliseconds. Might be too low, but it never failed so far.
                    sp.ReadTimeout = 10;

                    // Read amount of bytes and check if we find startByte within these bytes. Need to implement more complex check in the future, e.g. verify one packet.
                    for (var i = 0; i < maxBytes; i++)
                    {
                        int tmp = sp.ReadByte();
                        Debug.Log(String.Format("Byte {0}: {1}", i, tmp));

                        // TODO: Just checking for one byte is unreliable, but enough for now -> instead use VerifyData when crc has been implemented
                        if (tmp == startByte)
                        {
                            Debug.Log("Found port: " + portName);
                            readerThread = new Thread(ReaderLoop);
                            readerThread.Start();
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

        /**
         * Returns a SerialParser instance
         * 
         * <exception cref="PortNotFoundException">Thrown when port of the device hasn't been found</exception>
         */
        public static ISerialParser Instance => serialParser.Value;


        /**
         * Reads incoming bytes and calls methods accordingly
         *
         * TODO: Handle read timeout
         */
        private void ReaderLoop()
        {
            int invalidChecksumCounter = 0;

            // TODO: Change condition
            while (true)
            {
                try
                {
                    int[] dataBuffer = VerifyIncomingData();
                    invalidChecksumCounter = 0; // Reset counter to zero
                    ParseData(dataBuffer);
                }
                catch (InvalidChecksumException e)
                {
                    invalidChecksumCounter++;
                    if (invalidChecksumCounter >= maxInvalidChecksums)
                    {
                        // TODO: Notify user on gui
                        Debug.Log(invalidChecksumCounter + " invalid checksums in a row!");
                    }
                }
                catch (InvalidDataException e)
                {
                    Debug.Log(e.Message);
                    // TODO: Maybe count these attempts too
                }
            }
        }

        /**
         * Verifies that incoming bytes represent a valid dataBuffer that can be parsed
         * 
         * <returns>Valid dataBuffer</returns>
         * <exception cref="InvalidDataException">Thrown when incoming data doesn't match our protocol</exception>
         * <exception cref="InvalidChecksumException">Thrown when checksum of dataBuffer is invalid</exception>
         */
        private int[] VerifyIncomingData()
        {
            // Check if first byte matches startByte
            if (sp.ReadByte() != startByte)
                throw new InvalidDataException("Invalid startByte");

            // TODO: Convert to unsigned byte?
            int length = sp.ReadByte(); // TODO: One byte might not be large enough in the future 
            if (length <= 0)
                throw new InvalidCastException("Length can't be less or equal than zero");
                

            // Save actual data in buffer and verify it with crc before using it
            int[] dataBuffer = new int[length];
            for (var i = 0; i < dataBuffer.Length; i++)
                dataBuffer[i] = sp.ReadByte();


            // Check if byte matches endByte
            if (sp.ReadByte() != endByte)
                throw new InvalidDataException("Invalid endByte");

            // TODO: Check hash at the end
            // crc
            if (false) // TODO: change condition with return value of checksum check
                throw new InvalidChecksumException();

            return dataBuffer;
        }

        /**
         * Only parses and sets verified data
         * 
         * <exception cref="InvalidDataException">Thrown when an invalid data type has been read</exception>
         */
        private void ParseData(int[] dataBuffer)
        {
            for (var i = 0; i < dataBuffer.Length; i++)
            {
                // Get data type (speed, rotation, etc.) to conclude which value we have to update and how many bytes we need to read
                switch (dataBuffer[i])
                {
                    case speedByte:
                        // TODO: read one byte and set speed
                        break;
                    case rotationByte:
                        // Read multiple bytes?
                        break;
                    default:
                        throw new InvalidDataException("Invalid data type"); // TODO: Maybe use another exception
                }
            }
        }
    }
}