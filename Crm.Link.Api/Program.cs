using Crm.Link.Api;
using Crm.Link.RabbitMq.Consumer;
using Newtonsoft.Json.Converters;
using Serilog;

Log.Logger = ConfigureLogging.Bootstrap();
Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    
    builder.Host.AddCrmLogging();

    // builder.Services.AddCrmAuthentication(configuration);
    builder.Services.AddCrmCors("CorsPolicyName");
    builder.Services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });
    builder.Services.AddOpenApi();
    builder.Services.AddHostedService<LogConsumer>();

    var app = builder.Build();

    app.UseCrmLoggng();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseCors(ConfigureCORS.PolicyName);

    app.UseAuthentication();
    // app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
    throw;
}
finally
{
    Log.Information("Shut down complete");
    Console.ReadKey();
    Log.CloseAndFlush();
}
