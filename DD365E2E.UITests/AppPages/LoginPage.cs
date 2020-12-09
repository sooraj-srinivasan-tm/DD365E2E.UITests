using System;
using System.Collections.Generic;
using System.Text;
using AutomationFoundationDotNetCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using static AutomationFoundationDotNetCore.SettingsBase;

namespace DotNetCoreSpecFlowTemplate.AppPages
{
    public class LoginPage : BasePage
    {

        public string Title = "Gulf States Toyota DEV - Sign In";

        public LoginPage()
        {
            PageFactory.InitElements(Driver.GetDriver(), this);
        }

        // Common web elements should be defined here like headers, footers, buttons and links visible on all pages

        [FindsBy(How = How.Id, Using = "idp-discovery-username")]
        public IWebElement UsernameInput { get; set; }

        [FindsBy(How = How.Id, Using = "okta-signin-password")]
        public IWebElement PasswordInput { get; set; }

        [FindsBy(How = How.Id, Using = "idp-discovery-submit")]
        public IWebElement NextButton { get; set; }

        [FindsBy(How = How.Id, Using = "okta-signin-submit")]
        public IWebElement SigninButton { get; set; }


        // we do not use above web element format because we need a dynamic way when we call each section link from the examples table
        public IWebElement SectionLink(string text)
        {
            var element = Driver.GetDriver().FindElement(By.LinkText(text));
            return element;
        }

        public void LogInToDD365(string userName, string password)
        {
            UsernameInput.SendKeys("");
            UsernameInput.SendKeys(userName);
            NextButton.SendKeys("");
            NextButton.Click();
            PasswordInput.SendKeys("");
            PasswordInput.SendKeys(password);
           
            SigninButton.Click();

        }

    }
}
