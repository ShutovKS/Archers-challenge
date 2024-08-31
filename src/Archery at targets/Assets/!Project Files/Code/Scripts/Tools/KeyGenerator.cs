using System;

namespace Tools
{
    public static class KeyGenerator
    {
        public static string GenerateKey() => Guid.NewGuid().ToString();
    }
}