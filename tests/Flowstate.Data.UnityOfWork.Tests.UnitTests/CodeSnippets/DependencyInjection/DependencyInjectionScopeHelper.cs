using Microsoft.Extensions.DependencyInjection;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.CodeSnippets.DependencyInjection;

public sealed class DependencyInjectionScopeHelper 
{
    // Worth it? Not sure. It's not compatible with .netstandard2.1 and it's still a service locator.
    // Maybe it's better to let users manage scope by themselves for bg workers.

    private readonly IServiceProvider _serviceProvider;

    public DependencyInjectionScopeHelper(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    public void ExecuteScoped<T1>(Action<T1> execute)
        where T1 : class
    {
        using var scope = _serviceProvider.CreateScope();
        execute(scope.ServiceProvider.GetRequiredService<T1>());
    }

    public async Task ExecuteScopedAsync<T1>(Func<T1, Task> execute)
        where T1 : class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await execute(scope.ServiceProvider.GetRequiredService<T1>());
    }


    public void ExecuteScoped<T1, T2>(Action<T1, T2> execute)
        where T1 : class
        where T2 : class
    {
        using var scope = _serviceProvider.CreateScope();

        execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>());
    }

    public async Task ExecuteScopedAsync<T1, T2>(Func<T1, T2, Task> execute)
        where T1 : class
        where T2 : class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>());
    }


    public void ExecuteScoped<T1, T2, T3>(Action<T1, T2, T3> execute)
        where T1 : class
        where T2 : class
        where T3 : class
    {
        using var scope = _serviceProvider.CreateScope();

        execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>());
    }

    public async Task ExecuteScopedAsync<T1, T2, T3>(Func<T1, T2, T3, Task> execute)
        where T1 : class
        where T2 : class
        where T3 : class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>());
    }


    public void ExecuteScoped<T1, T2, T3, T4>(Action<T1, T2, T3, T4> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
    {
        using var scope = _serviceProvider.CreateScope();

        execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>());
    }

    public async Task ExecuteScopedAsync<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>());
    }



    public void ExecuteScoped<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
    {
        using var scope = _serviceProvider.CreateScope();

        execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>(),
            scope.ServiceProvider.GetRequiredService<T5>());
    }

    public async Task ExecuteScopedAsync<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Task> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>(),
            scope.ServiceProvider.GetRequiredService<T5>());
    }



    public void ExecuteScoped<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
    {
        using var scope = _serviceProvider.CreateScope();

        execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>(),
            scope.ServiceProvider.GetRequiredService<T5>(),
            scope.ServiceProvider.GetRequiredService<T6>());
    }

    public async Task ExecuteScopedAsync<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Task> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>(),
            scope.ServiceProvider.GetRequiredService<T5>(),
            scope.ServiceProvider.GetRequiredService<T6>());
    }


    public void ExecuteScoped<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
    {
        using var scope = _serviceProvider.CreateScope();

        execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>(),
            scope.ServiceProvider.GetRequiredService<T5>(),
            scope.ServiceProvider.GetRequiredService<T6>(),
            scope.ServiceProvider.GetRequiredService<T7>());
    }

    public async Task ExecuteScopedAsync<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, Task> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>(),
            scope.ServiceProvider.GetRequiredService<T5>(),
            scope.ServiceProvider.GetRequiredService<T6>(),
            scope.ServiceProvider.GetRequiredService<T7>());
    }

    public void ExecuteScoped<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
    {
        using var scope = _serviceProvider.CreateScope();

        execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>(),
            scope.ServiceProvider.GetRequiredService<T5>(),
            scope.ServiceProvider.GetRequiredService<T6>(),
            scope.ServiceProvider.GetRequiredService<T7>(),
            scope.ServiceProvider.GetRequiredService<T8>());
    }

    public async Task ExecuteScopedAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> execute)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await execute(
            scope.ServiceProvider.GetRequiredService<T1>(),
            scope.ServiceProvider.GetRequiredService<T2>(),
            scope.ServiceProvider.GetRequiredService<T3>(),
            scope.ServiceProvider.GetRequiredService<T4>(),
            scope.ServiceProvider.GetRequiredService<T5>(),
            scope.ServiceProvider.GetRequiredService<T6>(),
            scope.ServiceProvider.GetRequiredService<T7>(),
            scope.ServiceProvider.GetRequiredService<T8>());
    }
}