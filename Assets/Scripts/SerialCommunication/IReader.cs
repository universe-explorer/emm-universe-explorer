using System.IO.Ports;

namespace SerialCommunication
{
    /// <summary>
    /// Reader inteface for data buffer
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// Read data from data buffer
        /// </summary>
        /// <param name="data">data buffer</param>
        /// <returns>Amount of bytes read</returns>
        public int read(byte[] data);
    }
}