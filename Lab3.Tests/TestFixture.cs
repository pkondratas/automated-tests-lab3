
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Lab3.Tests
{
    public class TestFixture : IAsyncLifetime, IDisposable
    {
        public string FirstName = "Paul";
        public string LastName = "James";
        public string Password = "mypassword";
        public required string Email = $"paul.james{ new Random().Next() }@gmail.com";

        public async Task InitializeAsync()
        {
            await CreateUser();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task CreateUser()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");

            var driver = new ChromeDriver(options);
            var waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            await driver.Navigate().GoToUrlAsync("https://demowebshop.tricentis.com/");

            driver.Manage().Window.Maximize();

            driver.ClickByXPath("//a[@href = '/login']");
            driver.ClickByXPath("//input[@value = 'Register']");
            driver.ClickByXPath("//input[@id = 'gender-male']");

            driver.FillInFormField("FirstName", FirstName);
            driver.FillInFormField("LastName", LastName);
            driver.FillInFormField("Email", Email);
            driver.FillInFormField("Password", Password);
            driver.FillInFormField("ConfirmPassword", Password);

            driver.ClickByXPath("//input[@id = 'register-button']");

            waiter
                .Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@value = 'Continue']")))
                .Click();

            waiter.Until(d => d.ExecuteJavaScript<string>("return document.readyState")!.Equals("complete"));

            driver.Close();   
        }

        public void Dispose()
        {
            
        }
    }
}