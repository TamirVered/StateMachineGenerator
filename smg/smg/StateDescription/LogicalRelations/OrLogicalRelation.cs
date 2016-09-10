using System.Collections.Generic;
using System.Linq;

namespace smg.StateDescription.LogicalRelations
{
    /// <summary>
    /// Represents an or logical gate relation for state availability evaluation.
    /// </summary>
    public class OrLogicalRelation : ILogicalRelation
    {
        /// <summary>
        /// Checks whether the provided <paramref name="permutation"/> contains at least one of the states contained by <paramref name="availableForStates"/>.
        /// </summary>
        /// <param name="stateGroups">The state groups for which the permutation will be validated.</param>
        /// <param name="permutation">A permutation of states of the provided <paramref name="stateGroups"/> to be validated.</param>
        /// <param name="availableForStates">A set of states to determine whether the provided <paramref name="permutation"/> contains at least one of them.</param>
        /// <returns>A value indicating whether the provided <paramref name="permutation"/> contains at least one of the states contained by <paramref name="availableForStates"/>.</returns>
        public bool IsPermutationValid(Dictionary<string, string[]> stateGroups,
            string[] permutation,
            string[] availableForStates)
        {
            return availableForStates.Any(permutation.Contains);
        }
    }
}