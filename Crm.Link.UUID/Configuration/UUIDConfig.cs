using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Link.UUID.Configuration
{
    public static class UUIDConfig
    {
        public static IServiceCollection UseUUID(this IServiceCollection service)
        {
            service.AddTransient<IUUIDGateAway, UUIDGateAway>();

            return service;
        }
    }
}
