using System;
using System.Collections.Generic;
using System.Text;
using AutomationFoundationDotNetCore;

namespace DotNetCoreSpecFlowTemplate.AppPages
{
    public class AllPages
    {
        // we need a private page object and a getter function for each page class created for each physical page .

        private BasePage _basePage;
        private LoginPage _loginPage;
        private HomePage _homePage;
        private InventoryPage _inventoryPage;
        private LocatorPage _locatorPage;
        private RDRPage _rdrPage;
        private SalesPage _salesPage;
        private TradeSummaryPage _tradeSummaryPage;
        private VehicleDetailPage _VechicleDetailPage;



        public BasePage BasePage()
        {
            if (_basePage == null) _basePage = new BasePage();
            return _basePage;
        }

        public InventoryPage InventoryPage()
        {
            if (_inventoryPage == null) _inventoryPage = new InventoryPage();
            return _inventoryPage;
        }
        public LocatorPage LocatorPage()
        {
            if (_locatorPage == null) _locatorPage = new LocatorPage();
            return _locatorPage;
        }

        public RDRPage RDRPage()
        {
            if (_rdrPage == null) _rdrPage = new RDRPage();
            return _rdrPage;
        }

        public SalesPage SalesPage()
        {
            if (_salesPage == null) _salesPage = new SalesPage();
            return _salesPage;
        }

        public TradeSummaryPage TradeSummaryPage()
        {
            if (_tradeSummaryPage == null) _tradeSummaryPage = new TradeSummaryPage();
            return _tradeSummaryPage;
        }


        public LoginPage LoginPage()
        {
            if (_loginPage == null) _loginPage = new LoginPage();
            return _loginPage;
        }

        public HomePage HomePage()
        {
            if (_homePage == null) _homePage = new HomePage();
            return _homePage;
        }

        public VehicleDetailPage VehicleDetailPage()
        {
            if (_VechicleDetailPage == null) _VechicleDetailPage = new VehicleDetailPage();
            return _VechicleDetailPage;
        }


    }
}
