using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AutomationFoundationDotNetCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using static AutomationFoundationDotNetCore.SettingsBase;

namespace DotNetCoreSpecFlowTemplate.AppPages
{
    public class InventoryPage
    {
        // here we will keep all the common functions, web elements, variables to be inherited to other pages
        // no constructor here
        // parameters username, password, dbString, url etc can be called from the AppConfig.json

        // DB string - depending on the DB in test we can add or modify existing one in App.config
        public static string dbString = GetJsonConfigurationValue("DatabaseConnectionString");
        public string Title = "Inventory";
        // Common web elements should be defined here like headers, footers, buttons and links visible on all pages
        public readonly AllPages Pages = new AllPages();

        [FindsBy(How = How.Id, Using = "sampleId1")]
        public IWebElement CommonWebElement1 { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='SampleId2']")]
        public IWebElement CommonWebElement2 { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='tm-slide-panel-center']//section[@class='table table-section']//form[@class='ng-untouched ng-pristine ng-valid']")]
        public IWebElement inputVinRef { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='vin']")]
        public IWebElement inputVin { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='table-filter-item blank-header search-icon']/div[1]/i[@class='toyota-icon search']")]
        public IWebElement searchVehicleIcon { get; set; }


        [FindsBy(How = How.XPath, Using = "//div[@class='cdk-virtual-scroll-content-wrapper']")]
        public IWebElement inventoryGridRef { get; set; }


        [FindsBy(How = How.XPath, Using = "//div[@class='dd-no-data-container']//div[text()='No match found']")]
        public IWebElement NoVinFoundMsg { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='chip-panel-item clear tm-button']")]
        public IWebElement BtnClear { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='action__menu-list-item ng-star-inserted']//span[@class='tmi action__menu-list-item-icon tmi-trade']")]
        public IWebElement TradeLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".action__menu-list-item:nth-child(3) > .action__menu-list-item-label")]
        public IWebElement RDRLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='dd-inventory-tradeform']//div[@class='text-lg  red']")]
        public IWebElement TradePopupTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='chip-panel--container']//div[@class='tm-html']")]
        public IWebElement SearchedResult { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='text-sm']")]
        public IWebElement TradeWindowVin { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='tm-snackbar__message']")]
        public IWebElement popupMsg { get; set; }

        [FindsBy(How = How.XPath, Using = "//div/div[2]/tm-checkbox/div/ul/li/i")]
        public IWebElement gridCKBox { get; set; }
       
        [FindsBy(How = How.XPath, Using = "//div[10]//div[@class='//div[10]//div[@class='tm-grid-icon-label']']")]
        public IWebElement gridVin { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@class='action__btn btn-primary']']")]
        public IWebElement actionBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='dd-grid-toolbar']/ul//li//span[@class='customview-name']']")]
        public IWebElement customSelection { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='dd-inventory-filter-panel-header-label']/label[text()='Filter Inventory']']")]
        public IWebElement inventoryFilerLabel { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='cdk-overlay-pane']")]
        public IWebElement TradeNotAllowedAlert { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='tm-button']//button[@class='btn-primary trade-dialog__btn']")] // Got it 
        public IWebElement BtnGotIt { get; set; }



        WebDriverWait Wait = new WebDriverWait(Driver.GetDriver(), TimeSpan.FromSeconds(100));


        
        public InventoryPage()
        {
            PageFactory.InitElements(Driver.GetDriver(), this);
        }

        public Boolean selectTradeLink(string expectedVin)
        {

            Boolean isSuccessful = false;
            try
            {
                //Click(By.XPath("//div[10]//div[@class='tm-grid-icon-label']"));
                Actions action = new Actions(Driver.GetDriver());

                IWebElement shortCutMenulink = Driver.GetDriver().FindElement(By.XPath("//div[@class='tm-icon actions-cell ng-star-inserted']/i[@class='toyota-icon more-vert']"));
                action.MoveToElement(shortCutMenulink).Click().Build().Perform();
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                if (TradeLink.Displayed)
                {
                    Console.WriteLine("Trade Link is found");
                    TradeLink.Click();
                    Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    Console.WriteLine("Trade Popup Title======" + TradePopupTitle.Text);
                    if ((TradePopupTitle.Text.Trim() == "Trade vehicles") && (TradeWindowVin.Text.Contains(expectedVin)))
                    {
                        isSuccessful = true;
                        Console.WriteLine("Trade Link was found and clicked fine.");
                    }
                }
                
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return isSuccessful;
        }
           
        public Boolean SearchByVin(string vin)
        {
            Boolean IsSearchSuccessful = false;
           
            try
            {
                inputVin.Click();
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
                inputVin.SendKeys(vin);
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(2);
                searchVehicleIcon.Click();
                // inputVin.SendKeys("" + Keys.Enter);

                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                Console.WriteLine("Search Result--->"+ SearchedResult.Text);

                if (Driver.GetDriver().Title == Title) { 
                    if (SearchedResult.Text.Trim().Contains(vin))
                        IsSearchSuccessful = true;
                }
                
                Console.WriteLine("IsSearchSuccessful Status ==== " + IsSearchSuccessful);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to Search Vin from Inventory Page."+e.StackTrace);
            }
            return IsSearchSuccessful;
        }

        [Obsolete]
        public string GetTradeAlertMsg()
        {
            string StockTrade_Msg = null;

            Console.WriteLine("Current Page Title ====== " + Driver.GetDriver().Title);
            Boolean isPresent = TradeNotAllowedAlert.Size.Width > 0;
            Console.WriteLine("Size of Alert Element====" + TradeNotAllowedAlert.Size.Width + " Prsent   "+isPresent);
            if (isPresent == true)
            {
                StockTrade_Msg = TradeNotAllowedAlert.Text;
                Console.WriteLine("Stock Trade Message : " + StockTrade_Msg);
                BtnGotIt.Click();
                Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(35);
            }
            else
                Console.WriteLine("No Alert was found.");
            
            return StockTrade_Msg;

        }

        public void ClearFilter()
        {
            if (BtnClear.Displayed == true)
            {
                Actions action = new Actions(Driver.GetDriver());
                action.Click(BtnClear).Perform();
                //BtnClear.Click();
                Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                if (BtnClear.Displayed == false)
                    Console.WriteLine("Clear button is not displayed.");
            }
           

        }

    }
}
