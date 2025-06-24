namespace Pokorm.AdventOfCode.Helpers;

public static class Extensions
{
    public static T AddOrUpdate<T, TKey, TValue>(this T dict, TKey key, TValue initial, Func<TValue, TValue> update)
        where T : IDictionary<TKey, TValue>
    {
        if (!dict.TryAdd(key, initial))
        {
            dict[key] = update(dict[key]);
        }

        return dict;
    }
}
