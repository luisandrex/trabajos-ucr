using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.SeleniumTests.UserAdministration
{
    public class AuthenticationTests : IDisposable
    {
        private IWebDriver driver;
        private string url = "https://localhost:44368/";
        private double timeout = 0.5;

        [Fact]
        [Obsolete]
        public void AuthenticationNoPasswordTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")).SendKeys("test@test.com\n");

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[2]")));
            var output = driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[2]")).Text;
            Assert.Equal("Digite su contraseña", output);
        }

        [Fact]
        [Obsolete]
        public void AuthenticationNoEmailTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = url;

            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[1]/input")).SendKeys("Test");
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/div")));
            var output = driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/div")).Text;
            Assert.Equal("Digite su correo", output);
        }

        [Fact]
        [Obsolete]
        public void AuthenticationFailedTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = "https://localhost:44368/";

            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));
           
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")).SendKeys("test@test.com");

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[1]/input")).SendKeys("Test");

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")).Click();

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[3]/p")));
            var output = driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[3]/p")).Text;

            Assert.Equal("Ingresó su usuario o contraseña incorrectamente. Intente de nuevo.", output);
        }

        [Fact]
        [Obsolete]
        public void AuthenticationSuccessTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = "https://localhost:44368/";

            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")).SendKeys("juan.guzman@prime.com");

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[1]/input")).SendKeys("Juan.Guzman10");

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")).Click();

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[1]/a")));
            var output = driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[1]/a")).Text;
            Assert.Equal("Dashboard", output);
        }

        [Obsolete]
        private void Authenticate(WebDriverWait webDriverWait)
        {
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")).SendKeys("juan.guzman@prime.com");

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div[1]/input")).SendKeys("Juan.Guzman10");

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")).Click();
        }

        [Fact]
        [Obsolete]
        public void LogOutCancel()
        {
            driver = new ChromeDriver();

            driver.Url = "https://localhost:44368/";

            driver.Manage().Window.Maximize();

            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));
            
            Authenticate(webDriverWait);

            Thread.Sleep(3000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/header/div[2]/div[2]/div/a")));
            driver.FindElement(By.XPath("/html/body/header/div[2]/div[2]/div/a")).Click();

            Thread.Sleep(2000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/header/div[3]/div/div/div[3]/button[2]")));
            driver.FindElement(By.XPath("/html/body/header/div[3]/div/div/div[3]/button[2]")).Click();

            Thread.Sleep(3000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[1]/a")));
            var output = driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[1]/a")).Text;

            Assert.Equal("Dashboard", output);
        }

        [Fact]
        [Obsolete]
        public void LogOutSuccess()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = "https://localhost:44368/";

            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));

            Authenticate(webDriverWait);
            
            Thread.Sleep(3000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/header/div[2]/div[2]/div/a")));
            driver.FindElement(By.XPath("/html/body/header/div[2]/div[2]/div/a")).Click();

            Thread.Sleep(2000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Confirmar")));
            driver.FindElement(By.Id("Confirmar")).Click();

            Thread.Sleep(3000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/b")));
            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/b")).Text;
            Assert.Equal("Debe de registrarse antes de acceder a la página.", output);
        }

        public void Dispose()
        {
            driver.Dispose();
        }

    }
}
