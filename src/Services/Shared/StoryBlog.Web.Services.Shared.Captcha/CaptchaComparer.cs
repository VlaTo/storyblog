using System;
using System.Collections.Generic;
using System.Globalization;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    internal class CaptchaComparer : IEqualityComparer<char[]>
    {
        public static readonly CaptchaComparer Ordinal;
        public static readonly CaptchaComparer OrdinalIgnoreCase;

        private readonly Func<char, char, bool> comparison;

        protected CaptchaComparer(Func<char, char, bool> comparison)
        {
            this.comparison = comparison;
        }

        static CaptchaComparer()
        {
            Ordinal = new CaptchaComparer(OrdinalComparison);
            OrdinalIgnoreCase = new CaptchaComparer(OrdinalIgnoreCaseComparison);
        }

        public bool Equals(char[] x, char[] y)
        {
            if (ReferenceEquals(null, x))
            {
                return false;
            }

            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return CheckEqual(x, y);
        }

        public int GetHashCode(char[] obj)
        {
            return obj.GetHashCode();
        }

        private bool CheckEqual(char[] x, char[] y)
        {
            if (ReferenceEquals(null, y))
            {
                return false;
            }

            if (x.Length != y.Length)
            {
                return false;
            }

            for (var index = 0; index < x.Length; index++)
            {
                if (comparison.Invoke(x[index], y[index]))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private static bool OrdinalComparison(char x, char y) => x == y;

        private static bool OrdinalIgnoreCaseComparison(char x, char y) => Char.ToLower(x) == Char.ToLower(y);

        private static bool InvariantIgnoreCaseComparison(char x, char y) => Char.ToLowerInvariant(x) == Char.ToLowerInvariant(y);
    }
}