using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smg.ExtensionMethods
{
    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Gets all of the permutations available for collection of collections.
        /// </summary>
        /// <typeparam name="T">The type of the collections contained by the given collection.</typeparam>
        /// <param name="enumerable">The collection containing the collections to get the permutations of.</param>
        /// <returns>All of the permuations available for a certain collection of collections.</returns>
        /// <example>
        /// Input:
        /// [{1}, {2}, {3}], [{4}, {5}], [{6}, {7}]
        /// Output:
        /// [{1}, {4}, {6}], [{1}, {4}, {7}], [{1}, {5}, {6}], [{1}, {5}, {7}], 
        /// [{2}, {4}, {6}], [{2}, {4}, {7}], [{2}, {5}, {6}], [{2}, {5}, {7}], 
        /// [{3}, {4}, {6}], [{3}, {4}, {7}], [{3}, {5}, {6}], [{3}, {5}, {7}]
        /// </example>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<IEnumerable<T>> enumerable)
        {
            if (!enumerable.Skip(1).Any())
            {
                return enumerable
                    .SelectMany(x => x
                        .Select(y => Enumerable.Repeat(y, 1)));
            }
            return enumerable.First()
                .SelectMany(x => enumerable.Skip(1).GetPermutations()
                    .Select(y => Enumerable.Repeat(x, 1).Concat(y)));
        }
    }
}
