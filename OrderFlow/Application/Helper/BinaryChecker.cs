using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Helper
{
    public class BinaryChecker
    {
        public static bool ContainsHttp(string binary)
        {
            var linkPrefix = new string(binary.Take(5).ToArray()).ToLower();

            return linkPrefix.Contains("http");
        }
    }
}
