using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HelperNuget.Packages.Pdf
{
    public static class PdfHelper
    {
        public static async Task GeneratePdfFromHtml(string outputFile)
        {           
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
          
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 500,
                Height = 500
            });

            await page.GoToAsync("http://www.google.com");

            var result = await page.GetContentAsync();

            await page.PdfAsync(outputFile);
        }
    }
}
