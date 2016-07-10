using System;
using System.Runtime.Serialization;

namespace smg.ArgumentParsing.Exceptions
{
    /// <summary>
    /// Thrown when a property which represents a boolean argument is not <see cref="bool"/>.
    /// </summary>
    [Serializable]
    public class BooleanArgumentPropertyIsNotBoolException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="BooleanArgumentPropertyIsNotBoolException"/>.
        /// </summary>
        public BooleanArgumentPropertyIsNotBoolException()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="BooleanArgumentPropertyIsNotBoolException"/> with a relevant message.
        /// </summary>
        /// <param name="message">A message which describes the reason for throwing this instance of <see cref="BooleanArgumentPropertyIsNotBoolException"/>.</param>
        public BooleanArgumentPropertyIsNotBoolException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="BooleanArgumentPropertyIsNotBoolException"/> with a relevant message and an inner exception.
        /// </summary>
        /// <param name="message">A message which describes the reason for throwing this instance of <see cref="BooleanArgumentPropertyIsNotBoolException"/>.</param>
        /// <param name="inner">The original exception which caused throwing this instance of <see cref="BooleanArgumentPropertyIsNotBoolException"/>.</param>
        public BooleanArgumentPropertyIsNotBoolException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="BooleanArgumentPropertyIsNotBoolException"/> with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the instance of <see cref="BooleanArgumentPropertyIsNotBoolException"/> being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected BooleanArgumentPropertyIsNotBoolException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}