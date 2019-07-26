using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibMediator;
using LibMediator.Command;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MediatorServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, Type type)
        {
            services.AddScoped<IMediator, Mediator>();

            Assembly assembly = type.Assembly;
            RegisterCommandHandlers(services, assembly);

            return services;
        }

        private static void RegisterCommandHandlers(IServiceCollection services, Assembly assembly)
        {
            Type[] assemblyTypes = assembly.GetExportedTypes();
            IEnumerable<Type> commandTypes = assemblyTypes.Where(t => t.IsClass && typeof(ICommand).IsAssignableFrom(t));
            IEnumerable<Type> commandHandlerTypes = commandTypes.Select(t => typeof(ICommandHandler<>).MakeGenericType(t));

            foreach (Type handlerType in commandHandlerTypes)
            {
                Type handlerImplemention = assemblyTypes
                    .FirstOrDefault(t => t.IsClass && handlerType.IsAssignableFrom(t));
                if (handlerImplemention != null)
                {
                    services.AddTransient(handlerType, handlerImplemention);
                }
            }
        }
    }
}
