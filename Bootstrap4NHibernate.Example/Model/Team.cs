using System;
using System.Collections.Generic;

namespace Bootstrap4NHibernate.Example.Model
{
    public class Team
    {
        public Team()
        {
            Players = new List<Player>();
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Player> Players { get; set; }

        public virtual Stadium Stadium { get; set; }

        public virtual Player AddPlayer(Player player)
        {
            Players.Add(player);
            player.Team = this;

            return player;
        }
    }
}
