using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Lab3.Tests
{
    public class AutomatedTests : IAsyncLifetime
    {
        private ChromeDriver Driver = null!;
        private WebDriverWait Waiter = null!;

        public async Task InitializeAsync()
        {   
            var options = new ChromeOptions();
            options.AddArguments(["--headless=new", "--no-sandbox", "--disable-dev-shm-usage"]);

            Driver = new ChromeDriver(options);
            Waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));

            await Driver.Navigate().GoToUrlAsync("https://demowebshop.tricentis.com/");

            Driver.Manage().Window.Maximize();
        }

        public Task DisposeAsync()
        {
            Driver.Close();

            return Task.CompletedTask;
        }

        [Theory]
        [InlineData("data1.txt")]
        // [InlineData("data2.txt")]
        public async Task ConfirmCreatedOrderTest(string fileName)
        {
            // Act
            Driver.ClickByXPath("//a[@href = '/login']");
            
            Driver.FillInFormField("Email", "");
            Driver.FillInFormField("Password", "");
            
            Driver.ClickByXPath("//input[@value = 'Log in']");
            Driver.ClickByXPath("//div[@class = 'leftside-3']/descendant::a[@href='/digital-downloads']");
            
            var items = await Utils.ReadWordsInFileAsync(fileName);

            foreach (var item in items)
            {
                Waiter.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@id = 'bar-notification']")));

                Driver.ClickByXPath($"//h2[a[text() = '{item}']]/following-sibling::*/*/input[@value = 'Add to cart']");
             
                Waiter.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id = 'bar-notification']")));
            }

            Driver.ClickByXPath($"//a[@href = '/cart']");
            Driver.ClickByXPath($"//input[@id = 'termsofservice']");
            Driver.ClickByXPath($"//button[@id = 'checkout']");

            if (Driver.FindElement(By.XPath("//div[@id = 'billing-new-address-form']")).Displayed)
            {
                var address = new SelectElement(Driver.FindElement(By.XPath("//select[@id = 'BillingNewAddress_CountryId']")));

                address.SelectByText("Lithuania");

                Driver.FillInFormField("BillingNewAddress_City", "Vilnius");
                Driver.FillInFormField("BillingNewAddress_Address1", "Didlaukio g. 59");
                Driver.FillInFormField("BillingNewAddress_ZipPostalCode", "12345");
                Driver.FillInFormField("BillingNewAddress_PhoneNumber", "068686868");
            }

            Driver.ClickByXPath($"//div[@id = 'billing-buttons-container']/input");
            Waiter.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//div[@id = 'payment-method-buttons-container']/input"))).Click();
            Waiter.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//div[@id = 'payment-info-buttons-container']/input"))).Click();
            Waiter.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//div[@id = 'confirm-order-buttons-container']/input"))).Click();

            // Assert
            Assert.True(Waiter.Until(d => d.FindElement(By.XPath("//strong[text() = 'Your order has been successfully processed!']")).Displayed));
        }
    }
}
