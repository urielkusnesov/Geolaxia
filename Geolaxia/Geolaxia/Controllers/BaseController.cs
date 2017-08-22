using log4net;
using Service.Players;
using System.Linq;
using System.Web.Http;

namespace Geolaxia.Controllers
{
    public class BaseController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PlanetController));
        private IPlayerService playerService;

        public BaseController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }

        protected bool ValidateToken()
        {
            string username = "";
            string token = "";
            var request = Request;
            var headers = request.Headers;
            if (!headers.Contains("username") || !headers.Contains("token"))
            {
                return false;
            }
            username = headers.GetValues("username").First();
            token = headers.GetValues("token").First();

            logger.Info("getting planets for player " + username);

            return playerService.ValidateToken(username, token);
        }
    }
}
