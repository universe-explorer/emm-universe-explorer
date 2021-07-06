using System;

namespace SerialCommunication
{
    // TODO: Rename class since exception name is already taken 
    
    /// <summary>
    /// Invalid data has been received
    /// </summary>
    public class InvalidDataException : Exception
    {
        /// <summary>
        /// Constructor with message
        /// </summary>
        /// <param name="msg">Message</param>
        public InvalidDataException(string msg) : base(msg)
        {
            
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidDataException()
        {
            
        }
    }
}