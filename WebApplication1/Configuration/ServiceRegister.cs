using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Context;
using WebApplication1.Logging;
using WebApplication1.Repository;
using WebApplication1.Services;

namespace WebApplication1.Configuration
{
    public static class ServiceRegister
    {
        //register services
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<DapperContext>();
            services.AddScoped<IRouletteRepository, RouletteRepository>();
            services.AddScoped<IRouletteService, RouletteService>();
            services.AddSingleton<IDatabase, Database>();
        }
    }
}
