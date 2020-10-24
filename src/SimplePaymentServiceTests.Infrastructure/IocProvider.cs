namespace SimplePaymentServiceTests.Infrastructure
{
    using SimplePaymentServiceTests.Services;
    using SimplePaymentServiceTests.Stores;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.IO;

    public sealed class IocProvider
    {
        public static IServiceProvider ServiceProvider => CreateServiceProvider();

        private static IServiceProvider CreateServiceProvider()
        {
            const string localSettingFileName = "appsettings.json";
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IPaymentService, PaymentService>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(localSettingFileName, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var dataStoreType = configuration["dataStoreType"];
            if (dataStoreType == "Backup") serviceCollection.AddSingleton<IDataStore, BackupAccountDataStore>();
            else serviceCollection.AddSingleton<IDataStore, AccountDataStore>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}