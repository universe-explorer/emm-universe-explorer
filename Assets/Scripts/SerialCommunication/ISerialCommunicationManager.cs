namespace SerialCommunication
{
    /// <summary>
    /// Serial parser interface
    /// </summary>
    public interface ISerialCommunicationManager
    {
        /// <summary>
        /// Add reader to serial parser
        /// </summary>
        /// <param name="type">Byte header for the following data</param>
        /// <param name="reader">Reader implementation</param>
        /// <returns>False when byte header already exists</returns>
        public bool addReader(byte type, IReader reader);
        
        /// <summary>
        /// Exit reader loop thread
        /// </summary>
        public void exit();

        /// <summary>
        /// Get/Set readData
        /// </summary>
        bool ReadData { get; set; }
    }
}