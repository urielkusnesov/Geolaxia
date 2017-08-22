using Geolaxia.Controllers;
using Moq;
using NUnit.Framework;

namespace Test.Controllers
{
    [TestFixture]
    public class PlayerControllerTest
    {
        private PlayerController target;
        private Mock<IPlayerService> servcie;

        [SetUp]
        public void SetUp()
        {
            servcie = new Mock<IPlayerService>();
            target = new PlayerController(servcie.Object);
        }
    }
}
