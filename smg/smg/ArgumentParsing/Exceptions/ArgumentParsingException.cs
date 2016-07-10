using System;
using System.Runtime.Serialization;

namespace smg.ArgumentParsing.Exceptions
{
    /// <summary>
    /// Thrown when an error has occurred while trying to parse an argument.
    /// </summary>
    [Serializable]
    public class ArgumentParsingException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="ArgumentParsingException"/>.
        /// </summary>
        public ArgumentParsingException()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ArgumentParsingException"/> with a relevant message.
        /// </summary>
        /// <param name="message">A message which describes the reason for throwing this instance of <see cref="ArgumentParsingException"/>.</param>
        public ArgumentParsingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ArgumentParsingException"/> with a relevant message and an inner exception.
        /// </summary>
        /// <param name="message">A message which describes the reason for throwing this instance of <see cref="ArgumentParsingException"/>.</param>
        /// <param name="inner">The original exception which caused throwing this instance of <see cref="ArgumentParsingException"/>.</param>
        public ArgumentParsingException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ArgumentParsingException"/> with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the instance of <see cref="ArgumentParsingException"/> being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ArgumentParsingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}