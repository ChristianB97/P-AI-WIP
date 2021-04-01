using System.Collections.Generic;
using System.Linq;

namespace LS.Editor.Helpers
{
    public static class StringHelper
    {
        public static int Count(string str, char find)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == find)
                    ++count;
            }

            return count;
        }

        public static string[] SplitAndTrim(string str, char separator)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return str.Split(separator)
                .Where(m => !string.IsNullOrEmpty(m))
                .Select(m => m.Trim())
                .ToArray();
        }
    }
}
