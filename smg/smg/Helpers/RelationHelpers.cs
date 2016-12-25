using System;
using System.Collections.Generic;
using smg.common.Exceptions;
using smg.common.StateDescription.LogicalRelations;

namespace smg.Helpers
{
    /// <summary>
    /// Utility class containing helper methods for handling relation-related condition validations.
    /// </summary>
    static class RelationHelpers
    {
        /// <summary>
        /// Returns a value indicating whether a given permutation is valid under a given relation for a given collection of states.
        /// </summary>
        /// <param name="permutation">A permutation to be validated.</param>
        /// <param name="logicalRelationType">The type of the <see cref="ILogicalRelation"/> that should be used to validate the given permutation.</param>
        /// <param name="conditionStates">A collection of states for which the permutation is valid under the given relation.</param>
        /// <param name="stateGroups">The state groups for which the permutation will be validated.</param>
        /// <returns>A value indicating whether a given permutation is valid under a given relation for a given collection of states.</returns>
        /// <exception cref="InvalidStateRepresentationException"><paramref name="logicalRelationType"/> must be a non-abstract class with default constructor and implement <see cref="ILogicalRelation"/>.</exception>
        public static bool IsPermutationValid(string[] permutation, Type logicalRelationType, string[] conditionStates, Dictionary<string, string[]> stateGroups)
        {
            ILogicalRelation relation = null;
            try
            {
                relation = Activator.CreateInstance(logicalRelationType) as ILogicalRelation;
            }
            catch
            {
                // ignored
            }
            if (relation == null)
            {
                throw new InvalidStateRepresentationException(
                    $"{nameof(logicalRelationType)} must be a non-abstract class with default constructor and implement {nameof(ILogicalRelation)}.");
            }
            return relation.IsPermutationValid(stateGroups, permutation, conditionStates);
        }
    }
}
