using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.SeleniumTests.Checklists
{
    public class CheckListServiceSelenium : IDisposable
    {
        IWebDriver driver;
        private string url = "https://localhost:44368/";
        private string CheckListUrl = "https://localhost:44368/checklist";
        public void Dispose()
        {
            driver.Dispose();
        }

        [Fact]
        public void InsertTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = url;
            LogIn();
            Thread.Sleep(5000);
            driver.FindElement(By.LinkText("Listas de chequeo")).Click();
            Thread.Sleep(5000);
            Assert.Equal("Salida de Paciente de la Unidad de Internamiento", driver.FindElement(By.LinkText("Salida de Paciente de la Unidad de Internamiento")).Text);
            driver.Url = "https://localhost:44368/checklist/create";
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//input[@type='text']")).SendKeys("Pruebas selenium");
            driver.FindElement(By.XPath("//div[@id='contAll_LC']/form/div[2]/textarea")).SendKeys("Realizando pruebas de selenium");
            driver.FindElement(By.XPath("//div[@id='contAll_LC']/form/div[3]/div/div/select/option[3]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//div[5]/button")).Click();
            Thread.Sleep(5000);
            Assert.Equal("Pruebas selenium", driver.FindElement(By.XPath("//div[@id='def_LC']/div/div/div/input")).GetAttribute("value"));
            Assert.Equal("Realizando pruebas de selenium", driver.FindElement(By.XPath("//div[@id='def_LC']/div[2]/div/div/textarea")).GetAttribute("value"));
        }

        private void LogIn()
        {         
            Thread.Sleep(5000);
            
            driver.FindElement(By.XPath("/html/body/app/div/div/form/div[1]/input")).SendKeys("teodoro.barquero@prime.com");
            driver.FindElement(By.XPath("/html/body/app/div/div/form/div[2]/div/input")).SendKeys("Teodoro.Barquero10");
            driver.FindElement(By.XPath("/html/body/app/div/div/form/div[5]/button")).Click();
        }

        [Fact]
        public void InsertItemTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = url;
            LogIn();
            Thread.Sleep(5000);
            driver.FindElement(By.LinkText("Listas de chequeo")).Click();
            Thread.Sleep(5000);
            Assert.Equal("Salida de Paciente de la Unidad de Internamiento", driver.FindElement(By.LinkText("Salida de Paciente de la Unidad de Internamiento")).Text);
            driver.FindElement(By.LinkText("Salida de Paciente de la Unidad de Internamiento")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//div[2]/div/div[3]/button")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//div[@id='Create_I-SI_LC']/form/div/input")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//div[@id='Create_I-SI_LC']/form/div/input")).SendKeys("Pruebas selenium item");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//div[5]/button")).Click();
            Thread.Sleep(5000);
            Assert.Equal("Pruebas selenium item", driver.FindElement(By.XPath("//th[contains(.,'Pruebas selenium item')]")).Text);
        }
    }
}
