using Api.Extensions;
using Shared.Constants;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppSettings>()
    ?? throw new Exception("build fail");

builder.Services.AddSingleton(configuration);
builder.ConfigureServices(configuration);

var app = builder.Build();

app.ConfigurePipelineAsync(configuration);

app.Run();

// test
public partial class Program { }