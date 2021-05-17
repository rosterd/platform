using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Rosterd.Infrastructure.Search.Interfaces;

namespace Rosterd.Infrastructure.Search
{
    public class SearchIndexProvider : ISearchIndexProvider
    {
        private readonly SearchIndexClient _searchIndexClient;
        private readonly AzureKeyCredential _azureKeyCredential;
        private readonly Uri _azureSearchEndpoint;

        public SearchIndexProvider(string searchEndpoint, string searchApiKey)
        {
            _azureSearchEndpoint = new Uri(searchEndpoint);
            _azureKeyCredential = new AzureKeyCredential(searchApiKey);

            _searchIndexClient = new SearchIndexClient(_azureSearchEndpoint, _azureKeyCredential);
        }

        ///<inheritdoc/>
        public async Task CreateOrUpdateIndex<T>(string index) => await CreateOrUpdateIndex<T>(index, new List<SearchSuggester>());

        public async Task CreateOrUpdateIndex<T>(string index, List<SearchSuggester> searchSuggestersToAdd)
        {
            var builder = new FieldBuilder();
            var definition = new SearchIndex(index, builder.Build(typeof(T)));

            if (searchSuggestersToAdd.IsNotNullOrEmpty())
                definition.Suggesters.AddRange(searchSuggestersToAdd.ToArray());

            await _searchIndexClient.CreateOrUpdateIndexAsync(definition);
        }

        ///<inheritdoc/>
        public async Task DeleteIndex(string indexName) => await _searchIndexClient.DeleteIndexAsync(indexName);

        ///<inheritdoc/>
        public async Task<(bool Exists, long DocumentCount)> GetIndexStatus(string index)
        {
            try
            {
                //Check if index exists
                var indexToCheck = await _searchIndexClient.GetIndexAsync(index);
                if (indexToCheck?.Value == null)
                    return (false, 0);

                //Index exists return the document count
                var searchClient = new SearchClient(_azureSearchEndpoint, index, _azureKeyCredential);
                var documentsInIndex = await searchClient.GetDocumentCountAsync();
                return (true, documentsInIndex);
            }
            catch
            {
                return (false, 0);
            }
        }

        ///<inheritdoc/>
        public async Task AddOrUpdateDocumentsToIndex<T>(string indexToAddTo, List<T> itemsToAdd)
        {
            var batch = IndexDocumentsBatch.Upload(itemsToAdd);
            var searchClient = new SearchClient(_azureSearchEndpoint, indexToAddTo, _azureKeyCredential);

            await searchClient.IndexDocumentsAsync(batch);
        }

        ///<inheritdoc/>
        public async Task DeleteDocumentsFromIndex(string indexToDeleteFrom, string keyName, List<string> keysToDelete)
        {
            var searchClient = new SearchClient(_azureSearchEndpoint, indexToDeleteFrom, _azureKeyCredential);
            await searchClient.DeleteDocumentsAsync(keyName, keysToDelete);
        }
    }
}
