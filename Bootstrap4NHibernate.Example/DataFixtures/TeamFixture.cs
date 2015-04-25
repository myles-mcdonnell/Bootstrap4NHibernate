using Bootstrap4NHibernate.Example.Model;
using Bootstrap4NHibernate.Data;

namespace Bootstrap4NHibernate.Example.DataFixtures
{
    public class TeamFixture : DataFixture
    {
        public readonly Team Jets = new Team {Name = "Jets"};
        public readonly Team Falcons = new Team { Name = "Falcons" };
        public readonly Team Donkeys = new Team { Name = "Donkeys" };

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            return new[]{Jets, Falcons, Donkeys};
        }
    }
}
