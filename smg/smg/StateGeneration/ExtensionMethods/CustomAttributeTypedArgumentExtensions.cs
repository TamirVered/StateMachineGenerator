using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace smg.StateGeneration.ExtensionMethods
{
    /// <summary>
    /// Provides extension methods to <see cref="CustomAttributeTypedArgument"/>.
    /// </summary>
    static class CustomAttributeTypedArgumentExtensions
    {
        /// <summary>
        /// Recreates the value of an argument from its representing <see cref="CustomAttributeTypedArgument"/>.
        /// </summary>
        /// <param name="argument">The argument whose value is to be recreated.</param>
        /// <returns>A value of an argument recreated from its representing <see cref="CustomAttributeTypedArgument"/>.</returns>
        /// <example>
        /// If an argument is a single value - the value will be returned as it is.
        /// If an argument is an array - it will be represented as a <see cref="ReadOnlyCollection{T}"/>, therefore, it should be converted back to an array.
        /// </example>
        public static object GetActualArgument(this CustomAttributeTypedArgument argument)
        {
            ReadOnlyCollection<CustomAttributeTypedArgument> arrayArgument =
                argument.Value as ReadOnlyCollection<CustomAttributeTypedArgument>;

            if (arrayArgument == null)
            {
                return argument.Value;
            }

            Array toReturn = Array.CreateInstance(argument.ArgumentType.GetElementType(), arrayArgument.Count);

            for (int i = 0; i < toReturn.Length; i++)
            {
                toReturn.SetValue(arrayArgument[i].Value, i);
            }

            return toReturn;
        }
    }
}