using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace MediaVerse.Client.Application.Services.Transformer;

public partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object value)
    {
        return value == null ? null : MyRegex().Replace(value.ToString(), "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}