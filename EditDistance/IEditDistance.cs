using System;
using System.Collections.Generic;

using Utilities;

namespace EditDistance
{
    public interface IEditDistance : ICompute
    {
        int EditDistance(string s1, string s2);
    }
}
