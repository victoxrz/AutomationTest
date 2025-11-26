using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationTest
{
    internal class Program
    {
        static void Main()
        {
            const string url1 = "https://www.netflix.com/md-en/";
            const string url2 = "https://www.adidas.com/us";

            TestHeaderDisplay(url1);
            TestHeaderDisplay(url2);
        }

        static void TestHeaderDisplay(string url)
        {
            ChromeOptions options = new()
            {
                BinaryLocation = @"C:\Program Files\BraveSoftware\Brave-Browser\Application\brave.exe"
            };

            IWebDriver driver = new ChromeDriver(options);

            try
            {
                driver.Navigate().GoToUrl(url);

                Thread.Sleep(3000);

                IWebElement header = driver.FindElement(By.CssSelector("header"));
                
                if (header.Displayed)
                {
                    Console.WriteLine($"{url}: Header is displayed!");
                }
                else
                {
                    Console.WriteLine($"{url}: Header is NOT displayed.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"{url}: Header element not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{url}: An error occurred: {ex.Message}");
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
