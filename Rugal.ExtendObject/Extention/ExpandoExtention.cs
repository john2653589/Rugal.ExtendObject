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
        public static IDictionary<string, object> WithObjectProperty<TSource>(this IDictionary<string, object> Map, TSource Source)
            where TSource : class
        {
            foreach (var Item in GetPairs(Source))
            {
                if (!Map.TryAdd(Item.Key, Item.Value))
                    Map[Item.Key] = Item.Value;
            }
            return Map;
        }
        private static IEnumerable<KeyValuePair<string, object>> GetPairs<TSource>(TSource Source)
            where TSource : class
        {
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