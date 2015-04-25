using Bootstrap4NHibernate.Example.Model;
using FluentNHibernate.Mapping;

namespace Bootstrap4NHibernate.Example.ClassMaps
{
    public class PlayerClassMap : ClassMap<Player>
    {
        public PlayerClassMap()
        {
            Id(player => player.Id);
            Map(player => player.Name);
            References(player => player.Team);
        }
    }
}
