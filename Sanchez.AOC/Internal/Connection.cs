using System;
using PuppeteerSharp;

namespace Sanchez.AOC.Internal
{
    public class Connection
    {
        public static async Task<string> CreateAccessToken()
        {
            using BrowserFetcher browserFetcher = new();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions()
            {
                Headless = false,
                DefaultViewport = null
            });

            Page page = (await browser.PagesAsync()).First();
            await page.GoToAsync("https://google.com");

            await page.WaitForSelectorAsync(".HelloWorld");

            return "hello";
        }
    }
}

