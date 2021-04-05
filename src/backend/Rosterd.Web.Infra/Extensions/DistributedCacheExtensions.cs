using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Rosterd.Web.Infra.Extensions
{
    /// <summary>
    ///     <see cref="IDistributedCache" /> extension methods.
    /// </summary>
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Helper method that gets the cache expiry for a given absolute expiry in minutes
        /// </summary>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="absoluteExpiryInMinutes">The cache to expire in minutes</param>
        /// <returns></returns>
        public static DistributedCacheEntryOptions GetCacheExpiry(this IDistributedCache cache, int absoluteExpiryInMinutes) => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = new TimeSpan(0, absoluteExpiryInMinutes, 0)
        };

        /// <summary>
        /// Gets the value of type <typeparamref name="T" /> with the specified key from the cache asynchronously by
        /// deserializing it from JSON format or returns <c>default(T)</c> if the key was not found.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="jsonSerializerOptions">The JSON serializer options or <c>null</c> to use the default.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value of type <typeparamref name="T" /> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache"/> or <paramref name="key"/> is
        /// <c>null</c>.</exception>
        public static async Task<T> GetAsJsonAsync<T>(
            this IDistributedCache cache,
            string key,
            JsonSerializerOptions jsonSerializerOptions = null,
            CancellationToken cancellationToken = default) where T : class
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var bytes = await cache.GetAsync(key, cancellationToken);
            return (bytes.IsNullOrEmpty() ? default : Deserialize<T>(bytes, jsonSerializerOptions));
        }

        /// <summary>
        ///     Sets the value of type <typeparamref name="T" /> with the specified key in the cache asynchronously by
        ///     serializing it to JSON format.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="options">The cache options or <c>null</c> to use the default cache options.</param>
        /// <param name="jsonSerializerOptions">The JSON serializer options or <c>null</c> to use the default.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value of type <typeparamref name="T" /> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="cache" /> or <paramref name="key" /> is
        ///     <c>null</c>.
        /// </exception>
        public static Task SetAsJsonAsync<T>(
            this IDistributedCache cache,
            string key,
            T value,
            DistributedCacheEntryOptions options = null,
            JsonSerializerOptions jsonSerializerOptions = null,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, jsonSerializerOptions);
            return cache.SetAsync(key, bytes, options, cancellationToken);
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="T"/> with the specified key from the cache asynchronously by
        /// deserializing it from JSON format or calls the cacheMiss function and set the cache if the key was not found.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value.
        /// </typeparam>
        /// <param name="cache">
        /// The distributed cache.
        /// </param>
        /// <param name="key">
        /// The cache item key.
        /// </param>
        /// <param name="cacheMiss">
        /// The function executed when cache is missed
        /// </param>
        /// <param name="options">
        /// The cache options or <c>null</c> to use the default cache options.
        /// </param>
        /// <returns>
        /// The value of type <typeparamref name="T"/> or <c>null</c> if the key was not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cache"/> or <paramref name="key"/> is <c>null</c>.
        /// </exception>
        public static async Task<T> GetAsJsonAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> cacheMiss, DistributedCacheEntryOptions options = null)
            where T : class
        {
            var item = await cache.GetAsJsonAsync<T>(key);
            if (item != null)
                return item;

            var value = await cacheMiss();
            if (value == null)
                return default;

            await SetAsJsonAsync(cache, key, value, options);
            return value;
        }

        private static T Deserialize<T>(byte[] bytes, JsonSerializerOptions jsonSerializerOptions)
        {
            var utf8JsonReader = new Utf8JsonReader(bytes);
            return JsonSerializer.Deserialize<T>(ref utf8JsonReader, jsonSerializerOptions);
        }
    }
}
