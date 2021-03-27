using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, value);
        }
        return dic;
    }
    public static bool NestedTryAdd<TKey, TKey1, TValue1>(this Dictionary<TKey, Dictionary<TKey1, TValue1>> dic, TKey key, TKey1 key1, TValue1 value)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, new Dictionary<TKey1, TValue1>());
        }
        if (!dic[key].ContainsKey(key1))
        {
            dic[key].Add(key1, value);
            return true;
        }
        return false;
    }
    public static bool NestedContainsKey<TKey, TKey1, TValue1>(this Dictionary<TKey, Dictionary<TKey1, TValue1>> dic, TKey key, TKey1 key1)
    {
        if (!dic.ContainsKey(key)) return false;
        if (dic[key].ContainsKey(key1)) return true;
        return false;
    }
    public static bool NestedTryGetValue<TKey, TKey1, TValue1>(this Dictionary<TKey, Dictionary<TKey1, TValue1>> dic, TKey key, TKey1 key1, out TValue1 value1)
    {
        value1 = default;
        if (!dic.ContainsKey(key)) return false;
        return !dic[key].TryGetValue(key1, out value1);
    }
}
