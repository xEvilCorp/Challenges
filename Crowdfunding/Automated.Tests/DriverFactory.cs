using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;

namespace Automated.Tests
{
    public class DriverFactory
    {
        private IWebDriver driver;

        public DriverFactory()
        {
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService("/usr/share/applications/");
            service.Port = new Random().Next(64000, 64800);
            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("-headless");
            options.AddArgument("-safe-mode");
            options.AddArgument("-ignore-certificate-errors");
            FirefoxProfile profile = new FirefoxProfile();
            profile.AcceptUntrustedCertificates = true;
            profile.AssumeUntrustedCertificateIssuer = false;
            options.Profile = profile;

            driver = new FirefoxDriver(service, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();
        }

        public void NavigateToUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public void Close()
        {
            driver.Quit();
        }

        public IWebDriver GetWebDriver()
        {
            return driver;
        }
    }
}
