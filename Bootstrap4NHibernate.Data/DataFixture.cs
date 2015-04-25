using System;

namespace Bootstrap4NHibernate.Data
{
    public abstract class DataFixture
    {
        public virtual Type[] Dependencies { get { return new Type[0];} }

        public abstract object[] GetEntities(IFixtureContainer fixtureContainer);
    }
}
