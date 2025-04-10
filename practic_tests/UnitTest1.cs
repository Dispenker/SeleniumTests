using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace practic_tests;

public class Tests
{
    private ChromeDriver _driver;
    private WebDriverWait _wait;

    [SetUp]
    public void Setup()
    {
        ChromeOptions co = new();
        co.AddArguments("--no-sandbox", "--start-maximazed", "--disable-extensions", "--window-size=800,1000");

        _driver = new ChromeDriver(co);
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

    [Test]
    public void Authorization_test()
    {
        Assert.That(
            _driver.FindElement(By.CssSelector("[data-tid='Title']")).Text,
            Is.EqualTo("Новости"),
            "На странице Новостей нет названия 'Новостu'"
        );
    }

    private string TitleFromSidebarMenuElement(string buttonName, string endPath, string titleSelector = "[data-tid='Title']")
    {
        _driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']")).Click();

        string buttonSelector = $"[data-tid='SidePageBody'] [data-tid='{buttonName}']";
        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(buttonSelector)));
        _driver.FindElement(By.CssSelector(buttonSelector)).Click();

        _wait.Until(ExpectedConditions.UrlToBe($"https://staff-testing.testkontur.ru/{endPath}"));
        return _driver.FindElement(By.CssSelector(titleSelector)).Text;
    }

    [Test]
    public void SidebarMenu_test()
    {
        Assert.That(
            TitleFromSidebarMenuElement("Comments", "comments"),
            Is.EqualTo("Комментарии"),
            "На странице Комментариев нет названия 'Комментарии'");
        Assert.That(
            TitleFromSidebarMenuElement("Messages", "messages"),
            Is.EqualTo("Диалоги"),
            "На странице Диалогов нет названия 'Диалоги'");
        Assert.That(
            TitleFromSidebarMenuElement("Community", "communities"),
            Is.EqualTo("Сообщества"),
            "На странице Сообщества нет названия 'Сообщества'");
        Assert.That(
            TitleFromSidebarMenuElement("Events", "events", "[data-tid='Actual']"),
            Is.EqualTo("Актуальные"),
            "На странице Мероприятий нет вкладки 'Актуальные'");
        Assert.That(
            TitleFromSidebarMenuElement("Files", "files"),
            Is.EqualTo("Файлы"),
            "На странице Файлов нет названия 'Файлы'");
        Assert.That(
            TitleFromSidebarMenuElement("Documents", "documents"),
            Is.EqualTo("Документы"),
            "На странице Документов нет названия 'Документы'");
        Assert.That(
            TitleFromSidebarMenuElement("Structure", "company"),
            Is.EqualTo("Тестовый холдинг"),
            "На странице Компании нет названия 'Тестовый холдинг'");
        Assert.That(
            TitleFromSidebarMenuElement("News", "news"),
            Is.EqualTo("Новости"),
            "На странице Новостей нет названия 'Новости'");
    }



    [Test]
    public void EventsFilter_test()
    {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/events"));

        _driver.FindElement(By.CssSelector("[data-tid='OnlyMyRadio']")).Click();
        _driver.FindElement(By.CssSelector("[data-tid='OnlineRadio']")).Click();

        _driver.FindElement(By.CssSelector("span[style='width: 100%;'] button")).Click();
        _driver.FindElements(By.CssSelector("[data-tid='ScrollContainer__inner'] button"))[1].Click();

        Assert.That(
            _driver.FindElements(By.XPath("//button[text()='СБРОСИТЬ']")).Count == 1,
            "Кнопка Сброса фильтров не появилась");
        Assert.That(
            _driver.FindElements(By.XPath("//span[text()='Применено']")).Count == 1,
            "Сообщение о количестве примененных фильтров не появилось");
    }

    private void CreateOnlineAllDayEvent(string name)
    {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/events"));

        _driver.FindElement(By.CssSelector("[data-tid='AddButton']")).Click();
        _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='ModalPageBody']")));

        _driver.FindElement(By.CssSelector("[data-tid='Name']")).SendKeys(name);
        _driver.FindElement(By.CssSelector("[placeholder='Введите ИНН']")).SendKeys("592010166264");
        _driver.FindElement(By.CssSelector("[data-tid='AllDay']")).Click();
        _driver.FindElement(By.CssSelector("[data-tid='IsOnline']")).Click();

        _driver.FindElement(By.CssSelector("[data-tid='CreateButton']")).Click();
    }

    [Test]
    public void EventCreation_test()
    {
        string name = new Random().NextInt64().ToString();
        CreateOnlineAllDayEvent(name);

        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/events"));

        Assert.That(
            _driver.FindElements(By.XPath($"//a[text()='{name}']")).Count > 0,
            "Мероприятие не появилось в списке");
    }

    [Test]
    public void EventDeletion_test()
    {
        string name = new Random().NextInt64().ToString();
        CreateOnlineAllDayEvent(name);

        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/events"));

        _driver.FindElements(By.XPath($"//a[text()='{name}']"))[0].Click();
        _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Title']")));

        _driver.FindElements(By.CssSelector("[data-tid='DropdownButton']"))[1].Click();
        _driver.FindElements(By.CssSelector("[data-tid='ScrollContainer__inner'] svg"))[2].Click();
        _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='DeleteButton']")));

        _driver.FindElement(By.CssSelector("[data-tid='DeleteButton']")).Click();
        _driver.FindElement(By.CssSelector("[data-tid='ModalPageFooter'] [data-tid='DeleteButton']")).Click();

        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/events"));

        Assert.That(
            _driver.FindElements(By.XPath($"//a[text()='{name}']")).Count == 0,
            "Мероприятие не было удалено");
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
