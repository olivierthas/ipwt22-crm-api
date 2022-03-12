namespace Crm.Link.Api
{
    public static class ConfigureCORS
    {
        public static string PolicyName { get; set; } = "";
        public static IServiceCollection AddCrmCors(this IServiceCollection service, string corsPolicyName)
        {
            PolicyName = corsPolicyName;
            service.AddCors(options =>
            {
                options.AddPolicy(name: PolicyName,
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });

            return service;
        }
    }
}
