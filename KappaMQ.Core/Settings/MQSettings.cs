namespace KappaMQ.Core.Settings
{
    internal class MQSettings
    {
        /// <summary>
        /// Lease time before return message to queue. Measure in seconds. Default - 60s.
        /// </summary>
        public int LeaseTime { get; set; } = 60;
    }
}
