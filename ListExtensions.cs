namespace CodeBuilder;

public static class ListExtensions
{
    public static string IfNotEmpty<T>(this IList<T> list, Func<string> stringFactory) =>
        list.Any() ? stringFactory.Invoke() : string.Empty;
}