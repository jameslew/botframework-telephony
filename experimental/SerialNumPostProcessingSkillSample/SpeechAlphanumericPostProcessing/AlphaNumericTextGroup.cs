﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpeechAlphanumericPostProcessing
{
    public class AlphaNumericTextGroup
    {
        public AlphaNumericTextGroup(bool acceptsAlphabets, bool acceptsDigits, short lengthInChar)
        {
            this.AcceptsAlphabet = acceptsAlphabets;
            this.AcceptsDigits = acceptsDigits;
            this.LengthInChars = lengthInChar;
        }

        public AlphaNumericTextGroup(string regex)
        {
            if ((regex.IndexOf('(') < 0) || (regex.IndexOf('(') < 0) ||
                (regex.IndexOf('[') < 0) || (regex.IndexOf(']') < 0) ||
                (regex.IndexOf('{') < 0) || (regex.IndexOf('}') < 0))
            {
                throw new ArgumentException("Invalid regular expression");
            }

            // get character ranges
            int start = regex.IndexOf('[');
            int end = regex.IndexOf(']');
            if ((start < 0) || (end < 0) || (end < (start + 2)))
            {
                throw new ArgumentException("Invalid regular expression");
            }

            string ranges = regex.Substring(start + 1, end - start - 1);
            if (ranges.IndexOf("0-9", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                this.AcceptsDigits = true;
            }

            if (ranges.IndexOf("A-Z", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                this.AcceptsAlphabet = true;
            }

            // check for exclusion set
            start = regex.IndexOf('^', end);
            int endExclude = regex.IndexOf(']', end + 1);
            if ((start > 0) && (endExclude > 0) && (endExclude >= (start + 2)))
            {
                string invalid = regex.Substring(start + 1, endExclude - start - 1);
                this.InvalidChars = new HashSet<char>(invalid.ToCharArray());
            }

            // get length
            start = regex.IndexOf('{', end);
            end = regex.IndexOf('}', end);
            if ((start < 0) || (end < 0) || (end < (start + 2)))
            {
                throw new ArgumentException("Invalid regular expression");
            }

            string length = regex.Substring(start + 1, end - start - 1);
            this.LengthInChars = short.Parse(length, CultureInfo.InvariantCulture);
        }

        public bool AcceptsAlphabet { get; } = false;

        public bool AcceptsDigits { get; } = false;

#pragma warning disable CA2227 // Collection properties should be read only
        public HashSet<char> InvalidChars { get; } = new HashSet<char>();
#pragma warning restore CA2227 // Collection properties should be read only

        public short LengthInChars { get; } = 0;

        public string GenerateGroupExampleText()
        {
            string seed = string.Empty;
            Random rnd = new Random();
            if (AcceptsAlphabet)
            {
                seed += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if (AcceptsDigits)
            {
                seed += "0123456789";
            }

            return new string(Enumerable.Repeat(seed, LengthInChars)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}
