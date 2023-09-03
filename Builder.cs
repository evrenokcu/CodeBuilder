namespace CodeBuilder;

/// <summary>
/// Abstract class to generate code
/// </summary>
/// <typeparam name="TInterface"></typeparam>
/// <typeparam name="TClass"></typeparam>
public abstract class Builder<TInterface, TClass> where TClass : TInterface where TInterface : class
{
    public TClass With(bool condition, Action<TClass> action)
    {
        if (!condition)
            return GetBuilder();

        var builder = GetBuilder();
        action.Invoke(builder);
        return builder;
    }

    public TClass With(Action<TClass> action) => With(true, action);

    protected abstract TClass GetBuilder();

    public TClass WithEach<T>(IList<T>? list, Action<TClass, T> action, bool condition = true)
    {
        if (!condition)
            return GetBuilder();

        list?.ToList().ForEach(item => action.Invoke(GetBuilder(), item));
        return GetBuilder();
    }

    public TClass WithEach<T>(IEnumerable<T> list, Action<TClass, T> action, bool condition = true)
    {
        if (!condition)
            return GetBuilder();
        list.ToList().ForEach(item => action.Invoke(GetBuilder(), item));
        return GetBuilder();
    }

    public TClass WithEach<T>(IEnumerable<T> list, Action<TClass, T, int> action, bool condition = true)
    {
        if (!condition)
            return GetBuilder();
        int iteration = 0;
        list.ToList().ForEach(item => action.Invoke(GetBuilder(), item, iteration++));
        return GetBuilder();
    }
}