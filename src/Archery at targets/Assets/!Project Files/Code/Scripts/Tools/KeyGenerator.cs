#region

using System;

#endregion

namespace Tools
{
    public static class KeyGenerator
    {
        public static string GenerateKey() => Guid.NewGuid().ToString();
    }
}