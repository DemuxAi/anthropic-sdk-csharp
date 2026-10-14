# Changelog

## 0.2.0 (2026-10-14)

### Features

* **client:** add OpenTelemetry tracing. `AnthropicGoogleCloudClient` emits its spans from the `com.anthropic.sdk.csharp.googlecloud` activity source. Listen to that source: listening to `com.anthropic.sdk.csharp` alone does not cover it
