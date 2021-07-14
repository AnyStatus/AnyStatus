﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
        }

        public static bool None<T>(this IEnumerable<T> source) => !source.Any();
    }
}
