using Microsoft.Playwright;
using System.Text.RegularExpressions;


[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace CloseADTests
{
    public class CloseADTest
    {
 
        [Test]
        public async Task Visitsite() 
        {
            // Launch a Chrome browser instance
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Set to false to show the browser UI
            });
        
            // Create a new page
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            
            await context.RouteAsync("**/*", async r =>
            {
                if (Regex.IsMatch(r.Request.Url, @"googleads"))
                    await r.AbortAsync();
                else
                    await r.ContinueAsync();
            });
            await page.GotoAsync("https://automationexercise.com/");
        
            // Assert that the page title contains "Automation Exercise"
            Assert.IsTrue((await page.TitleAsync()).Contains("Automation Exercise"));
            
            var womenElememt = await page.QuerySelectorAsync("a[href='#Women']");
            if (womenElememt != null)
            {
                // Click on the element
                await womenElememt.ClickAsync();
                var dressElement = await page.QuerySelectorAsync("a[href='/category_products/1']");
                    
                if(dressElement != null)
                {   
                    Console.WriteLine("dress before"); 
                    // var dialogTask = page.WaitForEventAsync(PageEvent.Dialog);
                    await dressElement.ClickAsync();
                    Console.WriteLine("dress before");
                    

                    // GetByRole(AriaRole.Button, new() { Name = "Close ad" })
                    await Assertions.Expect(page).ToHaveTitleAsync("Automation Exercise - Dress Products");
                    Console.WriteLine("it means test finished if you see this"); 
                }
                
            }
            else
            {
                // Handle the case where the element was not found
                Console.WriteLine("Could not find the Women element.");
            }

           
        }

        
        
    }
}
