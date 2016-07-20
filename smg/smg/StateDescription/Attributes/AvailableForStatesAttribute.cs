using System;
using smg.StateDescription.Enums;

namespace smg.StateDescription.Attributes
{
    /// <summary>
    /// Indicates that a method is available for specific states that the stateful object containing it might be at.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    internal sealed class AvailableForStatesAttribute : Attribute
    {
        public LogicalRelations Relation { get; set; } = LogicalRelations.Or;

        /// <summary>
        /// The states in which the object containing the decorated method should be for the method to be available for it.
        /// </summary>
        public string[] States { get; }

        /// <summary>
        /// Creates a new instance of <see cref="AvailableForStatesAttribute"/> with the states in which the object containing the decorated method should be for the method to be available for it.
        /// </summary>
        /// <param name="states">The states in which the object containing the decorated method should be for the method to be available for it.</param>
        public AvailableForStatesAttribute(params string[] states)
        {
            States = states;
        }
    }
}