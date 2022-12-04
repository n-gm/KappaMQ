namespace KappaMQ.Interfaces
{
    public interface IKappaMQProducer
    {
        /// <summary>
        /// Produce message to queue
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="body"></param>
        void Produce(string queue, string body);
        /// <summary>
        /// Produce message to queue. Body serializing to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="body"></param>
        void Produce<T>(string queue, T body) where T : class;
    }
}
