using System.Text.RegularExpressions;

namespace Kaizen.API.Configuration;

// https://stackoverflow.com/a/79196750
public partial class ToKebabParameterTransformer : IOutboundParameterTransformer
{
    [GeneratedRegex("([a-z])([A-Z])")]
    public static partial Regex KebabCaseGeneratedRegex();

    public string? TransformOutbound(object? value)
    {
        return value is string s ? KebabCaseGeneratedRegex().Replace(s, "$1-$2").ToLower() : null;
    }
}