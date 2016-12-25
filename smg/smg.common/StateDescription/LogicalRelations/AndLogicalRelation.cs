using System.Collections.Generic;
using System.Linq;
using smg.common.Exceptions;

namespace smg.common.StateDescription.LogicalRelations
{
    /// <summary>
    /// Represents an or logical gate relation for state availability evaluation.
    /// </summary>
    public class AndLogicalRelation : ILogicalRelation
    {
        /// <summary>
        /// Checks whether the provided <paramref name="permutation"/> contains all of the states contained by <paramref name="availableForStates"/>.
        /// </summary>
        /// <param name="stateGroups">The state groups for which the permutation will be validated.</param>
        /// <param name="permutation">A permutation of states of the provided <paramref name="stateGroups"/> to be validated.</param>
        /// <param name="availableForStates">A set of states to determine whether the provided <paramref name="permutation"/> contains only one of them.</param>
        /// <returns>A value indicating whether the provided <paramref name="permutation"/> contains all of the states contained by <paramref name="availableForStates"/>.</returns>
        public bool IsPermutationValid(Dictionary<string, string[]> stateGroups,
            string[] permutation,
            string[] availableForStates)
        {
            if (stateGroups.Values.Any(x => x.Count(availableForStates.Contains) > 1))
            {
                throw new InvalidStateRepresentationException($"It is not possible to apply an {nameof(AndLogicalRelation)} between two states of the same state group.");
            }
            return availableForStates.All(permutation.Contains);
        }
    }
}