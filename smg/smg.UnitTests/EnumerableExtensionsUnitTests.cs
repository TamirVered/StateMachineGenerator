using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using smg.ExtensionMethods;

namespace smg.UnitTests
{
    /// <summary>
    /// Tests the functionality <see cref="EnumerableExtensions"/>.
    /// </summary>
    [TestClass]
    public class EnumerableExtensionsUnitTests
    {
        /// <summary>
        /// Test whether <see cref="EnumerableExtensions.GetPermutations{T}"/> returns an empty collection of collections when invoked with an empty collection of collections.
        /// </summary>
        [TestMethod]
        public void EmptyCollection_ExpectEmptyCollection()
        {
            Assert.IsFalse(Enumerable.Empty<IEnumerable<object>>().GetPermutations().Any(), "Expected empty collection of collection to have no permutations.");
        }

        /// <summary>
        /// Test whether <see cref="EnumerableExtensions.GetPermutations{T}"/> returns a valid collection of collections when invoked with a given collection of collections.
        /// </summary>
        [TestMethod]
        public void Collection_ExpectValidPermutations()
        {
            List<List<int>> collection = new List<List<int>> { new List<int> { 1, 2, 3 }, new List<int> { 4, 5 }, new List<int> { 6, 7 } };
            List<List<int>> expected = new List<List<int>>
            {
                new List<int> { 1, 4, 6 },
                new List<int> { 1, 4, 7 },
                new List<int> { 1, 5, 6 },
                new List<int> { 1, 5, 7 },
                new List<int> { 2, 4, 6 },
                new List<int> { 2, 4, 7 },
                new List<int> { 2, 5, 6 },
                new List<int> { 2, 5, 7 },
                new List<int> { 3, 4, 6 },
                new List<int> { 3, 4, 7 },
                new List<int> { 3, 5, 6 },
                new List<int> { 3, 5, 7 }
            };

            IEnumerable<IEnumerable<int>> permutations = collection.GetPermutations();

            Assert.AreEqual(expected.Count, permutations.Count());

            int index = 0;
            foreach (IEnumerable<int> permutation in permutations)
            {
                Assert.IsTrue(permutation.SequenceEqual(expected[index]), "Expected calculated permutations to be different than actual result.");
                index++;
            }
        }
    }
}
