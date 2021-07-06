using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
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
        private Thread readerThread; // TODO: Abort thread on close
        private const int maxBytes = 200;
        private const int maxInvalidChecksums = 10;
        private bool continueReaderLoop = true;

        private Dictionary<byte, IReader> dataReaderDictionary = new Dictionary<byte, IReader>();

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
                    sp.DtrEnable = false;
                    // TODO: Maybe add an isOpen check?
                    sp.Open();

                    // Timeout if we don't receive data within 10 milliseconds. Might be too low, but it never failed so far.
                    sp.ReadTimeout = 10; // TODO: Change value for gyroscope 

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
            
            while (continueReaderLoop)
            {
                try
                {
                    byte[] dataBuffer = VerifyIncomingData();
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
            
            Debug.Log("Exiting ReaderLoop...");
        }

        /**
         * Verifies that incoming bytes represent a valid dataBuffer that can be parsed
         * 
         * <returns>Valid dataBuffer</returns>
         * <exception cref="InvalidDataException">Thrown when incoming data doesn't match our protocol</exception>
         * <exception cref="InvalidChecksumException">Thrown when checksum of dataBuffer is invalid</exception>
         */
        private byte[] VerifyIncomingData()
        {
            // Check if first byte matches startByte
            if (sp.ReadByte() != startByte)
                throw new InvalidDataException("Invalid startByte");
            
            // Length of data
            int length = sp.ReadByte(); // TODO: One byte might not be large enough in the future
            Debug.Log("Length: " + length);
            if (length <= 0)
                throw new InvalidCastException("Length can't be less or equal than zero");
            
            // Save actual data in buffer and verify it with crc before using it
            byte[] dataBuffer = new byte[length+1];  // Increase length by one to append the crc8 byte at the end
            for (var i = 0; i < dataBuffer.Length-1; i++)
            {
                // TODO: Casting to byte without checking if ReadByte returns -1 might not be safe
                dataBuffer[i] = (byte) sp.ReadByte(); 
                Debug.Log("byte " + i + ": " + dataBuffer[i]);
            }

            // crc8
            dataBuffer[dataBuffer.Length - 1] = (byte) sp.ReadByte(); // Append crc8 byte
            
            byte crc = crc8(dataBuffer);
            Debug.Log("crc8: " + crc);
            if (crc != 0) // crc8 byte must be zero else throw an exception
                throw new InvalidChecksumException();
            
            byte[] returnBuffer = new byte[length];
            Array.Copy(dataBuffer, returnBuffer, returnBuffer.Length); // Might not be the most efficient way
            return returnBuffer;
        }

        /**
         * Only parses and sets verified data
         * 
         * <exception cref="InvalidDataException">Thrown when an invalid data type has been read</exception>
         */
        private void ParseData(byte[] dataBuffer)
        {

            int counter = 0;

            while (counter < dataBuffer.Length)
            {
                IReader reader;
                if ((reader = dataReaderDictionary[dataBuffer[counter]]) != null)
                {
                    counter++;
                    counter += reader.read(dataBuffer.Skip(counter).ToArray());
                }
                else
                {
                    throw new InvalidDataException("Invalid data type");
                }
                
            }
        }
        
        /**
         * Calculates crc8
         */
        private byte crc8(byte[] bytes)
        {
            byte crc = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                crc ^= bytes[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x80) != 0)
                        crc = (byte) ((crc << 1) ^ 0x15);
                    else
                        crc <<= 1;
                }
            }
            return crc;
        }

        /**
         * Adds a reader
         *
         * TODO: Check for race condition
         * <exception cref="ArgumentException">type already has a reader</exception>
         */
        public bool addReader(byte type, IReader reader)
        {
            try
            {
                dataReaderDictionary.Add(type, reader);
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
            
        }

        public void exit()
        {
            continueReaderLoop = false;
        }
    }
}