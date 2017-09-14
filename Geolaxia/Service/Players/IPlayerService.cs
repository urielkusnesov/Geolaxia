using System;
using System.Collections.Generic;
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

        bool SetPosition(string latitude, string longitude, int id);

        IList<Player> GetCloserPlayers(string username);
    }
}
