using System;

namespace Tools
{
    public static class UniqueIDGenerator
    {
        /// <summary>
        /// Генерирует уникальный строковый идентификатор.
        /// </summary>
        /// <returns>Уникальный идентификатор в виде строки.</returns>
        public static string Generate()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}