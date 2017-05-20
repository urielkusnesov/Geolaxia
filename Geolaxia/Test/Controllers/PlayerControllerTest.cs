using Geolaxia.Controllers;
using Moq;
using NUnit.Framework;
using Service.Planets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
