using System;
using smg.StateDescription.LogicalRelations;

namespace smg.StateDescription.Attributes
{
    /// <summary>
    /// Indicates that the decorated method should change return a state wrapper of a different state.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ChangeToStateAttribute : Attribute
    {
        /// <summary>
        /// The state which the stateful <see cref="Type"/> whos method is being invoked should be activated.
        /// </summary>
        public string TargetState { get; private set; }

        /// <summary>
        /// Determines how to <see cref="ConditionStates"/> should be referenced.
        /// </summary>
        /// <example>
        /// Stateful type has state groups:
        /// - Group1: State1, State2, State3.
        /// - Group2: State4, State5.
        /// - Group3: State6, State7.
        /// ChangeToStateAttribute.ConditionStates = [Group1State1, Group1State2, Group2State4]
        /// ===============================================================================
        /// ChangeToStateAttribute.ConditionRelation = Or:
        /// Changes State for: S1S4S6, S1S4S7, S1S5S6, S1S5S7, S2S4S6, S2S4S7, S2S5S6, S2S5S7, S3S4S6, S3S4S7.
        /// Doesn't Change State for: S3S5S6, S3S5S7.
        /// -------------------------------------------------------------------------------
        /// ChangeToStateAttribute.ConditionRelation = Xor:
        /// Changes State for: S1S5S6, S1S5S7, S2S5S6, S2S5S7, S3S4S6, S3S4S7.
        /// Doesn't Change State for: S1S4S6, S1S4S7, S2S4S6, S2S4S7, S3S5S6, S3S5S7.
        /// -------------------------------------------------------------------------------
        /// ChangeToStateAttribute.Relation = And:
        /// Invalid because Group1State1 and Group1State2 can never occur together since they are in the same group.
        /// </example>
        /// <remarks>Type must implement <see cref="ILogicalRelation"/>.</remarks>
        public Type ConditionRelation { get; set; } = typeof(AndLogicalRelation);

        /// <summary>
        /// The states for which the stateful <see cref="Type"/> containing the decorated method should change its state.
        /// </summary>
        public string[] ConditionStates { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ChangeToStateAttribute"/> with the states in which the stateful <see cref="Type"/> containing the decorated method should be for its state to change to the given state.
        /// </summary>
        /// <param name="targetState">The state which the stateful <see cref="Type"/> whos method is being invoked should be activated.</param>
        /// <param name="conditionStates">The states for which the stateful <see cref="Type"/> containing the decorated method should change its state.</param>
        public ChangeToStateAttribute(string targetState, params string[] conditionStates)
        {
            TargetState = targetState;
            ConditionStates = conditionStates;
        }
    }
}