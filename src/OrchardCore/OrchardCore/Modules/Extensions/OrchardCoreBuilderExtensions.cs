using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Descriptor;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Environment.Shell.Descriptor.Settings;
using OrchardCore.Modules;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class OrchardCoreBuilderExtensions
    {
        /// <summary>
        /// Registers at the host level a set of features which are always enabled for any tenant.
        /// </summary>
        public static OrchardCoreBuilder AddGlobalFeatures(this OrchardCoreBuilder builder, params string[] featureIds)
        {
            foreach (var featureId in featureIds)
            {
                builder.ApplicationServices.AddTransient(sp => new ShellFeature(featureId, alwaysEnabled: true));
            }

            return builder;
        }

        /// <summary>
        /// Registers at the tenant level a set of features which are always enabled.
        /// </summary>
        public static OrchardCoreBuilder AddTenantFeatures(this OrchardCoreBuilder builder, params string[] featureIds)
        {
            builder.ConfigureServices(services =>
            {
                foreach (var featureId in featureIds)
                {
                    services.AddTransient(sp => new ShellFeature(featureId, alwaysEnabled: true));
                }
            });

            return builder;
        }

        /// <summary>
        /// Registers a default tenant with a set of features that are used to setup and configure the actual tenants.
        /// For instance you can use this to add a custom Setup module.
        /// </summary>
        public static OrchardCoreBuilder AddSetupFeatures(this OrchardCoreBuilder builder, params string[] featureIds)
        {
            foreach (var featureId in featureIds)
            {
                builder.ApplicationServices.AddTransient(sp => new ShellFeature(featureId));
            }

            return builder;
        }

        /// <summary>
        /// Registers tenants defined in configuration.
        /// </summary>
        public static OrchardCoreBuilder WithTenants(this OrchardCoreBuilder builder)
        {
            var services = builder.ApplicationServices;

            services.AddScoped<IShellDescriptorManager, ConfiguredFeaturesShellDescriptorManager>();
            services.AddSingleton<IShellSettingsManager, ShellSettingsManager>();
            services.AddTransient<IConfigureOptions<ShellOptions>, ShellOptionsSetup>();

            return builder;
        }

        /// <summary>
        /// Registers a single tenant with the specified set of features.
        /// </summary>
        public static OrchardCoreBuilder WithFeatures(this OrchardCoreBuilder builder, params string[] featureIds)
        {
            foreach (var featureId in featureIds)
            {
                builder.ApplicationServices.AddTransient(sp => new ShellFeature(featureId));
            }

            builder.ApplicationServices.AddSetFeaturesDescriptor();

            return builder;
        }

        /// <summary>
        /// Registers and configures a background hosted service to manage tenant background tasks.
        /// </summary>
        public static OrchardCoreBuilder AddBackgroundService(this OrchardCoreBuilder builder)
        {
            builder.ApplicationServices.AddSingleton<IHostedService, ModularBackgroundService>();

            return builder;
        }
    }
}
