using Serilog;
using Serilog.Extensions.Hosting;

namespace Crm.Link.Api
{
    public static class ConfigureLogging
    {
        public static IHostBuilder AddCrmLogging(this IHostBuilder builder)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            builder.UseSerilog((ctx, lc) => lc
                   .WriteTo.Console()
                   .WriteTo.File($"{basePath}crm_logs/crm.txt")
                   .ReadFrom.Configuration(ctx.Configuration));
            return builder;
        }

        public static IApplicationBuilder UseCrmLoggng(this IApplicationBuilder builder)
        {
            builder.UseSerilogRequestLogging();
            builder.UseHttpLogging();
            return builder;
        }

        public static ReloadableLogger Bootstrap()
            => new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
    }
}
