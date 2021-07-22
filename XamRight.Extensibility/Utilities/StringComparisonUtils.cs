// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;

namespace XamRight.Extensibility.Utilities
{
    public static class StringComparisonUtils
    {
        /// <summary>
        /// Determines whether the string starts with the specified character after ignoring any initial whitespace.
        /// </summary>
        /// <param name="text">String to check</param>
        /// <param name="startsWith">Character to check for. Must be exact ordinal match.</param>
        /// <returns></returns>
        public static bool StartsWithIgnoreWhitespace(this string text, char startsWith)
        {
            var length = text.Length;
            for (int i = 0; i < length; i++)
            {
                var currChar = text[i];
                if (Char.IsWhiteSpace(currChar))
                    continue;

                if (currChar == startsWith)
                    return true;
                break;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the string ends with the specified character, ignoring any whitespace at the end of the string.
        /// </summary>
        /// <param name="text">String to check</param>
        /// <param name="endsWith">Character to check for. Must be exact ordinal match.</param>
        /// <returns></returns>
        public static bool EndsWithIgnoreWhitespace(this string text, char endsWith)
        {
            for (int i = text.Length - 1; i >= 0; i--)
            {
                var currChar = text[i];
                if (Char.IsWhiteSpace(currChar))
                    continue;

                if (currChar == endsWith)
                    return true;
                break;
            }
            return false;
        }
    }
}
