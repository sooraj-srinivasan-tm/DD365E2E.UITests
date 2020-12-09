using System;
using System.Collections.Generic;
using System.Text;
using AutomationFoundationDotNetCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using static AutomationFoundationDotNetCore.SettingsBase;

namespace DotNetCoreSpecFlowTemplate.AppPages
{
    public class TradeSummaryPage
    {
        // here we will keep all the common functions, web elements, variables to be inherited to other pages
        // no constructor here
        // parameters username, password, dbString, url etc can be called from the AppConfig.json

        // DB string - depending on the DB in test we can add or modify existing one in App.config
        public static string dbString = GetJsonConfigurationValue("DatabaseConnectionString");
        public string Title = "Trade";
        // Common web elements should be defined here like headers, footers, buttons and links visible on all pages

        [FindsBy(How = How.Id, Using = "sampleId1")]
        public IWebElement CommonWebElement1 { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='SampleId2']")]
        public IWebElement CommonWebElement2 { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='cdk-virtual-scroll-content-wrapper']")]
        public IWebElement TradeSummaryGridRef { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='dd-inventory-filter-panel']//label[text()='Filter Trade']")]
        public IWebElement TradeFilerTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='vin']")]
        public IWebElement inputVin { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='table-header-cell cdk-drag cdk-drag-disabled ng-star-inserted']//div[@class='tm-icon tooltip ng-star-inserted']")]
        public IWebElement searchVehicleIcon { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='vin-cell ng-star-inserted']//div[@class='tm-grid-icon-label']")]
        public IWebElement SearchedVin { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='chip-panel--container']//div[@class='tm-html']")]
        public IWebElement SearchedResult { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(.,'Clear')]")]
        public IWebElement btnClear { get; set; }
        public TradeSummaryPage()
        {
            PageFactory.InitElements(Driver.GetDriver(), this);
        }

        // the links at the footer and header are visible and reachable from all pages, we will keep them here but we will call them from the page where we are during the test

        // we do not use above web element format because we need a dynamic way when we call each section link from the examples table
        public IWebElement SectionLink(string text)
        {
            var element = Driver.GetDriver().FindElement(By.LinkText(text));
            return element;
        }

        public Boolean GetTradeStatusForSearchedVin(string vin, string tradetype)
        {
            Boolean checkTradeStatus = false;
            Actions ac1 = new Actions(Driver.GetDriver());
            Actions ac2 = new Actions(Driver.GetDriver());
            try
            {
                IWebElement VinFromGrid = TradeSummaryGridRef.FindElement(By.CssSelector(".tm-grid-icon-label"));
               
                IWebElement TradeStatus = TradeSummaryGridRef.FindElement(By.XPath("//section/div/div/div[3]"));

                ac1.MoveToElement(VinFromGrid).Click().Build().Perform();
                Console.WriteLine("TRADE TYPE========== : " + TradeStatus.Text + " for VIN = " + VinFromGrid.Text);

                if (VinFromGrid.Text.Contains(vin) == true)
                {
                    ac1.MoveToElement(TradeStatus).Click().Build().Perform();
                    if (TradeStatus.Text.Contains(tradetype))
                        Console.WriteLine("Status : " + tradetype + " from Trade Summary Grid is found successfully. ");
                        checkTradeStatus = true;
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Unable to find elements from grid." + ex.StackTrace);
            }     

            return checkTradeStatus;

        }

        public Boolean GetCommentsForSearchedVin(string vin, string comments)
        {
            Boolean checkTradeComments = false;
            try
            {
                IWebElement VinFromGrid = TradeSummaryGridRef.FindElement(By.CssSelector(".tm-grid-icon-label"));

                IWebElement TradeComments = TradeSummaryGridRef.FindElement(By.XPath("//section/div/div/div[10]"));

                Console.WriteLine("TRADE TYPE========== : " + TradeComments.Text + " for VIN = " + VinFromGrid.Text);

                if (VinFromGrid.Text.Contains(vin) == true)
                {
                    if (TradeComments.Text.Contains(comments))
                        checkTradeComments = true;
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Unable to find elements from grid." + ex.StackTrace);
            }

            return checkTradeComments;

        }

        public Boolean SearchByVin(string vin)
        {
            Boolean IsSearchSuccessful = false;

            try
            {
                Actions action = new Actions(Driver.GetDriver());
                action.MoveToElement(inputVin).Click().Build().Perform();
               // inputVin.Click();
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
                inputVin.SendKeys(vin);
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);

                Actions action2 = new Actions(Driver.GetDriver());
                action2.MoveToElement(searchVehicleIcon).Click().Build().Perform();
                //searchVehicleIcon.Click();
                
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
                Console.WriteLine("Search Result--->" + SearchedResult.Text);

                if (Driver.GetDriver().Title == Title)
                {
                    if (SearchedResult.Text.Trim().Contains(vin))
                        IsSearchSuccessful = true;
                }

                Console.WriteLine("IsSearchSuccessful Status ==== " + IsSearchSuccessful);
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Unable to Search Vin from Trade Summary Page." + ex.StackTrace);
            }
            return IsSearchSuccessful;
        }

       

        public Boolean GetTradePanelLabel()
        {
            Boolean isTradePageUp = false;

            if(TradeFilerTitle.Displayed)
                 isTradePageUp = true;

            return isTradePageUp;

        }

    }
}
