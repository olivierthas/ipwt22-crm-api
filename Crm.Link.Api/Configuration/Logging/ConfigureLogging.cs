using Serilog;
using Serilog.Extensions.Hosting;

namespace Crm.Link.Api
{
    public static class ConfigureLogging
    {
        public static IHostBuilder AddCrmLogging(this IHostBuilder builder)
        {
            builder.UseSerilog((ctx, lc) => lc
                   .WriteTo.Console()
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
