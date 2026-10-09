using DnetIndexedDb;
using DnetIndexedDb.Fluent;
using DnetIndexedDb.Models;
using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using PortalDoPublicador.Client;
using PortalDoPublicador.Client.Infrastructure.Data;
using PortalDoPublicador.Client.Infrastructure.Sync;
using PortalDoPublicador.Shared.Features.Publicadores.DTOs;
using PortalDoPublicador.Shared.Features.Publicadores.Validators;
using PortalDoPublicador.Shared.Infrastructure.Data;
using SqliteWasmBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 1. Componentes Raiz
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 2. Configuração do Entity Framework (SQLite)
Action<IServiceProvider, DbContextOptionsBuilder> dbOptions = (sp, options) =>
{
    var connection = new SqliteWasmConnection("Data Source=app.db");
    options.UseSqliteWasm(connection);
    options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.SqliteEventId.UnexpectedConnectionTypeWarning));
};

// Usa apenas AddDbContext (que por padrão é Scoped)
builder.Services.AddDbContext<ClientDbContext>(dbOptions);

builder.Services.AddSqliteWasm();
builder.Services.AddScoped<SharedDbContext>(sp => sp.GetRequiredService<ClientDbContext>());

// 3. Configuração do IndexedDB
builder.Services.AddIndexedDbDatabase<IndexedDbInterop>(options =>
{
    var model = new IndexedDbDatabaseModel()
        .WithName("MeuAppOfflineDb")
        .WithVersion(6);

    var pushStore = model.AddStore("SyncPushQueue").WithKey("id");
    pushStore.Indexes = new List<IndexedDbIndex>
    {
        new() { Name = "Timestamp" }
    };

    var pullStore = model.AddStore("SyncPullQueue").WithAutoIncrementingKey("id");
    pullStore.Indexes = new List<IndexedDbIndex>
    {
        new() { Name = "Timestamp" }
    };

    var configStore = model.AddStore("Configuracoes").WithKey("chave");
    configStore.Indexes = new List<IndexedDbIndex>
    {
        new() { Name = "dummy" }
    };

    options.UseDatabase(model);
});
builder.Services.AddScoped<IndexedDbOptions>(sp => sp.GetRequiredService<IndexedDbOptions<IndexedDbInterop>>());

// 4. Configuração de HTTP Clients
builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));

// 5. Serviços da Aplicação e UI
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, PortalDoPublicador.Client.Infrastructure.Auth.SimpleAuthStateProvider>();
builder.Services.AddFluentUIComponents();
builder.Services.AddValidatorsFromAssemblyContaining<App>();
builder.Services.AddValidatorsFromAssemblyContaining<NovoUsuarioDtoValidator>();
Mapster.TypeAdapterConfig.GlobalSettings.Scan(typeof(NovoUsuarioDtoConfig).Assembly);
builder.Services.AddScoped<PullProcessor>();
builder.Services.AddScoped<SyncService>();

// Configura o FluentValidation globalmente para pt-BR
ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("pt-BR");

// 6. Construir o Host da Aplicação
var host = builder.Build();

// 7. Inicialização do Banco de Dados (Deve ocorrer após o Build)
await host.Services.InitializeSqliteWasmDatabaseAsync<ClientDbContext>();

using (var scope = host.Services.CreateScope())
{
    var indexedDb = scope.ServiceProvider.GetRequiredService<IndexedDbInterop>();
    await indexedDb.OpenIndexedDb();

    var dbContext = scope.ServiceProvider.GetRequiredService<ClientDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// 8. Executar a Aplicação
await host.RunAsync();