using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Anthropic.Exceptions;

public class AnthropicApiException : AnthropicServiceException
{
    public new HttpRequestException InnerException
    {
        get
        {
            if (base.InnerException == null)
            {
                throw new ArgumentNullException();
            }
            return (HttpRequestException)base.InnerException;
        }
    }

    public AnthropicApiException(string message, HttpRequestException? innerException = null)
        : base(message, innerException) { }

    protected AnthropicApiException(HttpRequestException? innerException)
        : base(innerException) { }

    public required HttpStatusCode StatusCode { get; init; }

    public required string ResponseBody { get; init; }

    /// <summary>
    /// Headers of the failed response, keyed case-insensitively. A snapshot: the
    /// response itself is disposed once the exception is thrown. Some error
    /// semantics live only here — for example whether a 429 came from a usage
    /// limit (<c>anthropic-ratelimit-unified-*</c>) or a transient rate limit
    /// (<c>retry-after</c>).
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyList<string>> ResponseHeaders { get; init; } =
        EmptyHeaders;

    internal static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> EmptyHeaders =
        new ReadOnlyDictionary<string, IReadOnlyList<string>>(
            new Dictionary<string, IReadOnlyList<string>>()
        );

    internal static IReadOnlyDictionary<string, IReadOnlyList<string>> SnapshotHeaders(
        HttpResponseHeaders headers
    )
    {
        var copy = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in headers)
        {
            copy[header.Key] = Array.AsReadOnly(header.Value.ToArray());
        }
        return new ReadOnlyDictionary<string, IReadOnlyList<string>>(copy);
    }

    public override string Message
    {
        get
        {
            return ResponseBody.Length == 0
                ? string.Format("Status Code: {0}", StatusCode)
                : string.Format("Status Code: {0}\n{1}", StatusCode, ResponseBody);
        }
    }
}
