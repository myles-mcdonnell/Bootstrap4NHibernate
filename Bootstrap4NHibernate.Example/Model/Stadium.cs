using System.Collections.Generic;

namespace Bootstrap4NHibernate.Example.Model
{
    public class Stadium
    {
        public Stadium()
        {
            Teams = new List<Team>();
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Team> Teams { get; set; }

        public virtual Team AddTeam(Team team)
        {
            Teams.Add(team);
            team.Stadium = this;

            return team;
        }
    } 
}
