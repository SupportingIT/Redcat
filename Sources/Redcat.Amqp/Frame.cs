namespace Redcat.Amqp
{
    public class Frame
    {
        protected Frame(int type)
        {
            Type = type;
        }

        private int Type { get; }
    }
}
