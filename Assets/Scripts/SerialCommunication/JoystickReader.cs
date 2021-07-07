using System;
using UnityEngine;

namespace SerialCommunication
{
    public class JoystickReader : IReader
    {
        public double roll;
        public double pitch;
        public double yaw;
        public bool btn_state;

        private JoystickReader()
        {
        }

        private static readonly Lazy<JoystickReader> joystickReader =
            new Lazy<JoystickReader>(() => new JoystickReader());

        /** todo
         * Returns a SerialCommunicationManager instance
         * 
         * <exception cref="PortNotFoundException">Thrown when port of the device hasn't been found</exception>
         */
        public static JoystickReader Instance => joystickReader.Value;

        /// <summary>
        /// Parses Values from serial port. Sets class variables roll, pitch, yaw & button state.
        /// </summary>
        /// <param name="data">received bytes</param>
        /// <returns></returns>
        public int read(byte[] data)
        {
            int rollVal = data[0] - 100;
            roll = rollVal * 0.01f;

            int pitchVal = data[1] - 100;
            pitch = pitchVal * 0.01f;

            int yawVal = data[2] - 100;
            yaw = yawVal * 0.01f;

            btn_state = data[3] == 1.0;
            

            Debug.Log("JoystickReader: roll=" + roll + ", pitch =" + pitch + ", yaw =" + yaw + ", btn_state =" +
                      btn_state);

            return 4;
        }
    }
}