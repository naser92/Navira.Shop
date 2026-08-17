using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Navira.Shop.Core.Persistence.EF
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyReadConfigurations(
         this ModelBuilder modelBuilder,
         Assembly assembly)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
            assembly,
            type =>
                type is { IsClass: true, IsAbstract: false } &&
                typeof(IReadEntityConfiguration).IsAssignableFrom(type));
            //ApplyConfigurations<IReadEntityConfiguration>(
            //    modelBuilder,
            //    assembly);
        }

        public static void ApplyWriteConfigurations(
            this ModelBuilder modelBuilder,
            Assembly assembly)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
            assembly,
            type =>
                type is { IsClass: true, IsAbstract: false } &&
                typeof(IWriteEntityConfiguration).IsAssignableFrom(type));
            //ApplyConfigurations<IWriteEntityConfiguration>(
            //    modelBuilder,
            //    assembly);
        }

        private static void ApplyConfigurations<TMarker>(
            ModelBuilder modelBuilder,
            Assembly assembly)
        {
            var configurationTypes = assembly
                .GetTypes()
                .Where(type =>
                    type is { IsClass: true, IsAbstract: false } &&
                    typeof(TMarker).IsAssignableFrom(type))
                .ToList();

            foreach (var configurationType in configurationTypes)
            {

                var configurationInterface =
                    configurationType
                        .GetInterfaces()
                        .FirstOrDefault(x =>
                            x.IsGenericType &&
                            x.GetGenericTypeDefinition() ==
                            typeof(IEntityTypeConfiguration<>));

                if (configurationInterface == null)
                    continue;

                var entityType =
                    configurationInterface.GetGenericArguments()[0];

                var configuration =
                    Activator.CreateInstance(configurationType);

                var applyConfigurationMethod =
                    typeof(ModelBuilder)
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .First(x =>
                            x.Name == nameof(ModelBuilder.ApplyConfiguration) &&
                            x.IsGenericMethod &&
                            x.GetParameters().Length == 1);

                var genericMethod =
                    applyConfigurationMethod
                        .MakeGenericMethod(entityType);

                Console.WriteLine(
                    $"BEFORE INVOKE: {modelBuilder.Model.FindEntityType(entityType) != null}");

                Console.WriteLine(
                    $"METHOD: {genericMethod}");

                Console.WriteLine(
                    $"METHOD PARAMETER: {genericMethod.GetParameters()[0].ParameterType}");

                Console.WriteLine(
                    $"CONFIGURATION TYPE: {configuration.GetType()}");

                genericMethod.Invoke(
                    modelBuilder,
                    new[] { configuration });

                Console.WriteLine(
                   $"AFTER INVOKE: {modelBuilder.Model.FindEntityType(entityType) != null}");
            }
        }
    }
}
