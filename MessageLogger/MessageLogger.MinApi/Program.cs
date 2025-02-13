using System.Runtime.Intrinsics.Arm;
using System.Text.Json.Serialization;
using MessageLogger.Application;
using MessageLogger.Application.Models;
using MessageLogger.Application.Repositories;
using MessageLogger.Contracts.Requests;
using MessageLogger.Contracts.Responses;
using MessageLogger.MinApi.Mapping;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddApplication();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
var app = builder.Build();

// POST: Create a log
app.MapPost("api/logs", async (CreateLogRequest request, ILogRepository repo) =>
{
    var log = request.MapToLog();
    await repo.CreateAsync(log);
    var response = log.MapToResponse();

    return Results.Created($"/logs/{log.Id}", response);
});

// GET: Retrieve a single log by ID
app.MapGet("api/logs/{id:guid}", async (Guid id, ILogRepository repo) =>
{
    var log = await repo.GetByIdAsync(id);

    return log is not null
        ? Results.Ok(log.MapToResponse())
        : Results.NotFound();
});

// GET: Retrieve all logs
app.MapGet("api/logs", async (ILogRepository repo) =>
{
    var logs = await repo.GetAllAsync();
    var logsResponse = logs.MapToResponse();

    return Results.Ok(logsResponse);
});

app.Run();

[JsonSerializable(typeof(LogMessage))]
[JsonSerializable(typeof(CreateLogRequest))]
[JsonSerializable(typeof(LogResponse))]
[JsonSerializable(typeof(LogsResponse))]
[JsonSerializable(typeof(List<LogResponse>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
