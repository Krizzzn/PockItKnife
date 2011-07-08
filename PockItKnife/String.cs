﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Contains extension methods for String
    /// </summary>
    public static partial class PockItKnifeExtensions
    {
        /// <summary>
        /// Provides a convenient way to use the String.Format()
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

        /// <summary>
        /// Provides a weak but convenient way to en or de crpyt a string. Used for avoiding plain text passwords in config files.
        /// </summary>
        /// <param name="forCrypt"></param>
        /// <returns></returns>
        public static PockItKnife.Crypt Crypt(this string forCrypt)
        {
            return new PockItKnife.Crypt(forCrypt);
        }

        /// <summary>
        /// Provides a convenient way to use the String.IsNullOrEmpty()
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Provides a simple way to parse commandline-style arguments.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static PockItKnife.CommandlineArguments ParseCommandlineArguments(this string[] input)
        {
            return PockItKnife.CommandlineArguments.ParseCommandLineArguments(input);
        }
    }

}
