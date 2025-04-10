using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace practic_tests;

public class EventsTests : Tests
{
    [Test]
    public void EventsFilterTest()
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
    public void EventCreationTest()
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
    public void EventDeletionTest()
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
}
