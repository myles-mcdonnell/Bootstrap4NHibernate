namespace Bootstrap4NHibernate.Data
{
    public interface ISession
    {
        T Save<T>(T entity);
    }
}
