using Flowstate.Data.UnityOfWork;
using Flowstate.Data.UnityOfWork.Default;
using Microsoft.Data.Sqlite;
using SampleWebApp.Application;
using SampleWebApp.Domain;
using SampleWebApp.Infrastructure.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add scoped unity of work context for chosen DB provider
builder.Services.AddScoped(_ =>
    DbUnityOfWorkManagerFactory.Create<SqliteConnection, SqliteTransaction>(() =>
        new("Data Source=InMemorySample;Mode=Memory;Cache=Shared")));

// Add interface for app layer consumption
builder.Services.AddScoped<IUnityOfWorkManager>(p => 
    p.GetRequiredService<IDbUnityOfWorkManager<SqliteConnection, SqliteTransaction>>());

// Add interface for repository consumption
builder.Services.AddScoped<IDbUnityOfWorkContext<SqliteConnection, SqliteTransaction>>(p =>
    p.GetRequiredService<IDbUnityOfWorkManager<SqliteConnection, SqliteTransaction>>());

// Repositories
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

// Application Layer 
builder.Services.AddScoped<CreateTodoUseCase>();


var app = builder.Build();

app.MapPost("todos", () => "oi");
app.MapGet("todos", () => "oi");

app.Run();
