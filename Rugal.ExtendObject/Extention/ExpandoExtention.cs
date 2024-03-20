using System.Reflection;

namespace Rugal.ExtendObject.Extention
{
    public static class ExpandoExtention
    {
        public static IDictionary<string, object> Extend(this object Target, object Source)
        {
            var Result = new Dictionary<string, object>()
                .WithObjectProperty(Target)
                .WithObjectProperty(Source);

            return Result;
        }
        public static IDictionary<string, object> WithObjectProperty(this IDictionary<string, object> Map, object Source)
        {
            foreach (var Item in GetPairs(Source))
            {
                if (!Map.TryAdd(Item.Key, Item.Value))
                    Map[Item.Key] = Item.Value;
            }
            return Map;
        }
        private static IEnumerable<KeyValuePair<string, object>> GetPairs(object Source)
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