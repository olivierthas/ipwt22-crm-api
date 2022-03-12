namespace Crm.Link.Api.Extensions
{
    /// <summary>
    /// Extensions for IEnumerable.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Checks if a IEnumerable is empty. (Readability)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
            => enumerable.Count() == 0;

        /// <summary>
        /// Allows the use of Foreach on an enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
            => enumerable.ToList().ForEach(action);
    }
}
