using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.CodeSnippets.DependencyInjection;

public class WhenExecutingWithinDependencyInjectionScopes
{
    [Theory]
    [MemberData(nameof(ServiceTypeInputRecords))]
    public void CanResolveRequiredServicesViaExecuteScopedWithActionParameter(Type[] requiredServiceTypes)
    {
        var services = new ServiceCollection();

        foreach (var serviceType in requiredServiceTypes)
            services.AddScoped(serviceType);

        using var serviceProvider = services.BuildServiceProvider();

        var dependencyInjectionScopeHelper = new DependencyInjectionScopeHelper(serviceProvider);

        var executeAction = ExecuteDelegates[requiredServiceTypes.Length - 1]!;
        var executeActionGenericParameters = Enumerable
            .Range(0, requiredServiceTypes.Length)
            .Select(Type.MakeGenericMethodParameter)
            .ToArray();

        var executeScopedMethoInfo = dependencyInjectionScopeHelper
            .GetType()
            .GetMethod(
                "ExecuteScoped",
                requiredServiceTypes.Length,
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { executeAction
                    .GetType()
                    .GetGenericTypeDefinition()
                    .MakeGenericType(executeActionGenericParameters) },
                null)!;

        var genericExecuteMethodInfo = executeScopedMethoInfo.MakeGenericMethod(requiredServiceTypes);
        genericExecuteMethodInfo.Invoke(dependencyInjectionScopeHelper, new[] { executeAction });
    }

    public static IEnumerable<object[]> ServiceTypeInputRecords = new object[][]
    {
        new object[]{ new Type[]{ typeof(Service1) } },
        new object[]{ new Type[]{ typeof(Service1), typeof(Service2) } },
        new object[]{ new Type[]{ typeof(Service1), typeof(Service2), typeof(Service3) } },
        new object[]{ new Type[]{ typeof(Service1), typeof(Service2), typeof(Service3), typeof(Service4) } },
        new object[]{ new Type[]{ typeof(Service1), typeof(Service2), typeof(Service3), typeof(Service4), typeof(Service5) } },
        new object[]{ new Type[]{ typeof(Service1), typeof(Service2), typeof(Service3), typeof(Service4), typeof(Service5), typeof(Service6) } },
        new object[]{ new Type[]{ typeof(Service1), typeof(Service2), typeof(Service3), typeof(Service4), typeof(Service5), typeof(Service6), typeof(Service7) } },
        new object[]{ new Type[]{ typeof(Service1), typeof(Service2), typeof(Service3), typeof(Service4), typeof(Service5), typeof(Service6), typeof(Service7), typeof(Service8) } },
    };

    private record class Service1();
    private record class Service2();
    private record class Service3();
    private record class Service4();
    private record class Service5();
    private record class Service6();
    private record class Service7();
    private record class Service8();

    private static void Execute1(Service1 s1) { }
    private static void Execute2(Service1 s1, Service2 s2) { }
    private static void Execute3(Service1 s1, Service2 s2, Service3 s3) { }
    private static void Execute4(Service1 s1, Service2 s2, Service3 s3, Service4 s4) { }
    private static void Execute5(Service1 s1, Service2 s2, Service3 s3, Service4 s4, Service5 s5) { }
    private static void Execute6(Service1 s1, Service2 s2, Service3 s3, Service4 s4, Service5 s5, Service6 s6) { }
    private static void Execute7(Service1 s1, Service2 s2, Service3 s3, Service4 s4, Service5 s5, Service6 s6, Service7 s7) { }
    private static void Execute8(Service1 s1, Service2 s2, Service3 s3, Service4 s4, Service5 s5, Service6 s6, Service7 s7, Service8 s8) { }

    private static Delegate[] ExecuteDelegates = Enumerable
        .Range(1, 8)
        .Select(n =>
            typeof(WhenExecutingWithinDependencyInjectionScopes)
                .GetMethod($"Execute{n}", BindingFlags.NonPublic | BindingFlags.Static)!)
        .Select(CreateActionDelegate)
        .ToArray();

    private static Delegate CreateActionDelegate(MethodInfo methodInfo) =>
        Delegate.CreateDelegate(
            Expression.GetActionType(methodInfo.GetParameters().Select(p => p.ParameterType).ToArray()),
            methodInfo);
}