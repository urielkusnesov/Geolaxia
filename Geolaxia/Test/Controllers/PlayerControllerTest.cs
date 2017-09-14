using Geolaxia.Controllers;
using Moq;
using NUnit.Framework;
using Service.Planets;
using Service.Players;

namespace Test.Controllers
{
    [TestFixture]
    public class PlayerControllerTest
    {
        private PlayerController target;
        private Mock<IPlayerService> servcie;
        private Mock<IPlanetService> planetService;

        [SetUp]
        public void SetUp()
        {
            servcie = new Mock<IPlayerService>();
            target = new PlayerController(servcie.Object, planetService.Object);
        }
    }
}
