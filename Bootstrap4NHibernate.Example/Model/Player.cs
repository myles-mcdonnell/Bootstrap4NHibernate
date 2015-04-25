namespace Bootstrap4NHibernate.Example.Model
{
    public class Player
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Team Team { get; set; }
    }
}
