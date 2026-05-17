namespace Outbox.Api.Minimal.Tests.Integration.Middleware;

internal sealed record ProblemDetailsResponse(string Title, int Status, string Detail);
