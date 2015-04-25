using System;
using Bootstrap4NHibernate.Data;
using Bootstrap4NHibernate.Example.Model;

namespace Bootstrap4NHibernate.Example.DataFixtures
{
    public class PlayerFixture : DataFixture
    {
        public readonly Player JoeBloggs = new Player {Name = "Joe Bloggs"};
        public readonly Player FredSmith = new Player { Name = "Fred Smith" };
        public readonly Player DimTim = new Player { Name = "Dim Tim" };
        public readonly Player FastEddy = new Player { Name = "Fast Eddy" };
        public readonly Player SpongeBob = new Player { Name = "Sponge Bob" };
        public readonly Player FatherTed = new Player { Name = "Father Ted" };

        public override Type[] Dependencies
        {
            get { return new[] { typeof(TeamFixture) }; }
        }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var teamFixture = fixtureContainer.Get<TeamFixture>();

            teamFixture.Donkeys.AddPlayer(FatherTed);
            teamFixture.Donkeys.AddPlayer(SpongeBob);
            teamFixture.Falcons.AddPlayer(FastEddy);
            teamFixture.Falcons.AddPlayer(DimTim);
            teamFixture.Jets.AddPlayer(FredSmith);
            teamFixture.Jets.AddPlayer(JoeBloggs);

            return new[]
            {
                JoeBloggs,
                FredSmith,
                DimTim,
                FastEddy,
                SpongeBob,
                FatherTed
            };
        }
    }
}
