
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;




namespace AutomationPracticeTests
{
    public class OnlineshopTest
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IPage? _page;

        [SetUp]
        public async Task Setup()
        {
            // Launch a Chrome browser instance
            _playwright = await Playwright.CreateAsync();
            var browserTypes = new string[] { BrowserType.Chromium, BrowserType.Firefox, BrowserType.Webkit };
            foreach (var browserType in browserTypes)
            {
                _browser = await _playwright[browserType].LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false, // Set to false to show the browser UI
                    
                });
            
                // Create a new page
                _page = await _browser.NewPageAsync();
                await _page.RouteAsync("**/*", async r =>
                {
                    if (Regex.IsMatch(r.Request.Url, @"googleads"))
                        await r.AbortAsync();
                    else
                        await r.ContinueAsync();
                });

            }
           
        }

        [Test]
       
        public async Task Visitsite() 
        {
            // Visit the Google website
            if(_page !=null)
            {
                await _page.GotoAsync("https://automationexercise.com/");
            }
            else
            {
                Console.WriteLine("Could not find _page.");
            }

            // Assert that the page title contains "Google"
             if(_page !=null)
             {
                Assert.IsTrue((await _page.TitleAsync()).Contains("Automation Exercise"));
            
                
             }
            var womenElememt = await _page.QuerySelectorAsync("a[href='#Women']");
            if (womenElememt != null)
            {
                // Click on the element
                await womenElememt.ClickAsync();
                var dressElement = await _page.QuerySelectorAsync("a[href='/category_products/1']");
                
                if(dressElement != null)
                {   
                    Console.WriteLine("dress before"); 
                    await dressElement.ClickAsync();
                    Console.WriteLine("dress before");  
                    await Assertions.Expect(_page.GetByText("Women - Dress Products")).ToBeVisibleAsync();
                    Console.WriteLine("it means test finished if you see this"); 
                }
                 
            }
            else
            {
                // Handle the case where the element was not found
                Console.WriteLine("Could not find the Women element.");
            }
        }

        [TearDown]
        public async Task Teardown()
        {
            // Close the browser and dispose of the Playwright instance
            if(_browser !=null)
            {
                await _browser.CloseAsync();
            }
            else
            {
                Console.WriteLine("Could not find _browser.");
            }

            if(_playwright !=null)
            {
                _playwright.Dispose();
            }
            else
            {
                Console.WriteLine("Could not find _playwright.");
            }
            
            
            
        }
    }
}

