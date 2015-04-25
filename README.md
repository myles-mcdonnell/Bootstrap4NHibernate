#Bootstrap 4 NHibernate

A library for defining data to be inserted into a database via nhibernate in code.

Given a simple schema with three tables/entities:

Team
Player
Stadium

Related like so

Teams have many Players
Stadiums have many Teams
Teams have one Stadium
Players have one Team

once we have created the model and class maps correctly we can create DataFixture classes

<pre>

public class TeamFixture : Bootstrap4NHibernate.Data.DataFixture
    {
        public readonly Team Jets = new Team {Name = "Jets"};
        public readonly Team Falcons = new Team { Name = "Falcons" };
        public readonly Team Donkeys = new Team { Name = "Donkeys" };

        public override object[] GetEntities(Bootstrap4NHibernate.Data.IFixtureContainer fixtureContainer)
        {
            return new[]{Jets, Falcons, Donkeys};
        }
    }

public class PlayerFixture : Bootstrap4NHibernate.Data.DataFixture
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

        public override object[] GetEntities(Bootstrap4NHibernate.Data.IFixtureContainer fixtureContainer)
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
	
	</pre>

	Then we can create a <pre>Bootstrap4NHibernate.Database</pre> passing the database connection, class map assembly and data fixture assemblies and have the database class
	create the schema (optional) and populate the data:

	<pre>

	static void Main(string[] args)
        {
            var dbConf = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c
                .Database("Bootstrap4NHibernate")
                .Host("localhost")
                .Port(5432)
                .Username("postgres")
                .Password(""));

            var database = new Database(dbConf, Assembly.GetExecutingAssembly(), true);

            database.Populate(Assembly.GetExecutingAssembly());

            var sessionFactory = Fluently.Configure()
                .Database(() => dbConf)
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                foreach (var team in session.Query<Team>())
                {
                    Console.WriteLine("Team: " + team.Name);
                    Console.WriteLine("Stadium: " + team.Stadium.Name);
                    Console.WriteLine("Players: ");
                    foreach (var player in team.Players)
                    {
                        Console.WriteLine(player.Name);
                    }
                    Console.WriteLine();
                }
            }

            Console.ReadLine();
        }

	</pre>

	The Database class uses <a href="https://github.com/myles-mcdonnell/MPM.PDAG">https://github.com/myles-mcdonnell/MPM.PDAG</a> to determine the maximum level
	of concurrency when inserting the data.  Taking the above example Teams will be created first then Stadiums and Players will be created concurrently.  This can decrease
	data creation time significantly for complex/large schemas.

	Data is inserted in a single transaction, so it's all or nothing.

	Also note that <pre>DataFixture</pre> classes can get references to other DataFixtures and in this way can reference other entities without querying the database
	which is also a significant performance boost.

	See the example console application in the repo for a working exmaple.  This uses postgress but can be changed to any NHiberante supported DB.


