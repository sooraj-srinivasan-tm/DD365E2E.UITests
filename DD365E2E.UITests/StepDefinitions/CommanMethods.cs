using AutomationFoundationDotNetCore;

using DotNetCoreSpecFlowTemplate.AppPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using System;

namespace DD365E2E.UITests.AppPages
{
    class CommanMethods
    {

        public readonly AllPages Pages = new AllPages();
       
        public CommanMethods()
        {
            PageFactory.InitElements(Driver.GetDriver(), this);
        }

        public bool Click(By by)
        {
            bool status = false;
            int i = 0;
            while (i == 0)
                try
                {
                    Driver.GetDriver().FindElement(by).Click();
                    status = true;
                    break;
                }
                catch (StaleElementReferenceException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            return status;
        }

       
        public void ClearFilter(IWebElement ele)
        {
            if (ele.Displayed == true)
            {
                Actions action = new Actions(Driver.GetDriver());
                action.Click(ele).Perform();
                //BtnClear.Click();
                Driver.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                if (ele.Displayed == false)
                    Console.WriteLine("Clear button is not displayed after it was clicked once.");
            }


        }
        public Boolean selectActionsOption(string optionName, string expectedVin)
        {
            Boolean isTradeVehiclePopsup = false;

            try
            {
                Driver.GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);


                if (optionName.Contains("Trade"))
                {
                    Console.WriteLine("Window Title after clicking on Trade link....." + Driver.GetDriver().Title);
                    Console.WriteLine("Option for Vehicle==================>" + optionName);
                    if (Driver.GetDriver().Title.Contains(Pages.InventoryPage().Title))
                    {
                        Console.WriteLine("Vin for Trade Vehicle==================>" + expectedVin);
                       
                        isTradeVehiclePopsup = Pages.InventoryPage().selectTradeLink(expectedVin);
                    }
                    else
                    {
                        Console.WriteLine("Not on Inventory Page.");
                    }
                    Console.WriteLine("Trade Vechicle Popup Appeared Status : " + isTradeVehiclePopsup);
                }
                
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return isTradeVehiclePopsup;
        }

    }
}
