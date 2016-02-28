namespace Redcat.Core
{
    public abstract class Contact
    {
        protected Contact(object id)
        {
            Id = id;
        }

        public object Id { get; }
        public string Name { get; set; }
    }
}
