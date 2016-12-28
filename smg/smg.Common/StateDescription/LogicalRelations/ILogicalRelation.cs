using System.Collections.Generic;

namespace smg.Common.StateDescription.LogicalRelations
{
    /// <summary>
    /// Represents a logical gate relation for state availability evaluation.
    /// </summary>
    public interface ILogicalRelation
    {
        /// <summary>
        /// Checks whether the provided <paramref name="permutation"/> is valid under the <paramref name="availableForStates"/> set of states in the specific implementation.
        /// </summary>
        /// <param name="stateGroups">The state groups for which the permutation will be validated.</param>
        /// <param name="permutation">A permutation of states of the provided <paramref name="stateGroups"/> to be validated.</param>
        /// <param name="availableForStates">A set of states that determines whether the provided <paramref name="permutation"/> is valid in the specific implementation.</param>
        /// <returns>A value indicating whether the provided <paramref name="permutation"/> is valid under the <paramref name="availableForStates"/> set of states in the specific implementation.</returns>
        bool IsPermutationValid(Dictionary<string, string[]> stateGroups,
            string[] permutation,
            string[] availableForStates);
    }
}
