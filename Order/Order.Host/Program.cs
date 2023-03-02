using Infrastructure.Filters;
using Microsoft.OpenApi.Models;
using Infrastructure.Extensions;
using Order.Host.Configurations;

namespace Order.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
              .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "eShop- Order HTTP API",
                    Version = "v1",
                    Description = "The Order Service HTTP API"
                });

                var authority = configuration["Authorization:Authority"];
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                            TokenUrl = new Uri($"{authority}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "order.orderbff", "order.orderbff" }
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            builder.AddConfiguration();
            builder.Services.Configure<OrderConfig>(configuration);

            builder.Services.AddAuthorization(configuration);

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IInternalHttpClientService, InternalHttpClientService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();

            builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));
            builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            var app = builder.Build();

            app.UseSwagger()
                .UseSwaggerUI(setup =>
                {
                    setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Order.API V1");
                    setup.OAuthClientId("orderswaggerui");
                    setup.OAuthAppName("Order Swagger UI");
                });

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            CreateDbIfNotExists(app);
            app.Run();

            IConfiguration GetConfiguration()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

                return builder.Build();
            }

            void CreateDbIfNotExists(IHost host)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();

                        DbInitializer.Initialize(context).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred creating the DB.");
                    }
                }
            }
        }
    }
}