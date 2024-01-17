using Microsoft.OpenApi.Models;
using Rainfall.Core;
using System.Reflection;
using File = System.IO.File;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0",
        Title = "Rainfall Api",
        Description = "An API which provides rainfall reading data"
    });

    options.AddServer(new OpenApiServer()
    {
        Url = "http://localhost:3000",
        Description = "Rainfall Api",
    });

    options.CustomOperationIds(e => $"{e.HttpMethod}-{e.ActionDescriptor.RouteValues["controller"]}".ToLower());

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        options.EnableAnnotations();
    }
});

builder.Services.AddHttpClient("rainfallClient", client =>
{
    client.BaseAddress = new Uri("http://environment.data.gov.uk");                     // TODO: move and take from appsettings
});

builder.Services.RegisterCoreServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.yaml", "v1");
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();
