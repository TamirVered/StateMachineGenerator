using System;
using System.Runtime.Serialization;

namespace smg.Common.Exceptions
{
    /// <summary>
    /// Thrown when a stateful type was represented by an invalid set of attributes.
    /// </summary>
    [Serializable]
    public class InvalidStateRepresentationException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvalidStateRepresentationException"/>.
        /// </summary>
        public InvalidStateRepresentationException()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="InvalidStateRepresentationException"/> with a relevant message.
        /// </summary>
        /// <param name="message">A message which describes the reason for throwing this instance of <see cref="InvalidStateRepresentationException"/>.</param>
        public InvalidStateRepresentationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="InvalidStateRepresentationException"/> with a relevant message and an inner exception.
        /// </summary>
        /// <param name="message">A message which describes the reason for throwing this instance of <see cref="InvalidStateRepresentationException"/>.</param>
        /// <param name="inner">The original exception which caused throwing this instance of <see cref="InvalidStateRepresentationException"/>.</param>
        public InvalidStateRepresentationException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="InvalidStateRepresentationException"/> with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the instance of <see cref="InvalidStateRepresentationException"/> being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected InvalidStateRepresentationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}