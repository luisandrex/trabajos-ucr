using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace PRIME_UCR.Test.SeleniumTests.Dashboard
{
    public class IncidentTabDashboardTests : IDisposable
    {
        private IWebDriver driver;
        private string url = "https://localhost:44368/";

        [Fact]
        [Obsolete]
        public void NoDashboardPermissionTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            Thread.Sleep(3000);

            Login(false);            

            Thread.Sleep(2000);

            EnterToDashboardTab();

            Thread.Sleep(3000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/p")));

            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/p")).Text;

            Assert.Equal("Para acceder a esta página debe contar con el permiso de ver información de incidentes en el dashboard.",output);
        }
        [Fact]
        [Obsolete]
        public void HasDashboardPermissionTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            Thread.Sleep(1000);

            Login(true);

            Thread.Sleep(2000);

            EnterToDashboardTab();

            Thread.Sleep(5000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[1]")));


            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[1]")).Text;

            Assert.Equal("Filtros", output);
        }

        [Fact]
        [Obsolete]
        public void SwitchCounterFiltersTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            Thread.Sleep(1000);

            Login(true);

            Thread.Sleep(2000);

            EnterToDashboardTab();

            Thread.Sleep(5000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[1]/div/div/form/div/select")));

            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[1]/div/div/form/div/select")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[1]/div/div/form/div/select/option[2]")));

            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[1]/div/div/form/div/select/option[2]")).Click();

            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[1]/div/div/form/div/select/option[2]")));

            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[1]/div/div/form/div/select/option[2]")).Selected;

            Assert.True(output);
        }

        [Fact]
        [Obsolete]
        public void ApplyModalityFilterTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            Thread.Sleep(1000);

            Login(true);

            Thread.Sleep(2000);

            EnterToDashboardTab();

            Thread.Sleep(5000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[4]/div/div/div[1]/h4/a")));

            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[4]/div/div/div[1]/h4/a")).Click();

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse4\"]/form/div[1]/select")));

            driver.FindElement(By.XPath("//*[@id=\"collapse4\"]/form/div[1]/select")).Click();
            
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse4\"]/form/div[1]/select/option[2]")));
            driver.FindElement(By.XPath("//*[@id=\"collapse4\"]/form/div[1]/select/option[2]")).Click();
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse4\"]/form/div[2]/button[1]")));

            driver.FindElement(By.XPath("//*[@id=\"collapse4\"]/form/div[2]/button[1]")).Click();

            Thread.Sleep(5000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button")));

            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button")).Text;

            Assert.Equal("Modalidad: Aéreo ×", output);
        }

        [Fact]
        [Obsolete]
        public void ApplyStateFilterTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            Thread.Sleep(1000);

            Login(true);

            Thread.Sleep(2000);

            EnterToDashboardTab();

            Thread.Sleep(5000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[5]/div/div/div[1]/h4/a")));

            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[5]/div/div/div[1]/h4/a")).Click();

            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse7\"]/form/div[1]/select")));

            driver.FindElement(By.XPath("//*[@id=\"collapse7\"]/form/div[1]/select")).Click();
            
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse7\"]/form/div[1]/select/option[6]")));

            driver.FindElement(By.XPath("//*[@id=\"collapse7\"]/form/div[1]/select/option[6]")).Click();
            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse7\"]/form/div[1]/select/option[6]")));

            driver.FindElement(By.XPath("//*[@id=\"collapse7\"]/form/div[1]/select/option[6]")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse7\"]/form/div[2]/button[1]")));

            driver.FindElement(By.XPath("//*[@id=\"collapse7\"]/form/div[2]/button[1]")).Click();

            Thread.Sleep(5000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button")));

            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button")).Text;

            Assert.Equal("Estado: En proceso de creación ×", output);
        }

        [Fact]
        [Obsolete]
        public void ApplyDestinationFilterTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            Thread.Sleep(1000);

            Login(true);

            Thread.Sleep(2000);

            EnterToDashboardTab();

            Thread.Sleep(5000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[2]/div/div/div[1]/h4/a")));

            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[2]/div/div/div[1]/h4/a")).Click();

            Thread.Sleep(3000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse2\"]/form/div[1]/select")));

            driver.FindElement(By.XPath("//*[@id=\"collapse2\"]/form/div[1]/select")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse2\"]/form/div[1]/select/option[3]")));
            driver.FindElement(By.XPath("//*[@id=\"collapse2\"]/form/div[1]/select/option[3]")).Click();
            Thread.Sleep(3000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse2\"]/form/div[2]/button[1]")));

            driver.FindElement(By.XPath("//*[@id=\"collapse2\"]/form/div[2]/button[1]")).Click();

            Thread.Sleep(5000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button")));

            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button")).Text;

            Assert.Equal("Centro médico destino: Hospital México ×", output);
        }

        [Fact]
        [Obsolete]
        public void ApplyOriginFilterTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            Thread.Sleep(1000);

            Login(true);

            Thread.Sleep(2000);

            EnterToDashboardTab();

            Thread.Sleep(5000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[1]/div/div/div[1]/h4/a")));

            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[1]/div/div/div[1]/h4/a")).Click();

            Thread.Sleep(3000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse5\"]/form/div[1]/select")));

            driver.FindElement(By.XPath("//*[@id=\"collapse5\"]/form/div[1]/select")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse5\"]/form/div[1]/select/option[2]")));

            driver.FindElement(By.XPath("//*[@id=\"collapse5\"]/form/div[1]/select/option[2]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.XPath("//*[@id=\"collapse5\"]/form/div[3]/button[1]")).Click();

            Thread.Sleep(5000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button")));


            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button")).Text;

            Assert.Equal("Origen: Domicilio ×", output);
        }
        [Fact]
        [Obsolete]
        public void ApplyDateFilterTest()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Maximize();

            driver.Url = url;

            Thread.Sleep(1000);

            Login(true);

            Thread.Sleep(2000);

            EnterToDashboardTab();

            Thread.Sleep(5000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[3]/div/div/div[1]/h4/a")));

            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[2]/div[3]/div/div/div[1]/h4/a")).Click();

            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse1\"]/form/div[1]/input")));

            driver.FindElement(By.XPath("//*[@id=\"collapse1\"]/form/div[1]/input")).SendKeys("01/01/2020");
            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse1\"]/form/div[2]/input")));

            driver.FindElement(By.XPath("//*[@id=\"collapse1\"]/form/div[2]/input")).SendKeys("02/02/2020");
            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapse1\"]/form/div[3]/button[1]")));

            driver.FindElement(By.XPath("//*[@id=\"collapse1\"]/form/div[3]/button[1]")).Click();

            Thread.Sleep(5000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button[1]")));

            var initialOutput = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button[1]")).Text;

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button[2]")));

            var finalOutput = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div[2]/div[2]/div[2]/div/div[1]/div[2]/button[2]")).Text;

            var output = (initialOutput == "Fecha inicial: 01/01/2020 ×") && (finalOutput == "Fecha final: 02/02/2020 ×");

            Assert.True(output);
        }


        private void Login(bool hasPermission)
        {
            if (hasPermission) 
            {

                driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")).SendKeys("teodoro.barquero@prime.com");
                driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div/input")).SendKeys("Teodoro.Barquero10");
            }
            else
            {
                driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")).SendKeys("juan.guzman@prime.com");
                driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div/input")).SendKeys("Juan.Guzman10");
            }
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")).Click();
        }

        private void EnterToDashboardTab() 
        {
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[1]/a")).Click();
        }


        public void Dispose()
        {
            driver.Dispose();
        }

    }
}
