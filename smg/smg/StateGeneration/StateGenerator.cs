using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using smg.Common.Exceptions;
using smg.Common.StateDescription.Attributes;
using smg.ExtensionMethods;
using smg.StateGeneration.ExtensionMethods;

namespace smg.StateGeneration
{
    /// <summary>
    /// Generates state wrappers for a stateful type.
    /// </summary>
    class StateGenerator
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="StateGenerator"/> to generate state wrappers for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to generate state wrappers according to.</param>
        private StateGenerator(Type type)
        {
            mStatefulType = type;
            
            Dictionary<string, string[]> groupToStates = type.GetCustomAttributesReflectionOnly<StateGroupAttribute>()
                .ToDictionary(x => x.Name, x => x.States);

            ValidateStateGroups(groupToStates);

            IEnumerable<IEnumerable<string>> permutations = groupToStates
                .Select(x => x.Value)
                .GetPermutations();

            foreach (IEnumerable<string> permutation in permutations)
            {
                mStateWrappers.Add(StateWrapper.CreateWrapper(type, groupToStates, permutation));
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Provides a new instance of <see cref="StateGenerator"/> to generate state wrappers for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to generate state wrappers according to.</param>
        /// <returns>A new instance of <see cref="StateGenerator"/> to generate state wrappers for the given <see cref="Type"/>.</returns>
        public static StateGenerator GetFromType(Type type)
        {
            return new StateGenerator(type);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provides a <see cref="CodeCompileUnit"/> which contains all of the state wrappers represented for the states available for the wrapped stateful <see cref="Type"/>.
        /// </summary>
        /// <returns>A <see cref="CodeCompileUnit"/> which contains all of the state wrappers represented for the states available for the wrapped stateful <see cref="Type"/>.</returns>
        public CodeCompileUnit GetCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace codeNamespace = new CodeNamespace(mStatefulType.Namespace);
            codeNamespace.Types.AddRange(mStateWrappers
                .Select(x => x.GetTypeDeclaration())
                .ToArray());

            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validates a given set of state groups.
        /// </summary>
        /// <param name="groupToStates">The set of state group to be validated.</param>
        private void ValidateStateGroups(Dictionary<string, string[]> groupToStates)
        {
            var statesWithTheSameName = groupToStates.Values
                .SelectMany(x => x)
                .GroupBy(x => x)
                .Where(x => x.Count() > 1);

            if (statesWithTheSameName.Any())
            {
                throw new InvalidStateRepresentationException($"Two state groups cannot exist twice neither in the same group nor in a different one, problematic states: {string.Join(", ", statesWithTheSameName.Select(x => x.Key))}");
            }
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// Contains wrappers for every state permutation on the stateful type.
        /// </summary>
        private readonly List<StateWrapper> mStateWrappers = new List<StateWrapper>();

        /// <summary>
        /// A stateful <see cref="Type"/> being wrapped by the state wrappers to be generated.
        /// </summary>
        private readonly Type mStatefulType;

        #endregion
    }
}
