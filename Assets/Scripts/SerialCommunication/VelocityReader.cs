using UnityEngine;

namespace SerialCommunication
{
    /// <summary>
    /// Reader implementation for velocity data
    /// </summary>
    public class VelocityReader : IReader
    {
        private MainThreadObj mto;

        /// <summary>
        /// Constructor which gets the MainThreadObj script from the SerialCommunicationObject
        /// </summary>
        public VelocityReader()
        {
            mto = GameObject.Find("SerialCommunicationObject").GetComponent<MainThreadObj>();
        }

        /// <summary>
        /// Read data from data buffer
        /// </summary>
        /// <param name="data">data buffer</param>
        /// <returns>Amount of bytes read</returns>
        public int read(byte[] data)
        {
            Debug.Log("Velocity: " + data[0]);

            //Debug.Log(sc.velocity);
            //sc.Move((data[0] / 255) * 30);
            //sc.Move(sc.transform.forward, 5);
            mto.Velocity = data[0];
            Debug.Log("main thread velocity: " + mto.Velocity);
            return 1;
        }
    }
}