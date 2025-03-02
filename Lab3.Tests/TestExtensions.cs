using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Lab3.Tests
{
    public static class TestExtensions
    {
        public static void FillInFormField(this ChromeDriver driver, string id, string value)
        {
            driver
                .FindElement(By.XPath($"//input[@id = '{id}']"))
                .SendKeys(value);
        }

        public static void ClickByXPath(this ChromeDriver driver, string xpath)
        {
            driver
                .FindElement(By.XPath(xpath))
                .Click();
        }
    }
}