using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Service.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepositoryService repository;

        public PlayerService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public Player Get(int id)
        {
            return repository.Get<Player>(id);
        }

        public Player GetByUsername(string username)
        {
            return repository.Get<Player>(x => x.UserName == username);
        }

        public Player GetByFacebookId(string userId)
        {
            return repository.Get<Player>(x => x.FacebookId == userId);
        }

        public IList<Player> List(Expression<Func<Player, bool>> filter = null)
        {
            return repository.List<Player>(filter);
        }

        public Player Add(Player player)
        {
            if (!repository.Exists<Player>(x => x.UserName == player.UserName))
            {
                var result = repository.Add<Player>(player);
                repository.SaveChanges();
                return result;
            }
            return null;
        }

        public Player Update(int id, Player player)
        {
            var oldplayer = repository.Get<Player>(id);
            if (oldplayer != null)
            {
                oldplayer = player;
                repository.SaveChanges();
                return player;                
            }
            return null;
        }

        public Player Remove(Player player)
        {
            var result = repository.Remove<Player>(player);
            repository.SaveChanges();
            return result;
        }

        public Player Remove(int id)
        {
            var result = repository.Remove<Player>(id);
            repository.SaveChanges();
            return result;
        }

        public bool ValidateToken(string username, string token) 
        {
            return repository.Exists<Player>(x => x.UserName == username && x.Token == token);
        }

        public bool SetPosition(string latitude, string longitude, int id)
        {
            try
            {
                var player = repository.Get<Player>(id);
                player.lastLatitude = latitude;
                player.lastLongitude = longitude;
                repository.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
