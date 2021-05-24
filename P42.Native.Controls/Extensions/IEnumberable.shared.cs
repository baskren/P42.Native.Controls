using System;
using System.Collections;

namespace P42.Native.Controls
{

    public static class IEnumerableExtensions
    {
        public static bool Any(this IEnumerable items)
        {
            foreach (var item in items)
                return true;
            return false;
        }

        public static int Count(this IEnumerable items)
        {
            if (items is ICollection list)
                return list.Count;
            int count = 0;
            foreach (var item in items)
                count++;
            return count;
        }

        public static object ElementAt(this IEnumerable items, int index)
        {
            if (index < 0)
                return null;
            if (items is IList list && index < list.Count)
                return list[index];
            int i = 0;
            foreach (var item in items)
            {
                if (i == index)
                    return item;
                i++;
            }
            return null;
        }

        public static int IndexOf(this IEnumerable items, object item)
        {
            if (item is null)
                return -1;
            if (items is IList list)
                return list.IndexOf(item);
            int index = 0;
            foreach (var i in items)
            {
                if (i == item)
                    return index;
                index++;
            }
            return -1;
        }

    }
}