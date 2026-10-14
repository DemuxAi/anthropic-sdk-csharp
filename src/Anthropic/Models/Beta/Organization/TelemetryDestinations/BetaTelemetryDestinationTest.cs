using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Anthropic.Core;
using Anthropic.Exceptions;

namespace Anthropic.Models.Beta.Organization.TelemetryDestinations;

/// <summary>
/// The result of one test span sent to a destination.
/// </summary>
[JsonConverter(
    typeof(JsonModelConverter<BetaTelemetryDestinationTest, BetaTelemetryDestinationTestFromRaw>)
)]
public sealed record class BetaTelemetryDestinationTest : JsonModel
{
    /// <summary>
    /// How the test went: `success` when the destination answered with a 2xx status,
    /// else `failure` with the reason.
    /// </summary>
    public required BetaTelemetryDestinationTestResult Result
    {
        get
        {
            this._rawData.Freeze();
            return this._rawData.GetNotNullClass<BetaTelemetryDestinationTestResult>("result");
        }
        init { this._rawData.Set("result", value); }
    }

    public JsonElement Type
    {
        get
        {
            this._rawData.Freeze();
            return this._rawData.GetNotNullStruct<JsonElement>("type");
        }
        init { this._rawData.Set("type", value); }
    }

    /// <inheritdoc/>
    public override void Validate()
    {
        this.Result.Validate();
        if (
            !JsonElement.DeepEquals(
                this.Type,
                JsonSerializer.SerializeToElement("telemetry_destination_test")
            )
        )
        {
            throw new AnthropicInvalidDataException("Invalid value given for constant");
        }
    }

    public BetaTelemetryDestinationTest()
    {
        this.Type = JsonSerializer.SerializeToElement("telemetry_destination_test");
    }

#pragma warning disable CS8618
    [SetsRequiredMembers]
    public BetaTelemetryDestinationTest(BetaTelemetryDestinationTest betaTelemetryDestinationTest)
        : base(betaTelemetryDestinationTest) { }
#pragma warning restore CS8618

    public BetaTelemetryDestinationTest(IReadOnlyDictionary<string, JsonElement> rawData)
    {
        this._rawData = new(rawData);

        this.Type = JsonSerializer.SerializeToElement("telemetry_destination_test");
    }

#pragma warning disable CS8618
    [SetsRequiredMembers]
    BetaTelemetryDestinationTest(FrozenDictionary<string, JsonElement> rawData)
    {
        this._rawData = new(rawData);
    }
#pragma warning restore CS8618

    /// <inheritdoc cref="BetaTelemetryDestinationTestFromRaw.FromRawUnchecked"/>
    public static BetaTelemetryDestinationTest FromRawUnchecked(
        IReadOnlyDictionary<string, JsonElement> rawData
    )
    {
        return new(FrozenDictionary.ToFrozenDictionary(rawData));
    }

    [SetsRequiredMembers]
    public BetaTelemetryDestinationTest(BetaTelemetryDestinationTestResult result)
        : this()
    {
        this.Result = result;
    }
}

class BetaTelemetryDestinationTestFromRaw : IFromRawJson<BetaTelemetryDestinationTest>
{
    /// <inheritdoc/>
    public BetaTelemetryDestinationTest FromRawUnchecked(
        IReadOnlyDictionary<string, JsonElement> rawData
    ) => BetaTelemetryDestinationTest.FromRawUnchecked(rawData);
}
