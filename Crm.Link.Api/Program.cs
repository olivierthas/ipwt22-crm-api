using Crm.Link.Api;
using Crm.Link.Api.Configuration.Httpclient;
using Crm.Link.RabbitMq.Configuration;
using Crm.Link.Suitcrm.Tools;
using Crm.Link.UUID.Configuration;
using Newtonsoft.Json.Converters;
using Serilog;

    Log.Logger = ConfigureLogging.Bootstrap();
    Log.Information("Starting up");

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    builder.Host.AddCrmLogging();

    // builder.Services.AddCrmAuthentication(configuration);
    // builder.Services.AddCrmCors("CorsPolicyName");
    builder.Services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    });
    // builder.Services.AddHttpClientFactory(configuration);
    builder.Services.UseCrmTools(configuration);
    builder.Services.UseUUID();
    builder.Services.AddOpenApi();

    // configuration rabbitmq
    builder.Services.StartConsumers(configuration.GetConnectionString("RabbitMq"));
    builder.Services.AddPublisher();

    var app = builder.Build();

    app.UseCrmLoggng();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || true)
    {
        Log.Information("OpenApi active");
        app.UseOpenApi();
    }

    // app.UseHttpsRedirection();

    app.UseRouting();
    // app.UseCors(ConfigureCORS.PolicyName);

    // app.UseAuthentication();
    // app.UseAuthorization();

    app.MapControllers();

    app.Run();

