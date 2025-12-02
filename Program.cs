using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace AutomationTest
{
    public class HeaderDisplayTests : IDisposable
    {
        private ChromeDriver _driver;
        private const string BravePath = @"C:\Program Files\BraveSoftware\Brave-Browser\Application\brave.exe";

        public HeaderDisplayTests()
        {
            ChromeOptions options = new()
            {
                BinaryLocation = BravePath
            };
            _driver = new ChromeDriver(options);
        }

        [Fact]
        public void TestBalenciaga_HeaderDisplayedAfterSearch()
        {
            // Arrange
            const string url = "https://www.balenciaga.com/en-en";
            const string searchSelector = "button.c-topsearch__button";

            // Act & Assert
            TestHeaderDisplay(url, searchSelector);
        }

        [Fact]
        public void TestGitHub_HeaderDisplayedAfterSearch()
        {
            // Arrange
            const string url = "https://github.com/";
            const string searchSelector = "button.header-search-button";

            // Act & Assert
            TestHeaderDisplay(url, searchSelector);
        }

        [Fact]
        public void TestMicrosoftLearn_HeaderDisplayedAfterSearch()
        {
            // Arrange
            const string url = "https://learn.microsoft.com/en-us/";
            const string searchSelector = "input[type='text']";

            // Act & Assert
            TestHeaderDisplay(url, searchSelector);
        }

        private void TestHeaderDisplay(string url, string searchSelector)
        {
            try
            {
                _driver.Navigate().GoToUrl(url);
                Thread.Sleep(3000);

                IWebElement searchElement = _driver.FindElement(By.CssSelector(searchSelector));
                searchElement.Click();
                Thread.Sleep(1500);

                // Find the actual input field after clicking (if searchSelector is a button)
                IWebElement? searchInput = null;
                if (searchElement.TagName != "input")
                {
                    searchInput = FindSearchInput(_driver);
                }
                else
                {
                    searchInput = searchElement;
                }

                Assert.NotNull(searchInput);

                Thread.Sleep(500);

                searchInput.Click();
                searchInput.SendKeys("computer");
                searchInput.SendKeys(Keys.Enter);
                Thread.Sleep(3000);

                IWebElement header = _driver.FindElement(By.CssSelector("header"));
                Assert.True(header.Displayed, $"{url}: Header should be displayed after search");
            }
            catch (NoSuchElementException ex)
            {
                Assert.Fail($"Element not found: {ex.Message}");
            }
        }

        private static IWebElement? FindSearchInput(IWebDriver driver)
        {
            string[] selectors =
            {
                "input#query-builder-test",
                "input[type='search']",
                "input[name='q']",
                "input[placeholder*='search' i]",
                "input[placeholder*='Search' i]",
                "input[type='text']"
            };

            foreach (var selector in selectors)
            {
                try
                {
                    var element = driver.FindElement(By.CssSelector(selector));
                    if (element.Displayed)
                    {
                        return element;
                    }
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }

            return null;
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
