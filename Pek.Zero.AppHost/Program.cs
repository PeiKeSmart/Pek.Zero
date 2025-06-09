var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Pek_Zero_Server>("pek-zero-server");

builder.Build().Run();
