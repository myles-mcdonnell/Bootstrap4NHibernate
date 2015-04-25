using System;
using Bootstrap4NHibernate.Example.Model;
using FluentNHibernate.Mapping;

namespace Bootstrap4NHibernate.Example.ClassMaps
{
    public class TeamClassMap : ClassMap<Team>
    {
        public TeamClassMap()
        {
            Id(team => team.Id);
            Map(team => team.Name);
            References(team => team.Stadium);
            HasMany(team => team.Players);
        }
    }
}
