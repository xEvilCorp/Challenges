using FluentAssertions;
using System.Threading.Tasks;
using Vaquinha.Domain.Extensions;
using Vaquinha.MVC;
using Xunit;

namespace Integration.Tests
{
    [Collection(nameof(FixtureCollection))]
    public class HomeTests
    {
        private readonly Fixture<StartupWebTests> fix;
        public HomeTests (Fixture<StartupWebTests> fixture)
        {
            fix = fixture;
        }

        [Fact]
        [Trait("HomeController", "HomeController_InitialLoad")]
        public async Task HomeController_CheckInitialLoad()
        {
            var home = await fix.Client.GetAsync("Home");

            home.EnsureSuccessStatusCode();
            string homeContent = await home.Content.ReadAsStringAsync();

            string valueCollected = 0.ToDinheiroBrString();
            string campaignGoal = fix.AppConfig.MetaCampanha.ToDinheiroBrString();

            homeContent.Should().Contain(expected: "Arrecadamos quanto?");
            homeContent.Should().Contain(expected: valueCollected);
            homeContent.Should().Contain(expected: "Quanto falta arrecadar?");
            homeContent.Should().Contain(expected: campaignGoal);
        }
    }
}
