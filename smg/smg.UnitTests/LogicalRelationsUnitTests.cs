using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using smg.Common.Exceptions;
using smg.Common.StateDescription.LogicalRelations;

namespace smg.UnitTests
{
    /// <summary>
    /// Tests the functionality <see cref="ILogicalRelation"/>'s implementations.
    /// </summary>
    [TestClass]
    public class LogicalRelationsUnitTests
    {
        #region Private Static Fields

        /// <summary>
        /// An instance used to test <see cref="OrLogicalRelation"/>.
        /// </summary>
        private static readonly OrLogicalRelation OR_LOGICAL_RELATION;

        /// <summary>
        /// An instance used to test <see cref="XorLogicalRelation"/>.
        /// </summary>
        private static readonly XorLogicalRelation XOR_LOGICAL_RELATION;

        /// <summary>
        /// An instance used to test <see cref="AndLogicalRelation"/>.
        /// </summary>
        private static readonly AndLogicalRelation AND_LOGICAL_RELATION;

        /// <summary>
        /// Contains a set of state groups to be used for testing logical relations.
        /// </summary>
        private static readonly Dictionary<string, string[]> STATE_GROUPS;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes <see cref="LogicalRelationsUnitTests"/>.
        /// </summary>
        static LogicalRelationsUnitTests()
        {
            OR_LOGICAL_RELATION = new OrLogicalRelation();
            XOR_LOGICAL_RELATION = new XorLogicalRelation();
            AND_LOGICAL_RELATION = new AndLogicalRelation();
            STATE_GROUPS = new Dictionary<string, string[]>
            {
                { "Group1", new [] { "State1", "State2", "State3" } },
                { "Group2", new [] { "State4", "State5" } },
                { "Group3", new [] { "State6", "State7" } }
            };
        }

        #endregion

        #region OrLogicalState

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="OrLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(OrLogicalRelation))]
        public void Or_ValidPermutationWhenTwoStatesFit_ExpectedTrue()
        {
            string[] permutation = { "State1", "State4", "State6" };
            string[] availableForStates = { "State1", "State2", "State4" };
            AssertPermutationIsAvailable(permutation, availableForStates, OR_LOGICAL_RELATION, true);
        }

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="OrLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(OrLogicalRelation))]
        public void Or_ValidPermutationWhenOneStateFits_ExpectedTrue()
        {
            string[] permutation = { "State1", "State5", "State6" };
            string[] availableForStates = { "State1", "State2", "State4" };
            AssertPermutationIsAvailable(permutation, availableForStates, OR_LOGICAL_RELATION, true);
        }

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="OrLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(OrLogicalRelation))]
        public void Or_ValidPermutationNoStateFits_ExpectedFalse()
        {
            string[] permutation = { "State3", "State5", "State6" };
            string[] availableForStates = { "State1", "State2", "State4" };
            AssertPermutationIsAvailable(permutation, availableForStates, OR_LOGICAL_RELATION, false);
        }

        #endregion

        #region XorLogicalState

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="XorLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(XorLogicalRelation))]
        public void Xor_ValidPermutationWhenTwoStatesFit_ExpectedTrue()
        {
            string[] permutation = { "State1", "State4", "State6" };
            string[] availableForStates = { "State1", "State2", "State4" };
            AssertPermutationIsAvailable(permutation, availableForStates, XOR_LOGICAL_RELATION, false);
        }

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="XorLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(XorLogicalRelation))]
        public void Xor_ValidPermutationWhenOneStateFits_ExpectedTrue()
        {
            string[] permutation = { "State1", "State5", "State6" };
            string[] availableForStates = { "State1", "State2", "State4" };
            AssertPermutationIsAvailable(permutation, availableForStates, XOR_LOGICAL_RELATION, true);
        }

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="XorLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(XorLogicalRelation))]
        public void Xor_ValidPermutationNoStateFits_ExpectedFalse()
        {
            string[] permutation = { "State3", "State5", "State6" };
            string[] availableForStates = { "State1", "State2", "State4" };
            AssertPermutationIsAvailable(permutation, availableForStates, XOR_LOGICAL_RELATION, false);
        }

        #endregion

        #region AndLogicalState

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="AndLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(AndLogicalRelation))]
        public void And_ValidPermutationNoStatesToTest_ExpectedTrue()
        {
            string[] permutation = { "State1", "State4", "State6" };
            string[] availableForStates = Enumerable.Empty<string>().ToArray();
            AssertPermutationIsAvailable(permutation, availableForStates, AND_LOGICAL_RELATION, true);
        }

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="AndLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(AndLogicalRelation))]
        public void And_ValidPermutationWhenOneStateFits_ExpectedTrue()
        {
            string[] permutation = { "State1", "State4", "State6" };
            string[] availableForStates = { "State1" };
            AssertPermutationIsAvailable(permutation, availableForStates, AND_LOGICAL_RELATION, true);
        }

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="AndLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(AndLogicalRelation))]
        public void And_ValidPermutationWhenTwoStatesFit_ExpectedTrue()
        {
            string[] permutation = { "State1", "State4", "State6" };
            string[] availableForStates = { "State1", "State4" };
            AssertPermutationIsAvailable(permutation, availableForStates, AND_LOGICAL_RELATION, true);
        }

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="AndLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(AndLogicalRelation))]
        public void And_ValidPermutationWhenTwoStatesOneMissing_ExpectedFalse()
        {
            string[] permutation = { "State1", "State5", "State6" };
            string[] availableForStates = { "State1", "State4" };
            AssertPermutationIsAvailable(permutation, availableForStates, AND_LOGICAL_RELATION, false);
        }

        /// <summary>
        /// Test whether a given permutation is available for specific states under <see cref="AndLogicalRelation"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(nameof(AndLogicalRelation))]
        [ExpectedException(typeof(InvalidStateRepresentationException), "Using an AndLogicalRelation for two states of the same state group should throw an exception.")]
        public void And_ValidPermutationWhenExpectTwoOfSameGroup_ExpectedException()
        {
            string[] permutation = { "State1", "State5", "State6" };
            string[] availableForStates = { "State1", "State2", "State4" };

            AND_LOGICAL_RELATION.IsPermutationValid(STATE_GROUPS, permutation, availableForStates);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Asserts that a given permutation is available for specific states under a given <see cref="ILogicalRelation"/>.
        /// </summary>
        /// <param name="permutation">A permutation of states for which the condition will be asserted.</param>
        /// <param name="availableForStates">The states for which the given permutation should be available or not.</param>
        /// <param name="relation">The relation under which the given permutation should be available or not for the given state set.</param>
        /// <param name="shouldBeAvailable">A value indicating whether the given permutation should be available for the given states under the given <see cref="ILogicalRelation"/>.</param>
        private static void AssertPermutationIsAvailable(string[] permutation, string[] availableForStates, ILogicalRelation relation, bool shouldBeAvailable)
        {
            bool result = relation.IsPermutationValid(STATE_GROUPS, permutation,
                availableForStates);

            if (shouldBeAvailable)
            {
                Assert.IsTrue(result,
                    $"Permutation {FormatPermutation(permutation)} should be available for states {FormatPermutation(availableForStates)} under {relation.GetType().Name}.");
            }
            else
            {
                Assert.IsFalse(result,
                    $"Permutation {FormatPermutation(permutation)} should not be available for states {FormatPermutation(availableForStates)} under {relation.GetType().Name}.");
            }
        }

        /// <summary>
        /// Formats a given permutation to a displayable <see cref="string"/>.
        /// </summary>
        /// <param name="permutation">A permutation to be formatted to a displayable <see cref="string"/>.</param>
        /// <returns>A displayable <see cref="string"/> which describes a given permutation.</returns>
        private static string FormatPermutation(string[] permutation)
        {
            return $"{{{string.Join(", ", permutation)}}}";
        }

        #endregion
    }
}