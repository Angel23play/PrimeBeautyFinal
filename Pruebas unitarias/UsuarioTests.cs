using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI; // para WebDriverWait y SelectElement
using System;

namespace Pruebas_unitarias.PrimeBeautyMVC.Tests
{
    [TestFixture]
    public class UsuarioTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string baseUrl = "https://localhost:7265";

        // Guardamos el email para luego buscarlo en editar/eliminar
        private static string createdTestUserEmail = "";

        [SetUp]
        public void Setup()
        {
            var options = new EdgeOptions();
            driver = new EdgeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        // ===== LOGIN =====

        [Test]
        public void Login_CaminoFeliz()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Login/Login");

            driver.FindElement(By.Id("Email")).SendKeys("ikq@gmail.com");
            driver.FindElement(By.Id("Contrasena")).SendKeys("123456"); // Sin tilde
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.Url.Contains("/Home/Index"));
            ClassicAssert.IsTrue(driver.Url.Contains("/Home/Index"), "No redirigió a Home después del login exitoso.");
        }

        [Test]
        public void Login_PruebaNegativa_ContrasenaIncorrecta()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Login/Login");

            driver.FindElement(By.Id("Email")).SendKeys("ikq@gmail.com");
            driver.FindElement(By.Id("Contrasena")).SendKeys("password_incorrecta");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.FindElement(By.ClassName("alert-danger")).Displayed);
            var error = driver.FindElement(By.ClassName("alert-danger")).Text;
            ClassicAssert.AreEqual("Usuario o contraseña incorrectos.", error);
        }

        [Test]
        public void Login_PruebaLimite_CamposVacios()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Login/Login");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.FindElement(By.ClassName("alert-danger")).Displayed);
            var error = driver.FindElement(By.ClassName("alert-danger")).Text;
            ClassicAssert.AreEqual("Debe ingresar usuario y contraseña.", error);
        }

        // ===== CREATE USUARIO =====

        [Test]
        public void CrearUsuario_CaminoFeliz()
        {
            createdTestUserEmail = $"testuser{DateTime.Now.Ticks}@correo.com";

            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Create");

            driver.FindElement(By.Id("Nombre")).SendKeys("Test");
            driver.FindElement(By.Id("Apellido")).SendKeys("User");
            driver.FindElement(By.Id("Email")).SendKeys(createdTestUserEmail);
            driver.FindElement(By.Id("Contrasena")).SendKeys("Test123!");
            driver.FindElement(By.Id("Telefono")).SendKeys("8095551234");

            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByValue("2"); // Cliente

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.Url.Contains("/Usuario/Index"));

            ClassicAssert.IsTrue(driver.PageSource.Contains(createdTestUserEmail), "Nuevo usuario no encontrado en la lista.");
        }

        [Test]
        public void CrearUsuario_PruebaNegativa_EmailVacio()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Create");

            driver.FindElement(By.Id("Nombre")).SendKeys("Test");
            driver.FindElement(By.Id("Apellido")).SendKeys("User");
            driver.FindElement(By.Id("Contrasena")).SendKeys("Test123!");
            driver.FindElement(By.Id("Telefono")).SendKeys("8095551234");

            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByValue("2"); // Cliente

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.FindElement(By.CssSelector("span.text-danger")).Displayed);
            var error = driver.FindElement(By.CssSelector("span.text-danger")).Text;
            ClassicAssert.IsTrue(error.Length > 0, "No se mostró error por email vacío");
        }

        [Test]
        public void CrearUsuario_PruebaLimite_EmailMuyLargo()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Create");

            driver.FindElement(By.Id("Nombre")).SendKeys("Test");
            driver.FindElement(By.Id("Apellido")).SendKeys("User");

            var longEmail = new string('a', 256) + "@correo.com";
            driver.FindElement(By.Id("Email")).SendKeys(longEmail);
            driver.FindElement(By.Id("Contrasena")).SendKeys("Test123!");
            driver.FindElement(By.Id("Telefono")).SendKeys("8095551234");

            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByValue("2"); // Cliente

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.FindElement(By.CssSelector("span.text-danger")).Displayed);
            var error = driver.FindElement(By.CssSelector("span.text-danger")).Text;
            ClassicAssert.IsTrue(error.Length > 0, "No se detectó error en email demasiado largo");
        }

        // ===== UPDATE USUARIO =====

        [Test]
        public void EditarUsuario_CaminoFeliz()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Index");

            var editLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{createdTestUserEmail}')]]//a[contains(text(),'Editar')]"));
            editLink.Click();

            var nombreInput = driver.FindElement(By.Id("Nombre"));
            nombreInput.Clear();
            nombreInput.SendKeys("Test Modificado");

            // Seleccionar (o mantener) el tipo requerido
            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByValue("2"); // Cliente

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.Url.Contains("/Usuario/Index"));

            ClassicAssert.IsTrue(driver.PageSource.Contains("Test Modificado"), "Nombre modificado no aparece en la lista.");
        }

        [Test]
        public void EditarUsuario_NombreVacio()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Index");

            var editLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{createdTestUserEmail}')]]//a[contains(text(),'Editar')]"));
            editLink.Click();

            var nombreInput = driver.FindElement(By.Id("Nombre"));
            nombreInput.Clear();

            // Asegurar tipo válido seleccionado
            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByValue("2"); // Cliente

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            ClassicAssert.IsTrue(driver.PageSource.Contains("El campo Nombre es obligatorio"),
                "No se mostró el mensaje de validación para nombre vacío.");
        }

        [Test]
        public void EditarUsuario_EmailDuplicado()
        {
            string existingEmail = "usuarioexistente@test.com"; // Debe existir previamente

            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Index");

            var editLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{createdTestUserEmail}')]]//a[contains(text(),'Editar')]"));
            editLink.Click();

            var emailInput = driver.FindElement(By.Id("Email"));
            emailInput.Clear();
            emailInput.SendKeys(existingEmail);

            // Asegurar tipo válido seleccionado
            var tipoSelect = new SelectElement(driver.FindElement(By.Id("Tipo")));
            tipoSelect.SelectByValue("2"); // Cliente

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            ClassicAssert.IsTrue(driver.PageSource.Contains("El correo ya está registrado"),
                "No se mostró el mensaje de error para correo duplicado.");
        }

        // ===== DELETE USUARIO =====

        [Test]
        public void EliminarUsuario_CaminoFeliz()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Index");

            var deleteLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{createdTestUserEmail}')]]//a[contains(text(),'Eliminar')]"));
            deleteLink.Click();

            driver.SwitchTo().Alert().Accept();

            wait.Until(d => !driver.PageSource.Contains(createdTestUserEmail));

            ClassicAssert.IsFalse(driver.PageSource.Contains(createdTestUserEmail), "Usuario no fue eliminado correctamente.");
        }

        [Test]
        public void EliminarUsuario_Cancelar()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Index");

            var deleteLink = driver.FindElement(By.XPath($"//tr[td[contains(text(),'{createdTestUserEmail}')]]//a[contains(text(),'Eliminar')]"));
            deleteLink.Click();

            driver.SwitchTo().Alert().Dismiss(); // Cancelar

            ClassicAssert.IsTrue(driver.PageSource.Contains(createdTestUserEmail),
                "Usuario fue eliminado aunque se canceló la acción.");
        }

        [Test]
        public void EliminarUsuario_Inexistente()
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Usuario/Eliminar/999999"); // ID que no existe

            ClassicAssert.IsTrue(driver.PageSource.Contains("Usuario no encontrado")
                || driver.Url.Contains("/Usuario/Index"),
                "No se manejó correctamente la eliminación de usuario inexistente.");
        }
    }
}
