using System;

namespace smg.ArgumentParsing.Attributes
{
    /// <summary>
    /// Represents an argument which can either exists or not.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IndexedArgAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="IndexedArgAttribute"/>.
        /// </summary>
        /// <param name="index">A zero based index of this <see cref="IndexedArgAttribute"/> among all of the arguments.</param>
        /// <remarks>Indices must be consecutive.</remarks>
        public IndexedArgAttribute(int index)
        {
            Index = index;
        }

        /// <summary>
        /// A zero based index of this <see cref="IndexedArgAttribute"/> among all of the arguments.
        /// </summary>
        public int Index { get; set; }
    }
}