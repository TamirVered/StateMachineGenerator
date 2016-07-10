using System;
using System.Collections.Generic;
using System.Linq;

namespace smg.ArgumentParsing.Attributes
{
    /// <summary>
    /// Represents an argument which can either exists or not.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BooleanArgAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="BooleanArgAttribute"/>.
        /// </summary>
        /// <param name="name">A non-case-sensitive name which represents the attribute.</param>
        /// <param name="alternativeNames">Non-case-sensitive names that can also represent the attribute but are not mandatory.</param>
        public BooleanArgAttribute(string name, params string[] alternativeNames)
        {
            PossibleArgNames = new[] { name }.Concat(alternativeNames);
        }

        /// <summary>
        /// A collection of all the names possible for this instance of <see cref="BooleanArgAttribute"/>
        /// </summary>
        public IEnumerable<string> PossibleArgNames { get; set; }
    }
}