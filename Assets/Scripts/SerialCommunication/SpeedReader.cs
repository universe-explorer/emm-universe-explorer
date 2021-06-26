using UnityEngine;

namespace SerialCommunication
{
    public class SpeedReader : IReader
    {
        public int read(byte[] data)
        {
            Debug.Log("Speed: " + data[0]);
            
            return 1;
        }
    }
}