using System;
using System.Collections.Generic;

namespace PhysicsUtils
{
    public static class SiPrefixConverter
    {
        private static readonly Dictionary<string, int> _prefixExponents =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "deca", 1 },
                { "hecto", 2},
                { "kilo", 3},
                { "mega", 6},
                { "giga", 9},
                { "tera", 12},
                { "peta", 15},
                { "exa", 18},
                { "zetta", 21},
                { "yotta", 24},
                { "romma", 27},
                { "quecca", 30 },

                { "deci", -1 },
                { "centi", -2 },
                { "milli", -3 },
                { "micro", -6 },
                { "nano", -9 },
                { "pico", -12 },
                { "femto", -15 },
                { "atto", -18 },
                { "zepto", -21 },
                { "yocto", -24 },
                { "ronto", -27 },
                { "quecto", -30 }
            };
        public static double ToBase(double value, string fromPrefix)
        {
            int exp = GetExponent(fromPrefix);
            return value * Math.Pow(10, exp);
        }
        public static double FromBase(double valueInBase, string toPrefix)
        {
            int exp = GetExponent(toPrefix);
            return valueInBase * Math.Pow(10, -exp);
        }
        public static double Convert(double value, string fromPrefix, string toPrefix)
        {
            int fromExp = GetExponent(fromPrefix);
            int toExp = GetExponent(toPrefix);

            return value * Math.Pow(10, fromExp - toExp);
        }

        private static int GetExponent(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return 0;

            if (!_prefixExponents.TryGetValue(prefix.Trim(), out int exponent))
            {
                throw new ArgumentException(
                    "Prefijo inválido. Usa uno de: " + string.Join(", ", _prefixExponents.Keys));
            }

            return exponent;
        }
    }
}
