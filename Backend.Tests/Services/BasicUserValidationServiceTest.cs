using System.Threading.Tasks;
using GitMonitor.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitMonitor.Services
{
    [TestClass]
    public class BasicUserValidationServiceTest
    {
        [TestMethod]
        public async Task IsValidAsync_Anonymous()
        {
            var options = new ApplicationOptions
            {
                Username = null,
                Password = null,
            };

            var service = new BasicUserValidationService(Options.Create(options));

            Assert.IsTrue(await service.IsValidAsync("username", "password"));
        }

        [TestMethod]
        public async Task IsValidAsync_Credentials()
        {
            var options = new ApplicationOptions
            {
                Username = "username",
                Password = "password",
            };

            var service = new BasicUserValidationService(Options.Create(options));

            Assert.IsTrue(await service.IsValidAsync("username", "password"));
            Assert.IsFalse(await service.IsValidAsync("wrong_username", "password"));
            Assert.IsFalse(await service.IsValidAsync("username", "wrong_password"));
            Assert.IsFalse(await service.IsValidAsync("wrong_username", "wrong_password"));
        }
    }
}