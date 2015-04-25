using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bootstrap4NHibernate.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MPM.PDAG;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Bootstrap4NHibernate
{
    public class Database
    {
        private readonly ISessionFactory _sessionFactory;

        public Database(IPersistenceConfigurer persistenceConfigurer, Assembly classMapAssembly,
            bool resetSchema = false)
        {
            _sessionFactory = Fluently.Configure()
                .Database(() => persistenceConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssembly(classMapAssembly))
                .ExposeConfiguration(config =>
                {
                    if (resetSchema)
                    {
                        new SchemaExport(config).Create(false, true);
                    }
                })
                .BuildSessionFactory();
        }

        public void Populate(params Assembly[] dataFixtureAssemblies)
        {
            var fixtureVertices = new Dictionary<Type, Vertex>();
            var fixtureContainer = new FixtureContainer();

            foreach (var dataFixtureType in dataFixtureAssemblies.SelectMany(dataFixtureAssembly => dataFixtureAssembly.GetTypes().Where(t => typeof (DataFixture).IsAssignableFrom(t))))
                fixtureContainer.AddDataFixture((DataFixture) Activator.CreateInstance(dataFixtureType));
               
            using (var session = new Session(_sessionFactory.OpenSession()))
            using (var transaction = session.BeginTransaction())
            {
                foreach (var fixture in fixtureContainer.All)
                {
                    fixtureVertices.Add(
                        fixture.GetType(),
                        new Vertex(() =>
                        {
                            foreach (var entity in fixture.GetEntities(fixtureContainer))
                            {
                                session.Save(entity);
                            }
                        }));
                }

                foreach (var dataFixtureType in fixtureVertices.Keys)
                {
                    var dataFixture = fixtureContainer.Get(dataFixtureType);
                    var vertex = fixtureVertices[dataFixtureType];

                    vertex.AddDependencies(dataFixture.Dependencies.Select(d => fixtureVertices[d]).ToArray());
                }

                var graph = new DirectedAcyclicGraph(fixtureVertices.Values);
                var graphExecutive = new ConcurrentGraphExecutive(graph);

                graphExecutive.ExecuteAndWait();

                if (graphExecutive.VerticesFailed.Any())
                    throw new AggregateException(graphExecutive.VerticesFailed.Select(e=>e.Value));
                 
                transaction.Commit();
            }
        }
    }
}
