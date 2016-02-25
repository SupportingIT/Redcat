namespace Redcat.Core
{
    public class Contact
    {
        public Contact(object id)
        {
            Id = id;
        }

        public object Id { get; }
        public string Name { get; set; }
    }
}
