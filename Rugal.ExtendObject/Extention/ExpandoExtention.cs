using System.Reflection;

namespace Rugal.ExtendObject.Extention
{
    public static class ExpandoExtention
    {
        public static IDictionary<string, object> Extend<TSource>(this object Target, TSource Source)
            where TSource : class
        {
            var Result = new Dictionary<string, object>()
                .WithObjectProperty(Target)
                .WithObjectProperty(Source);

            return Result;
        }
        public static IDictionary<string, object> ExtendWith<TSource>(this object Target, TSource Source, Func<TSource, object> WithFunc)
            where TSource : class
        {
            var Result = new Dictionary<string, object>()
                .WithObjectProperty(Target);

            if (Source is not null)
                Result.WithObjectProperty(WithFunc(Source));

            return Result;
        }
        public static IDictionary<string, object> ExtendExcept<TSource>(this object Target, TSource Source, Func<TSource, object> ExceptFunc)
           where TSource : class
        {
            var Result = new Dictionary<string, object>()
                .WithObjectProperty(Target);

            if (Source is not null)
            {
                var ExceptKeys = GetPairs(ExceptFunc(Source))
                    .Select(Item => Item.Key);

                Result.WithObjectProperty(Source, ExceptKeys);
            }
            return Result;
        }
        public static IDictionary<string, object> WithObjectProperty<TSource>(this IDictionary<string, object> Map, TSource Source, IEnumerable<string> ExceptKeys = null) where TSource : class
        {
            if (Source is null)
                return Map;

            var Pairs = GetPairs(Source);
            if (Pairs is null)
                return Map;

            foreach (var Item in GetPairs(Source))
            {
                if (ExceptKeys != null && ExceptKeys.Contains(Item.Key))
                    continue;

                if (!Map.TryAdd(Item.Key, Item.Value))
                    Map[Item.Key] = Item.Value;
            }
            return Map;
        }
        private static IEnumerable<KeyValuePair<string, object>> GetPairs<TSource>(TSource Source)
            where TSource : class
        {
            if (Source is null)
                return null;

            var Properties = Source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var Result = Properties
                .Select(Property =>
                {
                    var Key = Property.Name;
                    var Value = Property.GetValue(Source);
                    var Pair = new KeyValuePair<string, object>(Key, Value);
                    return Pair;
                });

            return Result;
        }
    }
}