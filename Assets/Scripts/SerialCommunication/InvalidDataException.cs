using System;

namespace SerialCommunication
{
    // TODO: Rename class since exception name is already taken 
    public class InvalidDataException : Exception
    {
        public InvalidDataException(string msg) : base(msg)
        {
            
        }

        public InvalidDataException()
        {
            
        }
    }
}