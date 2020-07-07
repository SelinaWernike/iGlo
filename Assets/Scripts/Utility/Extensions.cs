namespace System.Collections.Generic
{
    public static class Extensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            TValue val;
            if (!dict.TryGetValue(key, out val))
            {
                val = new TValue();
                dict.Add(key, val);
            }
            return val;
        }

        public static void ReplaceOrAdd<TValue>(this IList<TValue> list, int index, TValue value)
        {
            if (list.Count <= index)
            {
                list.Insert(index, value);
            }
            else
            {
                list[index] = value;
            }
        }
    }
}