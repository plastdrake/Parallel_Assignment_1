using System;

namespace Utilities
{
    public static class RandomExtensions
    {
        /// <summary>
        ///  Fisher-Yates / Knuth Shuffle to make the ordering of an array random.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rng"></param>
        /// <param name="array"></param>
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1) {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        /// <summary>
        ///  Creates a random string.
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String RandomString(this Random rng, int length)
        {
            char[] str = new char[length];

            for (int i = 0; i < str.Length; ++i) {
                str[i] = (char)rng.Next(65, 65 + 26);
            }

            return new String(str);
        }
    }
}
