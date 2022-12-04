namespace KappaMQ.Interfaces
{
    public interface IKappaMQConnector
    {
        void Connect(string connectionString);
        void Disconnect();
    }
}
