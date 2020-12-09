using System;
using System.Collections.Generic;
using System.Text;
using AutomationFoundationDotNetCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace DotNetCoreSpecFlowTemplate.AppPages
{
    public class HomePage : BasePage
    {
        public HomePage()
        {
            PageFactory.InitElements(Driver.GetDriver(), this);
        }

        //[FindsBy(How = How.LinkText, Using = "Inventory Summary")]
        //public IWebElement InventoryTab { get; set; }
    }
}
