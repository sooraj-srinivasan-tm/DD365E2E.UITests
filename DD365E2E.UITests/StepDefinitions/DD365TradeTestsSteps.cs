using System;
using System.Linq;
using System.Threading;
using AutomationFoundationDotNetCore;
using DD365E2E.UITests.AppPages;
using DotNetCoreSpecFlowTemplate.AppPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using static AutomationFoundationDotNetCore.SettingsBase;


namespace DD365E2E.UITests.StepDefinitions
{
    public class SharedData
    {
        public string data1;
        public int data2;
    }

    [Binding]
    public class DD365TradeTestsSteps
    {
        Boolean isSearchVin = false;
        Boolean isTradWindowAppeared = false;
        String altMsg = null;

        ExpectedObjects exp_obj = new ExpectedObjects();

        // we create a constructor with shared data object
        private readonly SharedData _sharedData;
        public readonly AllPages Pages = new AllPages();
        CommanMethods comman = new CommanMethods();

        WebDriverWait Wait = new WebDriverWait(Driver.GetDriver(), TimeSpan.FromSeconds(300));
        

        public DD365TradeTestsSteps(SharedData sharedData)
        {
            this._sharedData = sharedData;

        }

        [Given(@"I am at the DD365 login page for Trade")]
        public void GivenIAmAtTheDD365LoginPage()
        {
            Driver.GetDriver().Url = GetJsonConfigurationValue("DefaultUrl");
            //Driver.GetDriver().Manage().Cookies.DeleteAllCookies();
            Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
            Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(35);
            if (!Driver.GetDriver().Title.Contains("Gulf States Toyota DEV - Sign In"))
            {
                for (int i = 0; i < 2; i++)
                {
                    Driver.CloseDriver();
                    Driver.GetDriver().Url = GetJsonConfigurationValue("DefaultUrl");

                    Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
                    Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(35);

                    if (Driver.GetDriver().Title.Contains("Gulf States Toyota DEV - Sign In"))
                        break;
                }

            }
            
            Assert.AreEqual("Gulf States Toyota DEV - Sign In", Driver.GetDriver().Title, "page title is different");
        }

        [When(@"I enter username and password to login for Trade")]
        public void WhenIEnterUsernameAndPasswordToLogin()
        {

            var userName = GetJsonConfigurationValue("UserName");
            var password = GetJsonConfigurationValue("Password");

            Pages.LoginPage().LogInToDD365(userName, password);
          

        }

       
        [Then(@"I should land to home page for Trade")]
        public void ThenIShouldLandToHomePage()
        {
            var retry = true;
            var userName = "";
            userName = GetJsonConfigurationValue("UserName");
            var password = "";
            password = GetJsonConfigurationValue("Password");

            while (retry)
            {

                //Wait for title to update

                Wait.Until(x => (x.Title != Pages.LoginPage().Title));

                if (Driver.GetDriver().Title == "Toyota Enterprise Security Service - System Error")
                {
                    Console.WriteLine("----------System Error occured----------");
                    Driver.GetDriver().FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
                    Driver.GetDriver().SwitchTo().Window(Driver.GetDriver().WindowHandles.Last());
                    //Driver.GetDriver().Close();
                    Driver.GetDriver().Url = GetJsonConfigurationValue("DefaultUrl");

                    Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
                    Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);

                    //Driver.GetDriver().Navigate().GoToUrl("Default");
                    Wait.Until(x => (x.Title != Pages.LoginPage().Title));

                    if (Driver.GetDriver().Title == "Gulf States Toyota DEV - Sign In")
                        Pages.LoginPage().LogInToDD365(userName, password);

                    if (Driver.GetDriver().Title == "Toyota Enterprise Security Service - System Error")
                        retry = true;
                }
                else
                {
                    retry = false;
                }
            }
            Console.WriteLine("Current Page Title ====" + Driver.GetDriver().Title);
            Assert.AreEqual("Dealer Daily", Driver.GetDriver().Title, "page title is different than " + "Dealer Daily");
            Wait.Until(x => (x.Title != Pages.BasePage().Title));

        }

        [When(@"I click on Inventory Summary tab for Trade")]
        public void WhenIClickOnInventorySummaryTab()
        {
            Console.WriteLine("Current Page Title ====" + Driver.GetDriver().Title);
           
            if (!Driver.GetDriver().Title.Contains(Pages.InventoryPage().Title)) {
                Pages.HomePage().InventoryTab.Click();
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
                Console.WriteLine("Page Title when Inventorty Summary tab is clicked:--->" + Driver.GetDriver().Title);
                Wait.Until(x => (x.Title != Pages.InventoryPage().Title));
                //Assert.AreEqual(Pages.InventoryPage().Title, Driver.GetDriver().Title, "Page title is different than " + Pages.InventoryPage().Title);
            }
        }

        [Then(@"I should see the Inventory header for Trade")]
        public void ThenIShouldSeeInventoryHeader()
        {
           
            Assert.AreEqual(Pages.InventoryPage().Title, Driver.GetDriver().Title, "inventory header is different");
            //Wait.Until(x => (x.Title != Pages.InventoryPage().Title));
           
        }

        [When(@"I search for a Category: ""(.*)"" Vin : ""(.*)"" to trade")]
        public void WhenISearchForACategoryVinToTrade(string catg, string vin)
        {
            Console.WriteLine("Expected Vin = " + exp_obj.vin);
            if (exp_obj.vin != "")
            {
               Pages.InventoryPage().ClearFilter();
               Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            }
           
            Console.WriteLine("*****Going to Search for Vin from Inventory Summary Page*****");

            Driver.GetDriver().FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
            Driver.GetDriver().SwitchTo().Window(Driver.GetDriver().WindowHandles.Last());
            
            isSearchVin = Pages.InventoryPage().SearchByVin(vin);
            Console.WriteLine("Search Vin Status ===== : " + isSearchVin);
            if (isSearchVin == true)
            {
                exp_obj.vin = vin;
               
            }
            else if(Pages.InventoryPage().NoVinFoundMsg.Displayed == true)
            {
                Console.WriteLine("No Vin Found message was appeared.");
                isSearchVin = false;
            }
            Assert.IsTrue(isSearchVin, "Vin Search Status");
        }

        [Then(@"I should see searched vehicle on the Inventory page")]
        public void ThenIShouldSeeSearchedVehicle()
        {            
            Assert.IsTrue(isSearchVin, "Searched Vehicle from Inventory page");
            
        }

        [When(@"I select the Trade link based on Expected Trade ""(.*)""")]
        public void WhenISelectTheTradeLinkBasedOnExpectedTrade(string TradeNeed)
        {
            if ((isSearchVin == true) && (TradeNeed.Contains("Yes")) )
            {
                //Wait.Until(x => (x.Title != ""));
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);

                Console.WriteLine("Going to click trade from window---->" + Driver.GetDriver().Title);
                if (Driver.GetDriver().Title.Contains(Pages.InventoryPage().Title))
                {
                    if (Pages.InventoryPage().inventoryGridRef.Displayed)
                    {
                        Pages.InventoryPage().inventoryGridRef.Click();
                        isTradWindowAppeared = comman.selectActionsOption("Trade", exp_obj.vin);
                    }
                    else
                    {
                        Boolean goBack = Pages.VehicleDetailPage().IsBackToButtonClicked();
                        if (goBack == true)
                            Console.WriteLine("Successfully clicked on Go back To Inventory link.");

                        Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
                    }
                }
                else
                    Console.WriteLine("Current window is not Inventory window to Trade from for this vin : " + exp_obj.vin);
                
                Console.WriteLine("Trade window popup happened Status....." + isTradWindowAppeared);
            }
        }

        [Then(@"I should be able to see Trade Vehicle popup window based on Expected Trade ""(.*)""")]
        public void ThenIShouldBeAbleToSeeTradeVehiclePopupWindowBasedOnExpectedTrade(string TradeNeed)
        {
            Console.WriteLine("Search VIN : " + isSearchVin);
            Console.WriteLine("Trade Window Appeared : " + isTradWindowAppeared);
            Console.WriteLine("Trade Needed : " + TradeNeed);

            if (TradeNeed.Contains("Yes") ){
                if (isSearchVin == true) {
                    if (isTradWindowAppeared == true)
                        Assert.IsTrue(isTradWindowAppeared, "Trade Popup window is opened successfully.");
                    else
                        Assert.IsFalse(isTradWindowAppeared, "Trade Popup window didn't appear.");
                }
                else
                    Assert.Fail("Trade Popup Window didn't appear due to search Vin Failed.");
            }
        }

        [When(@"I enter trade to dealer ""(.*)"" and note ""(.*)"" and Click Trade")]
        public void WhenIEnterTradeToDealerAndNoteAndClickTrade(string tradeToDealer, string tradeNote)
        {
            if ((isSearchVin == true) && (isTradWindowAppeared == true))
            {
               Pages.VehicleDetailPage().MakeTrade(tradeToDealer, tradeNote);             
                exp_obj.tradeToDealer = tradeToDealer;
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);

            }
            
        }

        [Then(@"I should see ""(.*)"" message for Category : ""(.*)"" vehicle")]

        public void ThenIShouldSeeSuccessfulTradeMessage(string TradeMsg, string catg)
        {
            
           

            if ((isSearchVin == true) && (isTradWindowAppeared == true))
            {
                Console.WriteLine("Alert message ====== : " + altMsg);
               
                Boolean backInventoryLnk = Pages.VehicleDetailPage().IsBackToButtonClicked();
                if (backInventoryLnk == true)
                {
                    Console.WriteLine("Successfully Traded vehicle and now back to Inventory Page.");
                    if (Pages.InventoryPage().NoVinFoundMsg.Displayed)
                    {
                       // Pages.InventoryPage().ClearFilter();
                        Assert.IsTrue(backInventoryLnk, "Trade was done successfully as Expected");
                    }
                }
                else
                {
                    altMsg = Pages.InventoryPage().GetTradeAlertMsg();
                    Console.WriteLine("Alert message found as : " + altMsg);
                    if (altMsg != "")
                        Assert.Fail("Trade is not done successfully as Expected.");
                }
          
            }
            else
                Assert.Fail("Trade was not done successfully for this vin due to not landing at inventory page for the after trade was per: " + exp_obj.vin);
            
        }
        [Then(@"I should see Trade is not possible ""(.*)"" message for Category : ""(.*)"" Vehicle")]
        public void ThenIShouldSeeTradeIsNotPossibleMessageForCategoryVehicle(string TradeMsg, string Category)
        {
            Boolean isStockErrMsg = false;

            if ((TradeMsg.Contains("STOCK TRADE NOT ALLOWED. UNIT NOT IN STOCK STATUS")) || (TradeMsg.Contains("Category F cannot be Traded")))
            {
                Console.WriteLine("Vehicle needs to be tested for not Traded Successfully.");
                Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                string vehTradeMsg = Pages.InventoryPage().GetTradeAlertMsg();

                if (vehTradeMsg != null)
                {
                    Console.WriteLine("Trade Alert Msg = " + vehTradeMsg);
                    if (vehTradeMsg.Contains(TradeMsg))
                        isStockErrMsg = true;
                    Assert.IsTrue(isStockErrMsg, "Found Error Message as " + vehTradeMsg + " for the Category : " + Category + " Vehicle");
                }
                else
                    Assert.Fail("Trade Alert message was blank.");
            }

        }

        [When(@"I click to Trade Summary Page")]
        public void WhenIClickToTradeSummaryPage()
        {
            try
            {
                if (Driver.GetDriver().Title.Contains("Details"))
                {
                    Boolean GoBackButton = Pages.VehicleDetailPage().IsBackToButtonClicked();
                    if (GoBackButton == true)
                        Pages.InventoryPage().ClearFilter();
                }

                Pages.HomePage().TradeSummaryTab.Click();
            }
            catch (NoSuchWindowException ex)
            {
                Console.WriteLine("Trade Summary Page didn't get open. Please check this error : "+ex.StackTrace);
            }
        }
        [Then(@"I should land to Trade Summary Page")]
        public void ThenIShouldLandToTradeSummaryPage()
        {
            Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            Assert.IsTrue(Pages.TradeSummaryPage().GetTradePanelLabel(), "Trade Summary Page Navigation title");
           
        }

        [Then(@"I should see traded vehicle ""(.*)"" listed on Trade Summary Page with Trade Type ""(.*)""")]
        public void ThenIShouldSeeTradedVehicleListedOnTradeSummaryPage(string vin, string tradeType)
        {
            if (exp_obj.vin != "")
            {
                comman.ClearFilter(Pages.TradeSummaryPage().btnClear);
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            }

            Boolean isVinAvailable = Pages.TradeSummaryPage().SearchByVin(vin);

            Console.WriteLine("Search Vin From Trade Summary Page------> " + isVinAvailable);

            if ( (isVinAvailable == true) && (tradeType != "") )
            {
                Boolean isTradedVinStatusFound = false;
                isTradedVinStatusFound = Pages.TradeSummaryPage().GetTradeStatusForSearchedVin(vin, tradeType);
                exp_obj.vin = vin;
                
                Assert.IsTrue(isTradedVinStatusFound, "Traded Vin found from the Trade Summary Page with Status : "+ tradeType);
            }
            else
                Assert.Fail("Vin from Trade Summary Page was not found with Trade Status : " + tradeType+ ". Trade may not have been done successfully.");
        }

        [Then(@"I should see traded vehicle ""(.*)"" listed on Trade Summary Page with Comments ""(.*)""")]
        public void ThenIShouldSeeTradedVehicleListedOnTradeSummaryPageWithComments(string vin, string comments)
        {            
            Boolean isCommentFound = Pages.TradeSummaryPage().GetCommentsForSearchedVin(vin, comments);
            Assert.IsTrue(isCommentFound, "Traded Vehicle Comments Verification for the VIN: "+vin);           
        }

        [When(@"I select Actions Menu for category ""(.*)""")]
        
        public void WhenISelectActionsMenu(string category)
        {
            isTradWindowAppeared = comman.selectActionsOption("Trade", exp_obj.vin);

        }

        [Then(@"I should not see Trade Option available within the Action Menu")]
        public void ThenIShouldNotSeeTradeOptionAvailableWithinTheActionMenu()
        {

            if (isTradWindowAppeared == true)
                Assert.Fail("Trade link was present for vehicle :"+exp_obj.vin);
            else
                Assert.IsFalse(isTradWindowAppeared, "Trade link for vehicle is not present");
        }

        [Then(@"I should not see traded vehicle ""(.*)"" listed on Trade Summary Page due to their Category ""(.*)""")]
        public void ThenIShouldNotSeeTradedVehicleListedOnTradeSummaryPageDueToTheirCategory(string vin, string catg)
        {
            Boolean isVinAvailable = Pages.TradeSummaryPage().SearchByVin(vin);

            Console.WriteLine("Search Vin From Trade Summary Page'''' " + isVinAvailable);

            if (isVinAvailable == true)
            {
                Assert.Fail("Vin from Trade Summary Page was found for category : "+catg+ " vehicle");
            }
            else
                Assert.IsFalse(isVinAvailable, "Traded Vin was not found from the Trade Summary Page for vehicle with category : " + catg);
                           
        }

        [Then(@"I should be re-route to Inventory Page via Detail Page if Back to Inventory link is present")]
        public void ThenIShouldBeRe_RouteToInventoryPageViaDetailPageIfBackToInventoryLinkIsPresent()
        {
            Boolean GoBackButton = Pages.VehicleDetailPage().IsBackToButtonClicked();
            if (GoBackButton == true)
            {
                Wait.Until(x => (x.Title != Pages.InventoryPage().Title));    
            }
            Assert.AreEqual(Pages.InventoryPage().Title, Driver.GetDriver().Title, "inventory header is different");
        }


      

    }
}
