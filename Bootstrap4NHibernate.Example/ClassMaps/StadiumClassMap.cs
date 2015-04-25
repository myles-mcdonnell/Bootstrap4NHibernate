using Bootstrap4NHibernate.Example.Model;
using FluentNHibernate.Mapping;

namespace Bootstrap4NHibernate.Example.ClassMaps
{
    public class StadiumClassMap : ClassMap<Stadium> 
    {
        public StadiumClassMap()
        {
            Id(stadium => stadium.Id);
            Map(stadium => stadium.Name);
            HasMany(stadium => stadium.Teams);
        }
    }
}
