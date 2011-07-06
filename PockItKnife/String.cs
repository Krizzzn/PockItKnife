using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Contains extension methods for String
    /// </summary>
    public static partial class PockItKnife
    {
        /// <summary>
        /// Provides a convinient way to use the String.Format()
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Inject(this string format, params object[] param)
        {
            if (format == null)
                return null;
            return string.Format(format, param);
        }
    }

}
