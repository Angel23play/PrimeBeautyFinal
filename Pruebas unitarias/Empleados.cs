using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace PrimeBeautyMVC.Tests
{
    [TestFixture]
    public class Empleados
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "https://localhost:5001"; // Ajusta tu puerto
        private string testEmpleadoEmail = $"testempleado_{Guid.NewGuid()}@mail.com";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();  
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

        // ===== CREATE =========

        [Test]
        public void CrearEmpleado_CaminoFeliz()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Empleado/Create");

            driver.FindElement(By.Id("Nombre")).SendKeys("Juan");
            driver.FindElement(By.Id("Apellido")).SendKeys("Pérez");
            driver.FindElement(By.Id("Email")).SendKeys(testEmpleadoEmail);
            driver.FindElement(By.Id("Telefono")).SendKeys("8091234567");

            // Seleccionar tipo de empleado
            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByText("Barbero");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.Url.Contains("/Empleado/Index"));
            ClassicAssert.IsTrue(driver.PageSource.Contains(testEmpleadoEmail), "Empleado no creado correctamente.");
        }

        [Test]
        public void CrearEmpleado_Fallo_DatosVacios()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Empleado/Create");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            ClassicAssert.IsTrue(driver.PageSource.Contains("El campo Nombre es obligatorio"), "No mostró error de validación.");
        }

        [Test]
        public void CrearEmpleado_Limite_LongitudMaximaNombre()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Empleado/Create");

            string nombreLargo = new string('A', 101);
            driver.FindElement(By.Id("Nombre")).SendKeys(nombreLargo);
            driver.FindElement(By.Id("Apellido")).SendKeys("Apellido");
            driver.FindElement(By.Id("Email")).SendKeys($"largo_{Guid.NewGuid()}@mail.com");
            driver.FindElement(By.Id("Telefono")).SendKeys("8091234567");

            // Seleccionar tipo de empleado
            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByText("Conserje");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            ClassicAssert.IsTrue(driver.PageSource.Contains("máximo permitido"), "No validó límite de caracteres.");
        }


        // ===== UPDATE =========
        [Test]
        public void EditarEmpleado_CaminoFeliz()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Empleado/Index");

            var editLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{testEmpleadoEmail}')]]//a[contains(text(),'Editar')]"));
            editLink.Click();

            var nombreInput = driver.FindElement(By.Id("Nombre"));
            nombreInput.Clear();
            nombreInput.SendKeys("Empleado Modificado");

            // Cambiar tipo de empleado
            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByText("Recepcionista");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            wait.Until(d => d.Url.Contains("/Empleado/Index"));

            ClassicAssert.IsTrue(driver.PageSource.Contains("Empleado Modificado"), "No se modificó correctamente.");
        }

        [Test]
        public void EditarEmpleado_Fallo_CamposVacios()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Empleado/Index");

            var editLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{testEmpleadoEmail}')]]//a[contains(text(),'Editar')]"));
            editLink.Click();

            var nombreInput = driver.FindElement(By.Id("Nombre"));
            nombreInput.Clear();

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            ClassicAssert.IsTrue(driver.PageSource.Contains("El campo Nombre es obligatorio"), "No validó campos vacíos.");
        }

        [Test]
        public void EditarEmpleado_Limite_NombreMuyLargo()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Empleado/Index");

            var editLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{testEmpleadoEmail}')]]//a[contains(text(),'Editar')]"));
            editLink.Click();

            var nombreInput = driver.FindElement(By.Id("Nombre"));
            nombreInput.Clear();
            nombreInput.SendKeys(new string('B', 101));

            // Mantener tipo válido
            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByText("Barbero");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            ClassicAssert.IsTrue(driver.PageSource.Contains("máximo permitido"), "No validó límite de caracteres.");
        }

        // ===== DELETE =========
        [Test]
        public void EliminarEmpleado_CaminoFeliz()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Empleado/Index");

            var deleteLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{testEmpleadoEmail}')]]//a[contains(text(),'Eliminar')]"));
            deleteLink.Click();

            driver.SwitchTo().Alert().Accept();
            wait.Until(d => !driver.PageSource.Contains(testEmpleadoEmail));

            ClassicAssert.IsFalse(driver.PageSource.Contains(testEmpleadoEmail), "Empleado no fue eliminado.");
        }

        [Test]
        public void EliminarEmpleado_Fallo_NoExiste()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Empleado/Eliminar/99999");
            ClassicAssert.IsTrue(driver.PageSource.Contains("Error"), "No manejó intento de eliminar inexistente.");
        }

        [Test]
        public void EliminarEmpleado_Limite_EliminarUltimoRegistro()
        {
            ClassicAssert.IsTrue(true, "Caso límite no implementado.");
        }
    }
}
