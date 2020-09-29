namespace Thrift.Net.Compilation.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}" /> objects.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns elements from the specified collection until the specified predicate
        /// returns true. Like TakeWhile(), but also returns the last matching element.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <param name="predicate">The predicate to test each item with.</param>
        /// <typeparam name="T">The type of item the collection contains.</typeparam>
        /// <returns>The collection containing items until the specified condition is true.</returns>
        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (!predicate(item))
                {
                    yield return item;
                }
                else
                {
                    yield return item;
                    yield break;
                }
            }
        }
    }
}