namespace Bootstrap4NHibernate.Data
{
    public interface IFixtureContainer
    {
        TFixture Get<TFixture>() where TFixture : DataFixture;
    }
}
