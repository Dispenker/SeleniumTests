using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace practic_tests;

public class SideBarMenuTests : Tests
{
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
    public void SidebarMenuCommentsTest()
    {
        Assert.That(
            TitleFromSidebarMenuElement("Comments", "comments"),
            Is.EqualTo("Комментарии"),
            "На странице Комментариев нет названия 'Комментарии'");
    }

    [Test]
    public void SidebarMenuMessagesTest()
    {
        Assert.That(
            TitleFromSidebarMenuElement("Messages", "messages"),
            Is.EqualTo("Диалоги"),
            "На странице Диалогов нет названия 'Диалоги'");
    }

    [Test]
    public void SidebarMenuCommunityTest()
    {
        Assert.That(
            TitleFromSidebarMenuElement("Community", "communities"),
            Is.EqualTo("Сообщества"),
            "На странице Сообщества нет названия 'Сообщества'");
    }

    [Test]
    public void SidebarMenuEventsTest()
    {
        Assert.That(
            TitleFromSidebarMenuElement("Events", "events", "[data-tid='Actual']"),
            Is.EqualTo("Актуальные"),
            "На странице Мероприятий нет вкладки 'Актуальные'");
    }

    [Test]
    public void SidebarMenuFilesTest()
    {
        Assert.That(
            TitleFromSidebarMenuElement("Files", "files"),
            Is.EqualTo("Файлы"),
            "На странице Файлов нет названия 'Файлы'");
    }

    [Test]
    public void SidebarMenuDocumentsTest()
    {
        Assert.That(
            TitleFromSidebarMenuElement("Documents", "documents"),
            Is.EqualTo("Документы"),
            "На странице Документов нет названия 'Документы'");
    }

    [Test]
    public void SidebarMenuStructureTest()
    {
        Assert.That(
            TitleFromSidebarMenuElement("Structure", "company"),
            Is.EqualTo("Тестовый холдинг"),
            "На странице Компании нет названия 'Тестовый холдинг'");
    }

    [Test]
    public void SidebarMenuNewsTest()
    {
        // Перемещаемся, чтобы Новости не были выбранными        
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/events"));

        Assert.That(
            TitleFromSidebarMenuElement("News", "news"),
            Is.EqualTo("Новости"),
            "На странице Новостей нет названия 'Новости'");
    }
}
