using System;
using System.Reflection;
using Bootstrap4NHibernate.Example.Model;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Linq;

namespace Bootstrap4NHibernate.Example
{
    class Program
    {
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
    }
}
