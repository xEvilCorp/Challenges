using Common.Tests;
using FluentAssertions;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Vaquinha.Domain.Extensions;
using Vaquinha.Domain.ViewModels;
using Vaquinha.MVC;
using Xunit;


namespace Integration.Tests
{
    [Collection(nameof(FixtureCollection))]
    public class DonationTests
    {
        private readonly Fixture<StartupWebTests> fix;

        public DonationTests(Fixture<StartupWebTests> fixture)
        {
            fix = fixture;
        }

        [Fact]
        [Trait("DonationController", "DonationController_InitialLoad")]
        public async Task DonationController_CheckInitialLoad()
        {
            var donationReq = await fix.Client.GetAsync("Doacoes/Create");

            donationReq.EnsureSuccessStatusCode();
            string donationContent = await donationReq.Content.ReadAsStringAsync();

            donationContent.Should().Contain(expected: "Doe agora");
            donationContent.Should().Contain(expected: "Dados Pessoais");
        }


        [Fact]
        [Trait("DonationController", "DonationController_AddDonation")]
        public async Task DonationController_CheckMakingADonation()
        {
            DoacaoViewModel donationVM = new DonationFixture().ValidVM();
            var donationReq = await fix.Client.PostAsync("Doacoes/Create", new FormUrlEncodedContent(ToKeyValuePairs(donationVM)));
            donationReq.EnsureSuccessStatusCode();

            string donationContent = await donationReq.Content.ReadAsStringAsync();

            donationContent.Should().Contain(expected: "Vaquinha online");
            donationContent.Should().Contain(expected: "nesse instante");
            donationContent.Should().Contain(expected: donationVM.DadosPessoais.Nome);
        }


        public  IDictionary<string, string> ToKeyValuePairs(object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            JToken token = metaToken as JToken;
            if (token == null)
            {
                return ToKeyValuePairs(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = ToKeyValuePairs(child);
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                            .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                jValue?.ToString("o", CultureInfo.InvariantCulture) :
                jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }
    }
}
