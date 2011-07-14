using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Contains Extensions for IEnumerable<T>
    /// </summary>
    public static partial class PockItKnifeExtensions
    {
        /// <summary>
        /// Execute an action on every object that matches the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> coll, Predicate<T> predicate, Action<T> action)
        {
            if (predicate == null)
                throw new ArgumentNullException("Argument predicate may not be null");
            if (action == null)
                throw new ArgumentNullException("Argument action may not be null");
            
            if (coll == null)
                return;

            foreach (T item in coll)
                if (predicate(item))
                    action(item);
        }

        /// <summary>
        /// Execute an action on every object in the IEnumerable<T>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> coll, Action<T> action)
        {
            ForEach(coll, (t) => true, action);
        }

        /// <summary>
        /// Returns Humanize object, that is able to convert listoutput into "a, b, c and d" or "a, b, c or d"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static PockItKnife.Humanizers.HumanizeEnumerables<T> Humanize<T>(this IEnumerable<T> coll)
        {
            return new PockItKnife.Humanizers.HumanizeEnumerables<T>(coll);
        }
    }
}
