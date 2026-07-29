using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using DnetIndexedDb;
using DnetIndexedDb.Models;
using DnetIndexedDb.Fluent;

class Program
{
    static void Main()
    {
        var services = new ServiceCollection();
        
        // Mock JSRuntime since IndexedDbInterop needs it
        services.AddSingleton<IJSRuntime, DummyJsRuntime>();

        services.AddIndexedDbDatabase<IndexedDbInterop>(options =>
        {
            var model = new IndexedDbDatabaseModel().WithName("Test").WithVersion(1);
            options.UseDatabase(model);
        });

        // Test resolving without forwarding
        var sp1 = services.BuildServiceProvider();
        try
        {
            var db1 = sp1.GetRequiredService<IndexedDbInterop>();
            Console.WriteLine("SUCCESS without forwarding!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("FAILED without forwarding: " + ex.Message);
        }

        // Add forwarding
        services.AddScoped<IndexedDbOptions>(sp => sp.GetRequiredService<IndexedDbOptions<IndexedDbInterop>>());
        var sp2 = services.BuildServiceProvider();
        try
        {
            var db2 = sp2.GetRequiredService<IndexedDbInterop>();
            Console.WriteLine("SUCCESS WITH forwarding!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("FAILED WITH forwarding: " + ex.Message);
        }
    }
}

class DummyJsRuntime : IJSRuntime
{
    public System.Threading.Tasks.ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args) => default;
    public System.Threading.Tasks.ValueTask<TValue> InvokeAsync<TValue>(string identifier, System.Threading.CancellationToken cancellationToken, object[] args) => default;
}
