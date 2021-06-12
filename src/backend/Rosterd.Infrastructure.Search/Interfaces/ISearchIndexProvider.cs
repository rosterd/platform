using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes.Models;

namespace Rosterd.Infrastructure.Search.Interfaces
{
    public interface ISearchIndexProvider
    {
        /// <summary>
        /// Create or updates an existing search index with the given index name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        Task CreateOrUpdateIndex<T>(string index);


        /// <summary>
        /// Create or updates an existing search index with the given index name and search suggesters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="searchSuggestersToAdd"></param>
        /// <returns></returns>
        Task CreateOrUpdateIndex<T>(string index, List<SearchSuggester> searchSuggestersToAdd);

        /// <summary>
        /// Deletes an index
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        Task DeleteIndex(string indexName);

        /// <summary>
        /// Gets the index status
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Task<(bool Exists, long DocumentCount)> GetIndexStatus(string index);

        /// <summary>
        /// Upsets documents to an index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexToAddTo"></param>
        /// <param name="itemsToAdd"></param>
        /// <returns></returns>
        Task AddOrUpdateDocumentsToIndex<T>(string indexToAddTo, List<T> itemsToAdd);

        /// <summary>
        /// Delete all the given documents from the index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexToDeleteFrom"></param>
        /// <param name="keyName"></param>
        /// <param name="keysToDelete"></param>
        /// <returns></returns>
        Task DeleteDocumentsFromIndex(string indexToDeleteFrom, string keyName, List<string> keysToDelete);

        SearchClient GetSearchClient(string searchIndex);
    }
}
