﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace WebSockets_applications.SocketManager
{
    public static class SocketsExtension
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(SocketHandler))
                    services.AddSingleton(type);
            }

            return services;

        }

        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path,
            SocketHandler socket)
        {
            return app.Map(path, (x) => x.UseMiddleware<SocketMiddelware>(socket));
        }
    }
}
