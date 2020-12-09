using System;
using System.Collections.Generic;
using System.Text;
using AutomationFoundationDotNetCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using static AutomationFoundationDotNetCore.SettingsBase;

namespace DotNetCoreSpecFlowTemplate.AppPages
{
    public class VehicleDetailPage : BasePage
    {
        WebDriverWait Wait = new WebDriverWait(Driver.GetDriver(), TimeSpan.FromSeconds(60));

        public string Title = "Dealer Daily";

        public VehicleDetailPage()
        {
            PageFactory.InitElements(Driver.GetDriver(), this);
        }

        public readonly AllPages Pages = new AllPages();

        [FindsBy(How = How.XPath, Using = "*//[text()='Traded Successfully']")]
        public IWebElement TradeSuccessMsg { get; set; }

        

        [FindsBy(How = How.XPath, Using = "//div[@class='dd-app-navbar-container-item label']//span[text() =' Back to Inventory ']")]
        public IWebElement GobackToInventoryLnk { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='vehicle-location']")]
        public IWebElement VehicleLocation { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='row no-gutters tm-tab-panel-container-inner sticky']//button[text() =' Quick Actions ']")]
        public IWebElement QuickActionsLnk { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='dd-app-navbar-container-item navigation']/div[2]/span[text() =' Inventory Management ']")]
        public IWebElement TitleMgmt { get; set; }

        //  <span _ngcontent-exw-c21="" class="action__menu-list-item-label"> Trade</span>
        [FindsBy(How = How.XPath, Using = "*//span[text() =' Trade ']")]
        public IWebElement TradeLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='dd-inventory-tradeform']//div[@class='text-lg  red']")]
        public IWebElement TradePopupTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='trade-input']")]
        public IWebElement inputTradeTo { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='dealer-address ellipsis']")]
        public IWebElement lnkTradeTo { get; set; }

        [FindsBy(How = How.XPath, Using = "//textarea[@formcontrolname='comment']")]
        public IWebElement inputTradeComment { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@class='btn-primary']")]
        public IWebElement BtnTrade { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='calculated-status-label']")]
        public IWebElement vechicleAvailable { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='trade-dialog __cdk-scrollbar__']")]
        public IWebElement TradeMessageDialog { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='tm-modal-dialog-container __cdk-scrollbar__']")]
        public IWebElement TradeDialog { get; set; }

        public Boolean IsBackToButtonClicked()
        {
            Boolean isBackBtn = false;
            try
            {
                //Clear Filter from Inventory Home page if its already applied
                
                if (GobackToInventoryLnk.Displayed)
                {
                    Console.WriteLine("I see Go Back To Inventory Link");
                    GobackToInventoryLnk.Click();
                    isBackBtn = true;
                }
            }
            catch (NoSuchElementException ex)
            {
                Pages.InventoryPage().BtnGotIt.Click();
                //string alertMasg = Pages.InventoryPage().GetTradeAlertMsg();
                Console.WriteLine("Go Back To Inventory link is not present ." + ex.StackTrace);

            }
            return isBackBtn;


        }

        public string GetVechileInventoryStatus()
        {
            return vechicleAvailable.Text.ToString();
        }

        public Boolean AccessTradePopup()
        {
            Boolean isTradePopoupAppear = false;

            try
            {
                Console.WriteLine("Found Vehicle in Status=====> " + GetVechileInventoryStatus());
                if ((GetVechileInventoryStatus().Contains("AVAILABLE")) || (GetVechileInventoryStatus().Contains("ALLOCATED")) || (GetVechileInventoryStatus().Contains("IN TRANSIT")))
                {
                    Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                    if (QuickActionsLnk.Displayed == true)
                    {
                        Console.WriteLine("Quick Actions link is present.");

                        QuickActionsLnk.Click();
                        Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                        if (TradeLink.Displayed)
                        {
                            Console.WriteLine("Trade link is present.");
                            TradeLink.Click();
                            Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                            Wait.Until(x => (TradeDialog.Displayed != true));
                            Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                            Console.WriteLine("Trade Popup Title======" + TradePopupTitle.Text);
                            if (TradePopupTitle.Text.Trim() == "Trade vehicles")
                            {
                                isTradePopoupAppear = true;
                            }
                        }

                    }
                    else
                        Console.WriteLine("Unable to see Quick Actions Link from Vehicle Detail Page.");


                }
                else
                    Console.WriteLine("Status of Vehicle was not tradeable. Olease check the vin before you execute." + GetVechileInventoryStatus());
              

            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to access Trade Popup to make trade." +e.StackTrace);
            }
            return isTradePopoupAppear;
        }

        public void MakeTrade(string tradeTo, string comment)
        {
           
            try
            {
               
                Wait.Until(x=> (TradePopupTitle.Text.Trim().Contains("Trade vehicles")));

                inputTradeTo.SendKeys(tradeTo);
                lnkTradeTo.Click();
                inputTradeComment.Click();
                inputTradeComment.SendKeys(comment);
                BtnTrade.Click();

                Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);



            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to click on Trade from Trade Vehicle Popup." + e.StackTrace);
            }
            
        }

        

        public void GoBackToInventoryPage()
        {
            try
            {
                GobackToInventoryLnk.Click();
            }
            catch(NoSuchElementException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

    }
}
