using System;
using System.Collections.Generic;
using System.Linq;
using Bootstrap4NHibernate.Data;

namespace Bootstrap4NHibernate
{
    public class FixtureContainer : IFixtureContainer
    {
        private readonly IDictionary<Type, DataFixture> _fixtures = new Dictionary<Type, DataFixture>();
        public DataFixture AddDataFixture(DataFixture dataFixture)
        {
            var type = dataFixture.GetType();

            _fixtures.Add(type, dataFixture);

            return dataFixture;
        }

        public TFixture Get<TFixture>() where TFixture : DataFixture
        {
            return (TFixture)_fixtures[typeof (TFixture)];
        }

        public DataFixture Get(Type type)
        {
            return _fixtures[type];
        }

        public DataFixture[] All
        {
            get { return _fixtures.Values.ToArray(); }
        }
    }
}
