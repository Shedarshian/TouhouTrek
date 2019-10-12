﻿using System.Linq;
using System.Collections.Generic;

using ZMDFQ;

namespace Tests
{
    static class TestExtension
    {
        public static int getCardID<T>(this Game game) where T : Card
        {
            return (game.Database as ConfigManager).Cards.First(p => p.Value is T).Key;
        }
        public static int getCardID(this Game game, string name)
        {
            return (game.Database as ConfigManager).Cards.First(p => p.Value.Name == name).Key;
        }
        public static IEnumerable<T> concatRepeat<T>(this IEnumerable<T> first, T value, int count)
        {
            T[] second = new T[count];
            for (int i = 0; i < second.Length; i++)
            {
                second[i] = value;
            }
            return first.Concat(second);
        }
    }
}
