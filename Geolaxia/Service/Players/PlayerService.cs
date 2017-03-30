﻿using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
    }
}
