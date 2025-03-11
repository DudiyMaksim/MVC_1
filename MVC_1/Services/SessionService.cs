using MVC_1.Settings;
using MVC_1.ViewModels;
using System.Text.Json;

namespace MVC_1.Services
{
    public static class SessionService
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            string json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            if (json == null)
                return default;

            var value = JsonSerializer.Deserialize<T>(json);
            return value;
        }

        public static int GetCartItemsCount(this ISession session)
        {
            var items = session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);

            if (items == null)
                return 0;

            return items.Count();
        }
    }
}