var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.PortalDoPublicador_Server>("portaldopublicador-server");
builder.AddProject<Projects.PortalDoPublicador_Client>("portaldopublicador-client")
       .WithReference(api);

builder.Build().Run();
