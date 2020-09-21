using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Common
{
    public class RandomGenerator
    {
        public static string GenerateRandomKey(int length)
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append((char)random.Next('a', 'z'));
            }

            return builder.ToString();
        }
    }
}
