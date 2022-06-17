using Microsoft.Extensions.Logging;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// A singleton container using <see cref="Log.GetOrCreateLogger"/> to retrieve a logger
    /// with the type name.
    /// </summary>
    /// <typeparam name="T">The type name to get a logger for.</typeparam>
    public static class Log<T>
    {
        /// <summary>
        /// A logger for this type name.
        /// </summary>
        public static ILogger? Logger => Log.GetOrCreateLogger(typeof(T).Name);
    }
}