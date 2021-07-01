using System;
using UnityEngine;

namespace SerialCommunication
{
    public class JoystickReader : IReader
    {
        public double roll;
        public double pitch;
        public double yaw;
        public double btn_state;

        private JoystickReader()
        {
        }

        private static readonly Lazy<JoystickReader> joystickReader =
            new Lazy<JoystickReader>(() => new JoystickReader());

        /** todo
         * Returns a SerialParser instance
         * 
         * <exception cref="PortNotFoundException">Thrown when port of the device hasn't been found</exception>
         */
        public static JoystickReader Instance => joystickReader.Value;


        public int read(byte[] data)
        {
            int roll_val = data[0] - 100;
            roll = roll_val * 0.01f;

            int pitch_val = data[1] - 100;
            pitch = pitch_val * 0.01f;

            int yaw_val = data[2] - 100;
            yaw = yaw_val * 0.01f;

            btn_state = data[3];

            Debug.Log("JoystickReader: roll=" + roll + ", pitch =" + pitch + ", yaw =" + yaw + ", btn_state =" +
                      btn_state);

            return 4;
        }
    }
}