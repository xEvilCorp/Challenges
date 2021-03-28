using Common.Tests;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Automated.Tests
{
    public class DonationTests : IDisposable, IClassFixture<DonationFixture>, IClassFixture<AddressFixture>,  IClassFixture<CreditCardFixture>
    {
        private IWebDriver driver;
        private DriverFactory driverFactory;

        private readonly CreditCardFixture credFix;
        private readonly DonationFixture donFix;
        private readonly AddressFixture addFix;

        public DonationTests(DonationFixture donationFix, AddressFixture addressFix, CreditCardFixture creditFix)
        {
            driverFactory = new DriverFactory();

            credFix = creditFix;
            donFix = donationFix;
            addFix = addressFix;
        }

        public void Dispose()
        {
            driverFactory.Close();
        }


        [Fact]
        [Trait("DonationUI", "DonationUI_HomeAccess")]
        public void DonationUI_CheckForHomeAccess()
        {
            driverFactory.NavigateToUrl("https://localhost:5001/");
            driver = driverFactory.GetWebDriver();

            var element = driver.FindElement(By.ClassName("vaquinha-logo"));

            element.Displayed.Should().BeTrue(because: "there is a cow logo in the homepage so... it should be there");
        }

    }
}
