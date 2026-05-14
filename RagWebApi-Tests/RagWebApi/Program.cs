using DotNetEnv;
using Jina;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using RagWebApi.DataContext;
using RagWebApi.Service;
using RagWebApi.Service.Documents;
using RagWebApi.Service.Images;
using RagWebApi.Service.Movies;
using System.Net.Http.Headers;


var builder = WebApplication.CreateBuilder(args);

Env.Load();
// Add environment variables into configuration
builder.Configuration.AddEnvironmentVariables();

// Disable HTTPS in container/non-development environments
if (builder.Environment.IsProduction() || Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(6100);
    });
}

// Add services to the container.
var services = builder.Services;
var config = builder.Configuration;

var dbServer = config["DBSERVER"];
var dbName = config["DBNAME"];
var dbUser = config["DBUSER"];
var dbPassword = config["DBPASSWORD"];

var connectionString = builder.Environment.IsDevelopment()?
    $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;Encrypt=False" :
    config.GetConnectionString("DefaultConnection");

services.AddDbContext<RagContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableServiceProviderCaching();
});

Console.WriteLine(connectionString);

services.AddControllers();

string jinaKey = config["JINA_KEY"]!;
string openRouterApiKey = config["OPEN_ROUTER_KEY"]!;
string cloudFlareKey = config["CLOUD_FLARE_KEY"]!;


Console.WriteLine(jinaKey);
Console.WriteLine(openRouterApiKey);
Console.WriteLine(cloudFlareKey);


services.AddKeyedSingleton<JinaClient>(serviceKey: null, (serviceProvider, _) =>
    new JinaClient(jinaKey));
var endpoint = "https://openrouter.ai/api/v1/chat/completions";
var imageEndpoint = "https://api.cloudflare.com/client/v4/accounts/6a688576791cd5f8a451879339f16545/ai/run/";

services.AddHttpClient("ChatClient", client =>
{
    client.BaseAddress = new Uri(endpoint);
    client.Timeout = TimeSpan.FromSeconds(100);
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", openRouterApiKey.Trim());

});

services.AddHttpClient("ImageClient", client =>
{
    client.BaseAddress = new Uri(imageEndpoint);
    client.Timeout = TimeSpan.FromSeconds(100);
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", cloudFlareKey.Trim());

});

services.AddTransient<IChatService>(sp =>
{
    var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ChatClient");

    var imageClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ImageClient");
    return new ChatService(client, imageClient);
});



services.AddSingleton<WorkflowNotifier>();
services.AddHostedService<ImageAnalysisBackgroundService>();
services.AddHostedService<ImageEmbeddingsBackgroundService>();
services.AddHostedService<MovieSearchBackgroundService>();
services.AddHostedService<MovieEmbeddingsBackgroundService>();
services.AddHostedService<DocumentEmbeddingsBackgroundService>();


services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .WithOrigins("http://localhost:3100", "http://localhost:3000", "http://localhost:5173", "http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));
services.AddScoped<IJinaClientWrapper, JinaClientWrapper>();
services.AddScoped<IDocumentAiService, DocumentAiService>();
services.AddScoped<IDocumentService, DocumentService>();
services.AddScoped<IImageAiService, ImageAiService>();
services.AddScoped<IImageService, ImageService>();

services.AddScoped<IMovieService, MovieService>();
services.AddScoped<IMovieAiService, MovieAiService>();

services.AddHealthChecks();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RagContext>();
    db.Database.Migrate();
}


if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();

}



app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

var url = app.Environment.IsDevelopment() ? "https://localhost:4100" : "http://localhost:6100";


app.Run(url);
