using UnityEngine;

namespace SerialCommunication
{
    public class SpeedReader : IReader
    {

        private MainThreadObj mto;

        public SpeedReader()
        {
            mto = GameObject.Find("SerialCommunicationObject").GetComponent<MainThreadObj>();
        }
        public int read(byte[] data)
        {
            Debug.Log("Speed: " + data[0]);
            
            //Debug.Log(sc.velocity);
            //sc.Move((data[0] / 255) * 30);
            //sc.Move(sc.transform.forward, 5);
            mto.Velocity = data[0];
            Debug.Log("main thread velocity: " + mto.Velocity);
            return 1;
        }
    }
}