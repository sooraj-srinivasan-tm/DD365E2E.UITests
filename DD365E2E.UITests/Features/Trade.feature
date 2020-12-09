@DD365
Feature: DD365 Trade Tests
	As a user I should be able to login to dealer daily portal and Trade a vehicle
	
	@tc:01
	Scenario: (1) As a dealer I should be able to access DD365 Login Page
		Given I am at the DD365 login page for Trade
		When I enter username and password to login for Trade
		Then I should land to home page for Trade

#	@tc:02
#	Scenario: (2) As a dealer I should see inventory on the Inventory Summary tab
#		When I click on Inventory Summary tab for Trade
#		Then I should see the Inventory header for Trade

	@tc:03
	Scenario Outline: (3) As a dealer I should be able to Trade vechicle from Inventory Page
		When I click on Inventory Summary tab for Trade
		Then I should see the Inventory header for Trade
		When I search for a Category: "<Catg>" Vin : "<VIN>" to trade
		Then I should see searched vehicle on the Inventory page
		When I select the Trade link based on Expected Trade "<TradePopupRequired>"
		Then I should be able to see Trade Vehicle popup window based on Expected Trade "<TradePopupRequired>"
		When I enter trade to dealer "<DealerToTrade>" and note "<DealerNote>" and Click Trade
		Then I should see "<ExpectedTrade>" message for Category : "<Catg>" vehicle
		When I click on Inventory Summary tab for Trade
		Then I should see the Inventory header for Trade
		When I click to Trade Summary Page
		Then I should land to Trade Summary Page
		And I should see traded vehicle "<VIN>" listed on Trade Summary Page with Trade Type "<TradeType>"
		And I should see traded vehicle "<VIN>" listed on Trade Summary Page with Comments "<DealerNote>"
		When I click on Inventory Summary tab for Trade
		Then I should be re-route to Inventory Page via Detail Page if Back to Inventory link is present	
		
	Examples: 
	| TestCase | VIN               | Catg | DealerToTrade | DealerNote | TradePopupRequired | ExpectedTrade    | TradeType |
	| TC01     | 4T1C11AK9LU889308 | G    | 42095         | TestNote1  | Yes                | Trade successful | SENT      |
	| TC02     | JTDEPMAE2MJ149979 | A    | 42095         | TestNote2  | Yes                | Trade successful | SENT      |
	
	
	@tc:04
	Scenario Outline: (4) As a dealer I should be able to see Trade not possible message for certain vehicles
		When I click on Inventory Summary tab for Trade
		Then I should see the Inventory header for Trade
		When I search for a Category: "<Catg>" Vin : "<VIN>" to trade
		Then I should see searched vehicle on the Inventory page
		When I select the Trade link based on Expected Trade "<TradePopupRequired>"
		Then I should be able to see Trade Vehicle popup window based on Expected Trade "<TradePopupRequired>"
		When I enter trade to dealer "<DealerToTrade>" and note "<DealerNote>" and Click Trade
		Then I should see Trade is not possible "<ExpectedTrade>" message for Category : "<Catg>" Vehicle
		When I click on Inventory Summary tab for Trade
		Then I should see the Inventory header for Trade
		When I click to Trade Summary Page
		Then I should land to Trade Summary Page
		And I should not see traded vehicle "<VIN>" listed on Trade Summary Page due to their Category "<Catg>"
		
	Examples: 
	| TestCase | VIN               | Catg | DealerToTrade | DealerNote | TradePopupRequired | ExpectedTrade										|
	| TC01     | JF1ZNAE17L8752998 | F    | 42095         | TestNote1  | Yes				| Category F cannot be Traded						|
	| TC02     | 5YFEPMAE0MP165285 | G    | 42095         | TestNote2  | Yes				| STOCK TRADE NOT ALLOWED. UNIT NOT IN STOCK STATUS |

	@tc:05
	Scenario Outline: (5) As a dealer I should not be able to Trade vechicle for catgory GJ, FJ, and AJ
		When I click on Inventory Summary tab for Trade
		Then I should see the Inventory header for Trade
		When I search for a Category: "<Catg>" Vin : "<VIN>" to trade
		Then I should see searched vehicle on the Inventory page
		When I select Actions Menu for category "<Catg>"
		Then I should not see Trade Option available within the Action Menu
		When I click to Trade Summary Page
		Then I should land to Trade Summary Page
		And I should not see traded vehicle "<VIN>" listed on Trade Summary Page due to their Category "<Catg>"
		
	Examples: 
	| TestCase | VIN               | Catg | DealerToTrade | DealerNote | TradePopupRequired | ExpectedTrade                                     |
	| TC01     | 5TFDY5F14LX913136 | GJ   | 42095         |			   | No					| No Trade allowed									|
#	| TC02	   | 4T1F31AK6LU542669 | FJ   | 42095         |			   | No                 | No Trade allowed									|
#	| TC03	   | 4T1F31AK0LU543168 | AJ   | 42095         |            | No                 | No Trade allowed									|

	@tc:06
	Scenario Outline: (6) As a dealer I should not be able to Trade vechicle which are already traded
		When I click on Inventory Summary tab for Trade
		Then I should see the Inventory header for Trade
		When I search for a Category: "<Catg>" Vin : "<VIN>" to trade
		Then I should see searched vehicle on the Inventory page
		When I select Actions Menu for category "<Catg>" from Detail vehicle page
		Then I should not see Trade Option available within the Action Menu
		When I click on Inventory Summary tab for Trade
		Then I should see the Inventory header for Trade
		When I click to Trade Summary Page
		Then I should land to Trade Summary Page
		And I should not see traded vehicle "<VIN>" listed on Trade Summary Page due to their Category "<Catg>"
		
		Examples: 
		| TestCase | VIN               | Catg | DealerToTrade | DealerNote | TradePopupRequired | ExpectedTrade                                     |
		| TC01     | 3MYDLBJVXLY705947 | G    | 42095         |            | No					| No Trade Required Due to already Traded           |
		| TC02     | NMTKHMBX6LR118363 | A    | 42095         |			   | No					| No Trade Required Due to already Traded           |
