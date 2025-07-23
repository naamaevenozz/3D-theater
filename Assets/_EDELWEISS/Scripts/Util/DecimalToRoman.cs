using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Edelweiss.Core;
using QFSW.QC.Utilities;
using Sirenix.OdinInspector;

namespace Edelweiss.Utils
{
    public class RomanTester : EdelMono
    {
        public int input;

        [Button]
        private void Convert()
        {
            print($"{input} -> {input.ToRoman()}");
        }
    }

    public readonly struct RomanNumeral

    {
        private static readonly string[] UNITS        = { "", "I", "II", "III", "IV" };
        private static readonly string   FIVE_UNIT    = "V";
        private static readonly string[] TENS         = { "", "X", "XX", "XXX", "XL" };
        private static readonly string   FIVE_TENS    = "L";
        private static readonly string[] HUNDREDS     = { "", "C", "CC", "CCC", "CD" };
        private static readonly string   FIVE_HUNDRED = "D";
        private static readonly string   THOUSAND     = "M";

        public readonly int      Value;
        public readonly string[] Numerals;

        public RomanNumeral(int value)
        {
            Value = value;

            Numerals = ProcessValue(value);
        }

        private static string[] ProcessValue(int value)
        {
            if (value < 1) return new string[] { };

            List<string> result = new();

            int units = value % 10;
            result.Add(UNITS[units % 5]);
            if (units >= 5) result.Add(FIVE_UNIT);

            int tens = (value / 10) % 10;
            if (tens > 0) result.Add(TENS[tens % 5]);
            if (tens >= 5) result.Add(FIVE_TENS);

            int hundreds = (value / 100) % 10;
            if (hundreds > 0) result.Add(HUNDREDS[hundreds % 5]);
            if (hundreds >= 5) result.Add(FIVE_HUNDRED);

            int    thousands   = value / 1000;
            string thousandStr = new string(THOUSAND[0], thousands);
            if (thousands > 0) result.Add(thousandStr);

            return result.ToArray();
        }

        public override string ToString()
        {
            return string.Join("", Numerals.Reversed());
        }
    }

    public static class RomanNumerals
    {
        private static readonly string[] ROMAN_UNITS = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
        private static readonly string[] ROMAN_TENS  = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };

        private static readonly string[] ROMAN_HUNDREDS =
            { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };

        private static readonly char ROMAN_THOUSANDS = 'M';

        public static string ToRoman(this int value)
        {
            string result = "";

            int units     = value         % 10;
            int tens      = (value / 10)  % 10;
            int hundreds  = (value / 100) % 10;
            int thousands = (value / 1000);

            return (thousands > 0 ? new string(ROMAN_THOUSANDS, thousands) : "")
                 + ROMAN_HUNDREDS[hundreds] + ROMAN_TENS[tens] + ROMAN_UNITS[units];
        }
    }
}