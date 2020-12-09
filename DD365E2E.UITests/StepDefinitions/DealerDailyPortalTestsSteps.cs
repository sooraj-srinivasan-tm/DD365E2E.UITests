using System;
using System.Linq;
using System.Threading;
using AutomationFoundationDotNetCore;
using DotNetCoreSpecFlowTemplate.AppPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using static AutomationFoundationDotNetCore.SettingsBase;

namespace DotNetCoreSpecFlowTemplate.StepDefinitions
{
    // for Context Injection, we create a class and store data at any step and use in another step in the same or another binding class in the same scenario
    // this data can be defined or changed at any step and used in another step
    public class SharedData
    {
        public string data1;
        public int data2;
    }


    [Binding]
    public class DealerDailyPortalTestsSteps
    {
        // we create a constructor with shared data object
        private readonly SharedData _sharedData;
        WebDriverWait Wait = new WebDriverWait(Driver.GetDriver(), TimeSpan.FromSeconds(10));

        public DealerDailyPortalTestsSteps(SharedData sharedData)
        {
            this._sharedData = sharedData;
        }
        public readonly AllPages Pages = new AllPages();

        [Given(@"I am at the DD365 login page")]
        public void GivenIAmAtTheDD365LoginPage()
        {
            Assert.AreEqual("Gulf States Toyota DEV - Sign In", Driver.GetDriver().Title, "page title is different");
        }

        [When(@"I enter username and password to login")]
        public void WhenIEnterUsernameAndPasswordToLogin()
        {
            var userName = GetJsonConfigurationValue("UserName");
            var password = GetJsonConfigurationValue("Password");
            Pages.LoginPage().UsernameInput.SendKeys(userName);
            Pages.LoginPage().NextButton.Click();
            Pages.LoginPage().PasswordInput.SendKeys(password);
            Pages.LoginPage().SigninButton.Click();
        }

        [When(@"I click on Inventory Summary tab")]
        public void WhenIClickOnInventorySummaryTab()
        {
            Pages.HomePage().InventoryTab.Click();
            //Assert.AreEqual(Pages.InventoryPage().Title, Driver.GetDriver().Title, "Page title is different than " + Pages.InventoryPage().Title);
        }

        [When(@"I click on Vehicle Locator tab")]
        public void WhenIClickOnVehicleLocatorTab()
        {
            Pages.HomePage().VehicleLocatorTab.Click();
            //Assert.AreEqual(Pages.LocatorPage().Title, Driver.GetDriver().Title, "Page title is different than " + Pages.LocatorPage().Title);
        }

        [When(@"I click on RDR Summary tab")]
        public void WhenIClickOnRDRSummaryTab()
        {
            Pages.HomePage().RDRSummaryTab.Click();
            //Assert.AreEqual(Pages.RDRPage().Title, Driver.GetDriver().Title, "Page title is different than " + Pages.RDRPage().Title);
        }

        [When(@"I click on Sales Summary tab")]
        public void WhenIClickOnSalesSummaryTab()
        {
            Pages.HomePage().SalesSummaryTab.Click();
            //Assert.AreEqual(Pages.SalesPage().Title, Driver.GetDriver().Title, "Page title is different than " + Pages.SalesPage().Title);
        }

        [When(@"I click on Trade Summary tab")]
        public void WhenIClickOnTradeSummaryTab()
        {
            Pages.HomePage().TradeSummaryTab.Click();
            //Assert.AreEqual(Pages.TradePage().Title, Driver.GetDriver().Title, "Page title is different than " + Pages.TradePage().Title);
        }

        [Then(@"I should land to home page")]
        public void ThenIShouldLandToHomePage()
        {
            var retry = true;

            while (retry)
            {
                Driver.GetDriver().FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
                Driver.GetDriver().SwitchTo().Window(Driver.GetDriver().WindowHandles.Last());
                Driver.GetDriver().Url = GetJsonConfigurationValue("DefaultUrl");
                //Driver.GetDriver().Navigate().GoToUrl("Default");

                var userName = GetJsonConfigurationValue("UserName");
                var password = GetJsonConfigurationValue("Password");
                Pages.LoginPage().UsernameInput.SendKeys(userName);
                Pages.LoginPage().NextButton.Click();
                Pages.LoginPage().PasswordInput.SendKeys(password);
                Pages.LoginPage().SigninButton.Click();

                //Wait for title to update
                Wait.Until(x => x.Title != Pages.LoginPage().Title);

                if (Driver.GetDriver().Title == "Toyota Enterprise Security Service - System Error")
                {
                    retry = true;
                } else
                {
                    retry = false;
                }
            }
            Assert.AreEqual("Dealer Daily", Driver.GetDriver().Title, "page title is different than " + "Dealer Daily");
            Wait.Until(x => (x.Title != Pages.BasePage().Title));
        }

        [Then(@"I should see the Inventory header")]
        public void ThenIShouldSeeInventoryHeader()
        {
            Assert.AreEqual(Pages.InventoryPage().Title, Driver.GetDriver().Title, "inventory header is different");
        }

        [Then(@"I should see the Locator header")]
        public void ThenIShouldSeeLocatorHeader()
        {
            Assert.AreEqual(Pages.LocatorPage().Title, Driver.GetDriver().Title, "locator header is different");
        }

        [Then(@"I should see the RDR header")]
        public void ThenIShouldSeeRDRHeader()
        {
            Assert.AreEqual(Pages.RDRPage().Title, Driver.GetDriver().Title, "rdr header is different");
        }

        [Then(@"I should see the Sales header")]
        public void ThenIShouldSeeSalesHeader()
        {
            Assert.AreEqual(Pages.SalesPage().Title, Driver.GetDriver().Title, "sales header is different");
        }

        [Then(@"I should see the Trade header")]
        public void ThenIShouldSeeTradeHeader()
        {
            Wait.Until(x => (x.Title != Pages.TradeSummaryPage().Title));
            Assert.AreEqual(Pages.TradeSummaryPage().Title, Driver.GetDriver().Title, "trade header is different");
        }

    }
}
