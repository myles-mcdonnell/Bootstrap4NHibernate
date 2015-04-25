using System;
using Bootstrap4NHibernate.Example.Model;

namespace Bootstrap4NHibernate.Example.DataFixtures
{
    public class StadiumFixture : Bootstrap4NHibernate.Data.DataFixture
    {
        public readonly Stadium MegaBowl = new Stadium {Name = "Mega Bowl"};
        public readonly Stadium MuddyField = new Stadium {Name = "Muddy Field"};

        public override Type[] Dependencies
        {
            get { return new []{typeof(TeamFixture)}; }
        }

        public override object[] GetEntities(Bootstrap4NHibernate.Data.IFixtureContainer fixtureContainer)
        {
            var teamFixture = fixtureContainer.Get<TeamFixture>();

            MegaBowl.AddTeam(teamFixture.Jets);
            MuddyField.AddTeam(teamFixture.Falcons);
            MuddyField.AddTeam(teamFixture.Donkeys);

            return new []
            {
                MegaBowl,
                MuddyField
            };
        }
    }
}
