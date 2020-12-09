using System;
using System.Collections.Generic;
using System.Text;
using AutomationFoundationDotNetCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using static AutomationFoundationDotNetCore.SettingsBase;

namespace DotNetCoreSpecFlowTemplate.AppPages
{
    public class LocatorPage
    {
        // here we will keep all the common functions, web elements, variables to be inherited to other pages
        // no constructor here
        // parameters username, password, dbString, url etc can be called from the AppConfig.json

        // DB string - depending on the DB in test we can add or modify existing one in App.config
        public static string dbString = GetJsonConfigurationValue("DatabaseConnectionString");
        public string Title = "Locate";
        // Common web elements should be defined here like headers, footers, buttons and links visible on all pages

        [FindsBy(How = How.Id, Using = "sampleId1")]
        public IWebElement CommonWebElement1 { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='SampleId2']")]
        public IWebElement CommonWebElement2 { get; set; }


        // the links at the footer and header are visible and reachable from all pages, we will keep them here but we will call them from the page where we are during the test
        
        // we do not use above web element format because we need a dynamic way when we call each section link from the examples table
        public IWebElement SectionLink(string text)
        {
            var element = Driver.GetDriver().FindElement(By.LinkText(text));
            return element;
        }

    }
}
