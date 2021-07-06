namespace SerialCommunication
{
    public interface ISerialParser
    {
        public bool addReader(byte type, IReader reader);
        public void exit();
    }
}