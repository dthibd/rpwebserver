using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Reflection;
using RPWebServer.AutoMapping;
using RPWebServer.Configuration;
using RPWebServer.Services;
using RPWebServer.Services.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

namespace RPWebServer;

[ExcludeFromCodeCoverage]
public class Startup
{
    public WebApplicationBuilder Builder { get; }
    public WebApplication? App { get; set; }
    
    public Startup(string[] args)
    {
        Builder = WebApplication.CreateBuilder(args);
    }

    public void Init()
    {
        SetupServices();
        CreateApp();
        UpdateComponentsMapping();
    }
    
    
    public void Run()
    {
        App?.Run();
    }


    private void SetupServices()
    {
        Builder.Services.AddTransient<IFileSystem, FileSystem>();
        Builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(RouteConfigProfile)));
        Builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
        Builder.Services.AddControllers();
        Builder.Services.AddEndpointsApiExplorer();
        Builder.Services.AddSwaggerGen();
        Builder.Services
            .AddSingleton<ICustomMemoryConfigFactory, CustomMemoryConfigFactory>()
            .AddSingleton<IComponentProviderFactory, ComponentProviderFactory>()
            .AddSingleton<IComponentsMappingService, ComponentsMappingService>()
            .AddSingleton<IReverseProxyChangesMonitor, ReverseProxyChangesMonitor>()
            .AddSingleton<IRoutesConfigProvider, RoutesConfigProvider>()
            .AddSingleton<IClustersConfigProvider, ClustersConfigProvider>()
            .AddSingleton<IProxyConfigProvider, CustomProxyConfigProvider>()
            .AddReverseProxy();

        Builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
        }));
        
        Builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("*");
                policy.WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            });
        });

        Builder.Services.AddOptions<WebComponentsServerOptions>()
            .Bind(Builder.Configuration.GetSection("WebComponentsServer"));

        Builder.Logging.AddConsole();
    }

    private void CreateApp()
    {
        App = Builder.Build();

        App.UseRouting();
        
        App.MapReverseProxy();

        // Configure the HTTP request pipeline.
        if (App.Environment.IsDevelopment())
        {
            App.UseSwagger();
            App.UseSwaggerUI();
        }

        App.UseHttpsRedirection();

        App.UseAuthorization();
        App.UseCors();

        App.MapControllers();
    }

    private void UpdateComponentsMapping()
    {
        var componentsMappingService = App?.Services.GetService<IComponentsMappingService>();
        componentsMappingService?.UpdateMapping();
    }

}