using System;
using smg.Common.StateDescription.LogicalRelations;

namespace smg.Common.StateDescription.Attributes
{
    /// <summary>
    /// Indicates that a method is available for specific states that the stateful object containing it might be at.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class AvailableForStatesAttribute : Attribute
    {
        /// <summary>
        /// Determines how to <see cref="States"/> should be referenced.
        /// </summary>
        /// <example>
        /// Stateful type has state groups:
        /// - Group1: State1, State2, State3.
        /// - Group2: State4, State5.
        /// - Group3: State6, State7.
        /// AvailableForStatesAttribute.States = [Group1State1, Group1State2, Group2State4]
        /// ===============================================================================
        /// AvailableForStatesAttrubute.Relation = Or:
        /// Available for: S1S4S6, S1S4S7, S1S5S6, S1S5S7, S2S4S6, S2S4S7, S2S5S6, S2S5S7, S3S4S6, S3S4S7.
        /// Unavailable for: S3S5S6, S3S5S7.
        /// -------------------------------------------------------------------------------
        /// AvailableForStatesAttrubute.Relation = Xor:
        /// Available for: S1S5S6, S1S5S7, S2S5S6, S2S5S7, S3S4S6, S3S4S7.
        /// Unavailable for: S1S4S6, S1S4S7, S2S4S6, S2S4S7, S3S5S6, S3S5S7.
        /// -------------------------------------------------------------------------------
        /// AvailableForStatesAttrubute.Relation = And:
        /// Invalid because Group1State1 and Group1State2 can never occur together since they are in the same group.
        /// </example>
        /// <remarks>Type must implement <see cref="ILogicalRelation"/>.</remarks>
        public Type Relation { get; set; } = typeof(OrLogicalRelation);

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