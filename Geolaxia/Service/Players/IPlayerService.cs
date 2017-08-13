using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Linq.Expressions;

namespace Service.Players
{
    public interface IPlayerService
    {
        Player Get(int id);

        Player GetByUsername(string username);

        Player GetByFacebookId(string userId);

        IList<Player> List(Expression<Func<Player, bool>> filter = null);

        Player Add(Player player);

        Player Update(int id, Player player);

        Player Remove(Player player);

        Player Remove(int id);

        bool ValidateToken(string username, string token);
    }
}
