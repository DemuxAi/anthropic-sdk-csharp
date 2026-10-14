namespace Anthropic.Core;

/// <summary>
/// The telemetry that a client emits through OpenTelemetry.
///
/// <para>A <c>bool</c> converts to this type, so <c>OpenTelemetry = false</c> is the short form of
/// <c>OpenTelemetry = new() { Traces = false }</c>.</para>
///
/// <para>Options that <c>WithOptions</c> sets take the place of those of the client it was called
/// on, as a whole: the two are not merged, so the new options hold every setting that the new
/// client is to have. What they leave unset is as the environment variables had it when the
/// first client was made.</para>
/// </summary>
public sealed record class OpenTelemetryOptions
{
    /// <summary>
    /// The spans that the client emits. A <c>bool</c> converts to
    /// <see cref="OpenTelemetryTraces"/>, so <c>Traces = false</c> is the short form of
    /// <c>Traces = new() { Enabled = false }</c>. A setting that this leaves unset has its
    /// default.
    /// </summary>
    public OpenTelemetryTraces? Traces { get; init; }

    /// <summary>
    /// Whether the client sends the trace context of a call's span with the call's requests, in
    /// the headers that the registered <c>System.Diagnostics.DistributedContextPropagator</c>
    /// writes, such as <c>traceparent</c> and <c>baggage</c>. When false, it writes none of
    /// them, and still emits its spans. That is for an application whose HTTP client
    /// instrumentation already propagates the trace context, so that only the instrumentation
    /// writes it.
    ///
    /// <para>When unset, it is true unless the <c>ANTHROPIC_OPEN_TELEMETRY_PROPAGATION</c>
    /// environment variable is <c>false</c>.</para>
    /// </summary>
    public bool? Propagation { get; init; }

    /// <summary>
    /// Sets <see cref="Traces"/> to <paramref name="traces"/>.
    /// </summary>
    public static implicit operator OpenTelemetryOptions(bool traces) => new() { Traces = traces };
}

/// <summary>
/// The spans that a client emits, and what they hold.
///
/// <para>A <c>bool</c> converts to this type, so <c>Traces = true</c> is the short form of
/// <c>Traces = new() { Enabled = true }</c>.</para>
/// </summary>
public sealed record class OpenTelemetryTraces
{
    /// <summary>
    /// Whether the client emits a span for each API call. When false, it emits none, and the
    /// other settings are not used.
    ///
    /// <para>The spans come from the <c>System.Diagnostics.ActivitySource</c> named
    /// <c>com.anthropic.sdk.csharp</c>, so a call emits one only when a listener is registered on
    /// that source. An application that uses the OpenTelemetry .NET SDK registers one by adding the
    /// source to its tracer provider.</para>
    ///
    /// <para>When unset, it is true unless the <c>ANTHROPIC_OPEN_TELEMETRY</c> environment variable is
    /// <c>false</c>.</para>
    /// </summary>
    public bool? Enabled { get; init; }

    /// <summary>
    /// Sets <see cref="Enabled"/> to <paramref name="enabled"/>.
    /// </summary>
    public static implicit operator OpenTelemetryTraces(bool enabled) =>
        new() { Enabled = enabled };
}
