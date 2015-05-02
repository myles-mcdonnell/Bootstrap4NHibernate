/*The MIT License (MIT)

Copyright (c) 2015 Shiftkey Software

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Linq;
using System.Reflection;
using Bootstrap4NHibernate.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MPM.PDAG;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Bootstrap4NHibernate
{
    public class Database
    {
        private readonly ISessionFactory _sessionFactory;

        public Database(IPersistenceConfigurer persistenceConfigurer, Assembly classMapAssembly, Action<Configuration> configAction = null,
            bool resetSchema = false)
        {
            _sessionFactory = Fluently.Configure()
                .Database(() => persistenceConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssembly(classMapAssembly))
                .ExposeConfiguration(config =>
                {
                    if (resetSchema)
                        new SchemaExport(config).Create(false, true);

                    configAction(config);
                })
                
                .BuildSessionFactory();
        }

        public void Populate(params Assembly[] dataFixtureAssemblies)
        {
            var fixtureContainer = new FixtureContainer();

        foreach (var dataFixtureType in dataFixtureAssemblies.SelectMany(dataFixtureAssembly => dataFixtureAssembly.GetTypes().Where(t => typeof (DataFixture).IsAssignableFrom(t))))
            fixtureContainer.AddDataFixture((DataFixture) Activator.CreateInstance(dataFixtureType));

            var fixtureVertices = fixtureContainer.All.ToDictionary(fixture => fixture.GetType(), fixture => new Vertex(() =>
            {
                using (var session = new Session(_sessionFactory.OpenSession()))
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in fixture.GetEntities(fixtureContainer))
                        session.SaveOrUpdate(entity);
                    
                    transaction.Commit();
                }
            }));

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
        }
    }
}
