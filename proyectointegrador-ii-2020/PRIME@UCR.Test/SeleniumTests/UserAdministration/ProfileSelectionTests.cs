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
    public class ProfileSelectionTests 
    {
        private IWebDriver driver;
        private string url = "https://localhost:44368/";
        private double timeout = 0.5;

        [Fact]
        [Obsolete]
        public void CreateNewUser()
        {
            driver = new ChromeDriver();

            driver.Url = url;

            driver.Manage().Window.Maximize();

            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));

            Login(webDriverWait);

            Thread.Sleep(5000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[7]/a")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[7]/a")).Click();

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/div/div[1]/ul/li[2]/a")));
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[1]/ul/li[2]/a")).Click();

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[1]/div[1]/div/div/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[1]/div[1]/div/div/input")).SendKeys("234123411");
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[1]/div[2]/div/div/input")).SendKeys("Luis");
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[1]/div[3]/div[1]/div/input")).SendKeys("Morales");
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[1]/div[4]/div/div/input")).SendKeys("27/12/2000");
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[2]/div[1]/div/div/input")).SendKeys("luis.sanchez@prime.com");
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[2]/div[2]/div[1]/div/input")).SendKeys("84312743");

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[3]/div/table/tbody/tr[1]/td[1]/center/div")));
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/div[3]/div/table/tbody/tr[1]/td[1]/center/div")).Click();

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/button")));
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/form/button")).Click();

            Thread.Sleep(5000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/div/div[2]/div")));
            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/div")).Text;
            var expected = "El usuario indicado se ha registrado en la aplicación y se le ha enviado un correo de validación de cuenta.";
            
            Assert.Equal(expected,output);
        }


        [Fact]
        [Obsolete]
        public void resendConfirmationEmail()
        {
            driver = new ChromeDriver();

            driver.Url = url;
            
            driver.Manage().Window.Maximize();


            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));
            Login(webDriverWait);

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[7]/a")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/ul/li[7]/a")).Click();

            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/div/div[1]/ul/li[3]/a")));
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[1]/ul/li[3]/a")).Click();

            Thread.Sleep(3000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/div/div[2]/div/table/tbody/tr[1]/td[1]/center/button")));
            driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/div/table/tbody/tr[1]/td[1]/center/button")).Click();
            
            Thread.Sleep(5000);
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/main/div/div/div[2]/div[1]")));
            var output = driver.FindElement(By.XPath("/html/body/app/div/div/main/div/div/div[2]/div[1]")).Text;
            var expected = "Se ha reenviado un correo de validación de cuenta al usuario indicado.";
            
            Assert.Equal(expected, output);
        }

        [Obsolete]
        private void Login(WebDriverWait webDriverWait)
        {
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[1]/input")).SendKeys("juan.guzman@prime.com");
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[2]/div/input")).SendKeys("Juan.Guzman10");
            webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")));
            driver.FindElement(By.XPath("/html/body/app/div/div/aside/nav/div/form/div[5]/button")).Click();
        }


    }
}
