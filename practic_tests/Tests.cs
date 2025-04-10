using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace practic_tests;

public class Tests
{
    protected ChromeDriver _driver;
    protected WebDriverWait _wait;

    [SetUp]
    public void Setup()
    {
        ChromeOptions options = new();
        options.AddArguments("--no-sandbox", "--start-maximazed", "--disable-extensions", "--window-size=800,1000");

        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        Login("username", "password");
    }

    private void Login(string username, string password)
    {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");

        _driver.FindElement(By.Id("Username")).SendKeys(username);
        _driver.FindElement(By.Id("Password")).SendKeys(password);
        _driver.FindElement(By.Name("button")).Click();

        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}