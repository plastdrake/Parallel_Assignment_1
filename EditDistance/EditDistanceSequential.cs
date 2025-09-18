using System;
using System.Collections.Generic;

namespace EditDistance
{
    public class EditDistanceSequential : IEditDistance
    {
        public string Name
        {
            get { return nameof(EditDistanceSequential); }
        }

        public int EditDistance(string s1, string s2)
        {
            int[,] dist = new int[s1.Length + 1, s2.Length + 1];
            for (int i = 0; i <= s1.Length; i++) {
                dist[i, 0] = i;
            }
            for (int j = 0; j <= s2.Length; j++) {
                dist[0, j] = j;
            }
            for (int i = 1; i <= s1.Length; i++) {
                for (int j = 1; j <= s2.Length; j++) {
                    dist[i, j] =
                        (s1[i - 1] == s2[j - 1])
                        ? dist[i - 1, j - 1]
                        : 1 + Math.Min(dist[i - 1, j],
                                       Math.Min(dist[i, j - 1], dist[i - 1, j - 1]));
                }
            }
            return dist[s1.Length, s2.Length];
        }
    }
}
