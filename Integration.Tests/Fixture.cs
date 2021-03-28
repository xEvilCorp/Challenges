using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Vaquinha.Domain;
using Vaquinha.MVC;
using Xunit;

namespace Integration.Tests
{

    [CollectionDefinition(nameof(FixtureCollection))]
    public class FixtureCollection : ICollectionFixture<Fixture<StartupWebTests>> { }

    public class Fixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly AppFactory<TStartup> Factory;
        public HttpClient Client;
        public IConfigurationRoot Config;
        public GloballAppConfig AppConfig;

        public Fixture()
        {
            var clientOption = new WebApplicationFactoryClientOptions();

            Config = GetConfig();
            AppConfig = BuildAppConfig();
            Factory = new AppFactory<TStartup>();
            Client = Factory.CreateClient(clientOption);
        }

        public void Dispose()
        {
            Factory.Dispose();
            Client.Dispose();
        }

        private GloballAppConfig BuildAppConfig()
        {
            var globalAppSettings = new GloballAppConfig();
            Config.Bind("ConfiguracoesGeralAplicacao", globalAppSettings);
            return globalAppSettings;
        }

        private IConfigurationRoot GetConfig()
        {
            var dir = Directory.GetCurrentDirectory();
            return new ConfigurationBuilder().SetBasePath(dir).AddJsonFile("appsettings.json").AddJsonFile("appsettings.Testing.json").Build();
        }
    }
}
