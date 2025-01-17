using System.Text.Json;

namespace ASP.NET.Models;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }
    
    public static T Get<T>(this ISession session, string key)
    {
        var json = session.GetString(key);
        if (string.IsNullOrEmpty(json))
        {
            return default(T);
        }
        else
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
    
}