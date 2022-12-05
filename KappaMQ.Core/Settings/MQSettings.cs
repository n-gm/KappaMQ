namespace KappaMQ.Core.Settings
{
    internal class MQSettings
    {
        /// <summary>
        /// Lease time before return message to queue. Measure in seconds. Default - 60s.
        /// </summary>
        public int LeaseTime { get; set; } = 60;
        /// <summary>
        /// Maximum reaction time before new message will be processed. Measure in ms. Default - 10ms.
        /// </summary>
        public int ReactionTime { get; set; } = 10;
    }
}
