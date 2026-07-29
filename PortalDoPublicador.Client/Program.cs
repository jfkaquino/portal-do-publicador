using DnetIndexedDb;
using DnetIndexedDb.Fluent;
using DnetIndexedDb.Models;
using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using PortalDoPublicador.Client;
using PortalDoPublicador.Client.Features.Usuarios;
using PortalDoPublicador.Client.Infrastructure;
using PortalDoPublicador.Client.Infrastructure.Sync;
using PortalDoPublicador.Shared.Infrastructure.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 1. Componentes Raiz
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 2. Configuração do Entity Framework (SQLite)
builder.Services.AddDbContext<ClientDbContext>((sp, options) =>
{
    options.UseSqlite("Filename=app.db");
});
builder.Services.AddScoped<SharedDbContext>(sp => sp.GetRequiredService<ClientDbContext>());

// 3. Configuração do IndexedDB
builder.Services.AddIndexedDbDatabase<IndexedDbInterop>(options =>
{
    var model = new IndexedDbDatabaseModel()
        .WithName("MeuAppOfflineDb")
        .WithVersion(2);

    model.AddStore("SyncPushQueue").WithKey("id");
    model.AddStore("SyncPullQueue").WithAutoIncrementingKey("id");

    options.UseDatabase(model);
});
builder.Services.AddScoped<IndexedDbOptions>(sp => sp.GetRequiredService<IndexedDbOptions<IndexedDbInterop>>());

// 4. Configuração de HTTP Clients
builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));

// 5. Serviços da Aplicação e UI
builder.Services.AddFluentUIComponents();
builder.Services.AddValidatorsFromAssemblyContaining<App>();
builder.Services.AddScoped<PullProcessor>();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<IRepository, Repository>();

// 6. Construir o Host da Aplicação
var host = builder.Build();

// 7. Inicialização do Banco de Dados (Deve ocorrer após o Build)
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClientDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// 8. Executar a Aplicação
await host.RunAsync();