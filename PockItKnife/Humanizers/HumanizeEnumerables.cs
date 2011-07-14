using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PockItKnife.Humanizers
{
    /// <summary>
    /// Converts listoutput into "a, b, c and d" or "a, b, c or d"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HumanizeEnumerables<T>
    {
        internal HumanizeEnumerables(IEnumerable<T> enumerable)
        {
            this.Subject = enumerable;
        }

        /// <summary>
        /// Object to apply humanizing on
        /// </summary>
        public IEnumerable<T> Subject
        {
            get;
            private set;
        }

        /// <summary>
        /// converts listoutput into "a, b, c and d".
        /// </summary>
        /// <returns></returns>
        public string ConcatWithAnd()
        {
            return ConcatWith(", ", " and ");
        }

        /// <summary>
        /// converts listoutput into "a, b, c and d". If list is empty, this will return the parameter coalesceWith
        /// </summary>
        /// <returns></returns>
        public string ConcatWithAnd(string coalesceWith)
        {
            return ConcatWith(", ", " and ", coalesceWith);
        }

        /// <summary>
        /// converts listoutput into "a, b, c or d".
        /// </summary>
        /// <returns></returns>
        public string ConcatWithOr(string coalesceWith)
        {
            return ConcatWith(", ", " or ", coalesceWith);
        }

        /// <summary>
        /// converts listoutput into "a, b, c or d". If list is empty, this will return the parameter coalesceWith
        /// </summary>
        /// <returns></returns>
        public string ConcatWithOr()
        {
            return ConcatWith(", ", " or ");        
        }

        /// <summary>
        /// converts listoutput into "a{comma}b{comma}c{lastConcat}d".
        /// </summary>
        /// <returns></returns>
        public string ConcatWith(string comma, string lastConcat)
        {
            return ConcatWith(comma, lastConcat, "");
        }

        /// <summary>
        /// converts listoutput into "a{comma}b{comma}c{lastConcat}d". If list is empty, this will return the parameter coalesceWith
        /// </summary>
        /// <returns></returns>
        public string ConcatWith(string comma, string lastConcat, string coalesceWith)
        {
            var result = "";

            if (Subject == null)
                return coalesceWith;

            var subject = (from s in Subject
                           where s != null && !s.ToString().Trim().IsNullOrEmpty()
                          select s.ToString().Trim()).ToArray();

            int i = subject.Length - 1;
            while (i >= 0) {
                if (i == 0)
                    lastConcat = "";

                result = lastConcat + subject[i--] + result;
                lastConcat = comma;
            }

            if (result.IsNullOrEmpty())
                return coalesceWith;
            return result;
        }
    }

}
