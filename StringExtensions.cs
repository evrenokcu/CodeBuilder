namespace CodeBuilder;

/// <summary>
///     A static class to provide extension methods for string manipulation when generating code.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Concatenates strings
    /// </summary>
    /// <param name="strings">List of strings to concatenate.</param>
    /// <param name="joinWith">String to use between the provided list of strings when concatenating.</param>
    /// <returns>Concatenated string</returns>
    public static string JoinStrings(this IEnumerable<string> strings, string joinWith)
    {
        _ = strings ?? throw new ArgumentNullException(nameof(strings));
        var combinedString = string.Empty;
        IEnumerable<string> enumerable = strings.ToList();
        var count = enumerable.Count();
        for (var i = 0; i < count; i++)
        {
            combinedString += enumerable.ElementAt(i);
            if (i != count - 1) combinedString += joinWith;
        }

        return combinedString;
    }

    /// <summary>
    ///     Wraps a string with another string as prefix and postfix.
    ///     <example>"variable".Wrap("'") returns 'variable'.</example>
    /// </summary>
    /// <param name="str">String to wrap.</param>
    /// <param name="wrapWith">String to wrap with.</param>
    /// <returns>Wrapped string.</returns>
    public static string Wrap(this string str, string wrapWith)
    {
        return str.Wrap(wrapWith, wrapWith);
    }

    /// <summary>
    ///     Wraps a string with the provided prefix and suffix.
    ///     <example>"variable".Wrap("(",")") returns "(variable)"</example>
    /// </summary>
    /// <param name="str">String to wrap.</param>
    /// <param name="prefix">Prefix</param>
    /// <param name="suffix">Suffix</param>
    /// <returns>Concatenated string with the prefix and suffix.</returns>
    public static string Wrap(this string str, string prefix, string suffix)
    {
        _ = str ?? throw new ArgumentNullException(str);
        return prefix + str + suffix;
    }

    /// <summary>
    ///     Wraps the provided string with the quotation marks.
    /// </summary>
    /// <param name="str">String to wrap.</param>
    /// <returns>Concatenated string.</returns>
    public static string WrapQuotation(this string str)
    {
        return str.Wrap("\"");
    }

    /// <summary>
    ///     Wraps the provided string with parenthesis.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string WrapParenthesis(this string str)
    {
        return str.Wrap("(", ")");
    }
}