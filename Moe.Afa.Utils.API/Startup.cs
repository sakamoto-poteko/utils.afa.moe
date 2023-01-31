using Azure.Data.Tables;
using Moe.Afa.Utils.API.Services;
using Moe.Afa.Utils.API.Settings;

namespace Moe.Afa.Utils.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpClient();

        services.AddTransient<ISteamService, SteamService>();
        services.AddTransient<IPlocnPhoneNumberLookupService, PlocnPhoneNumberLookupService>();
        
        services.AddSingleton<ISteamCacheManager, SteamCacheManager>();

        var tableStorageConnectionString = Configuration.GetConnectionString("TableStorage");
        var tableServiceClient = new TableServiceClient(tableStorageConnectionString);
        services.AddSingleton(tableServiceClient);

        services.Configure<SteamSettings>(Configuration.GetSection("Steam"));
        services.Configure<CidLookupSettings>(Configuration.GetSection("CidLookup"));
        services.Configure<PotekoOfficeStatusSettings>(Configuration.GetSection("PotekoOfficeStatusSettings"));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseSwagger();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}