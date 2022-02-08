using Carter;

using MartenRepro.Api;
using MartenRepro.Core.Domain.TemplateFolder;
using MartenRepro.Core.Infrastructure;

using Marten;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

using Weasel.Postgresql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.AddScoped<IEventStore, MartenStore>();
builder.Services.AddScoped<Application, Application>(sp =>
{
    return new Application(sp.GetRequiredService<IEventStore>(), _ => false);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMarten(options =>
{
    var connectionString = "Host=localhost;Port=5432;Database=MartenRepro;Username=martenrepro;password=password";
    options.Connection(connectionString);
//    options.Events.StreamIdentity = Marten.Events.StreamIdentity.AsString;
    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate; // change to None when we stabilize db structure
    options.Projections.Add<TemplateFolderBuilder>(Marten.Events.Projections.ProjectionLifecycle.Inline);
});
var app = builder.Build();
app.MapCarter();

app.Run();



