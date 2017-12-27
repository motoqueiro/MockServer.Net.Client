namespace MockServer.Net.Client.Entities
{
    public enum LogLevelEnum
    {
        OFF,

        /// <summary>
        /// Exceptions and errors.
        /// </summary>
        WARN,

        /// <summary>
        /// All interactions with the MockServer including setting up expectations, matching expectations, clearing expectations and verifying requests.
        /// </summary>
        INFO,

        DEBUG,

        /// <summary>
        /// All matcher results, including when specific matchers fail(such as HeaderMatcher).
        /// </summary>
        TRACE,

        ALL
    }
}