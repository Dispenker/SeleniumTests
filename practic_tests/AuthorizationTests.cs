using OpenQA.Selenium;

namespace practic_tests;

public class AuthorizationTests : Tests
{
    [Test]
    public void AuthorizationTest()
    {
        Assert.That(
            _driver.FindElement(By.CssSelector("[data-tid='Title']")).Text,
            Is.EqualTo("Новости"),
            "На странице Новостей нет названия 'Новостu'"
        );
    }
}
