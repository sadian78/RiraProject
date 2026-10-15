using RiraProject.Users.Application;
using RiraProject.Users.Infrastructure;
using RiraProject.Users.Presentation;
using RiraProject.Users.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

// ?. ????? ??????? ???????
builder.Services.AddUsersApplication();
builder.Services.AddUsersInfrastructure(builder.Configuration);
builder.Services.AddUsersPresentation();

var app = builder.Build();

// ?. ?? ???? Endpoint ????? gRPC
app.MapGrpcService<UserGrpcServiceImpl>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();