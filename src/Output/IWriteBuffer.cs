namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Provides an interface for writing content.
    /// </summary>
    public interface IWriteBuffer
    {
        /// <summary>
        /// Gets the number of characters to indent anytime a newline character is
        /// encountered.
        /// </summary>
        int Margin { get; set; }
        
        /// <summary>
        /// Gets the number of characters that have been written since the last
        /// new line character.
        /// </summary>
        int LinePosition { get; }

        /// <summary>
        /// Writes a character to the buffer.
        /// </summary>
        /// <param name="c">Character to write.</param>
        /// <param name="count">The number of times to repeat writing <paramref name="c"/></param>
        void Write(char c, int count = 1);

        /// <summary>
        /// Writes a string to the buffer.
        /// </summary>
        /// <param name="str">String to write</param>
        void Write(string str);

        /// <summary>
        /// Writes a string or string portion.
        /// </summary>
        /// <param name="str">String value</param>
        /// <param name="startIndex">Starting index</param>
        /// <param name="length">Number of characters to write</param>
        void Write(string str, int startIndex, int length);
        
        /// <summary>
        /// Gets the length of the buffer.
        /// </summary>
        int Length { get; }
        
        /// <summary>
        /// Flushes the content of the buffer to an underlying output. 
        /// </summary>
        void Flush();
    }
}