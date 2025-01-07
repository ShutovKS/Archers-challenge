#region

using System;

#endregion

public static class KeyGenerator
{
    public static string GenerateKey() => Guid.NewGuid().ToString();
}