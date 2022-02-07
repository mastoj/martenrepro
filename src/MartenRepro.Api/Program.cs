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

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCarter();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IEventStore, MartenStore>();
builder.Services.AddSingleton<ISomeService, SomeService>();
builder.Services.AddScoped<Application, Application>(sp =>
{
    return new Application(sp.GetRequiredService<IEventStore>(), _ => false);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization(opts =>
{
    opts.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddMarten(options =>
{
    var connectionString = "Host=localhost;Port=5432;Database=MartenRepro;Username=martenrepro;password=password";
    options.Connection(connectionString);

    // Use the morp permissive schema auto create behavior
    // while in development
    // if (app.Hosting.IsDevelopment())
    // {
    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate; // change to None when we stabilize db structure
    // }

    // Register projections
    options.Projections.Add<TemplateFolderBuilder>(Marten.Events.Projections.ProjectionLifecycle.Inline);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.AllowAnyOrigin();
        b.AllowAnyHeader();
        b.AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseForwardedHeaders();

// app.UseHttpsRedirection();

app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else {
    app.UseAuthorization();
}

app.MapControllers();
app.MapCarter();

app.Run();



