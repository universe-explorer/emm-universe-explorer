using System.IO.Ports;

namespace SerialCommunication
{
    public interface IReader
    {
        public int read(byte[] data);
    }
}