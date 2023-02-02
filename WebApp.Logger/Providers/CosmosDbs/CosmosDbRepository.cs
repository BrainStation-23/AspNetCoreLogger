using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Loggers;
using WebApp.Logger.Providers.Mongos;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Providers.CosmosDbs
{
    public class CosmosDbRepository<TItem> : ICosmosDbRepository<TItem> where TItem : IItem
    {

        protected readonly CosmosClient _client;
        protected readonly Database _database;
        protected Container _container;

        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string PartitionKey { get; set; }

        private readonly LogOption _logOption;
        private readonly CosmosDb _cosmosOptions;

        public CosmosDbRepository(IOptions<LogOption> options)
        {
            _logOption = options.Value;
            _cosmosOptions = _logOption.Provider.CosmosDb;

            var cosmosClientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            //var cosmosClientOptions = new CosmosClientOptions
            //{
            //    Serializer = new NewtonsoftJsonCosmosSerializer(new JsonSerializerSettings
            //    {
            //        //TypeNameHandling = TypeNameHandling.Auto,
            //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //        PreserveReferencesHandling = PreserveReferencesHandling.None,
            //        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            //        Converters = new JsonConverter[]
            //        {
            //            new MemoryStreamJsonConverter()
            //        }
            //    })
            //};

            DatabaseName = _cosmosOptions.DatabaseName;
            ContainerName = typeof(TItem).Name;
            PartitionKey = "logType";
            _client = new CosmosClient(accountEndpoint: _cosmosOptions.AccountUrl, authKeyOrResourceToken: _cosmosOptions.Key, cosmosClientOptions);

            _database = _client.GetDatabase(DatabaseName);
            //if (string.IsNullOrEmpty(_database.Id))
            //_database = Task.Run(async () => await CreateDatabaseAsync()).Result;

            _container = _database.GetContainer(ContainerName);
            // if (string.IsNullOrEmpty(_container.Id))
            //_container = Task.Run(async () => await CreateContainerAsync()).Result;
        }

        public async Task<Database> CreateDatabaseAsync()
        {
            var response = await _client.CreateDatabaseIfNotExistsAsync(_cosmosOptions.DatabaseName);

            return response.Database;
        }

        public async Task<Container> CreateContainerAsync()
        {
            var response = await _database.CreateContainerIfNotExistsAsync(id: ContainerName,
                partitionKeyPath: "/logType",
                throughput: 400);

            return response.Container;
        }

        public async Task<TItem> GetAsync(string id)
        {
            var response = await _container.ReadItemAsync<TItem>(id, new PartitionKey(id));

            return response.Resource;
        }


        public async Task<dynamic> GetsAsync(string queryString, DapperPager pager)
        {
            double totalRUsConsumed = 0;
            double budgetRUs = 100;
            pager.ContinuationToken = pager.ContinuationToken.ToDecodeBase64();

            var requestOptions = new QueryRequestOptions { MaxItemCount = pager.PageSize };
            IOrderedQueryable<TItem> queryable = _container.GetItemLinqQueryable<TItem>(false, pager.ContinuationToken, requestOptions)
                .OrderByDescending(x => x.CreatedDateUtc);
            //var query = _container.GetItemQueryIterator<TItem>(new QueryDefinition(queryString), continuationToken: continuationToken, requestOptions: requestOptions);

            var query = queryable.ToFeedIterator();
            var items = new List<TItem>();

            //while (query.HasMoreResults)
            //{
            //    var response = await query.ReadNextAsync();

            //    totalRUsConsumed = totalRUsConsumed + response.RequestCharge;

            //    if (totalRUsConsumed > budgetRUs)
            //    {
            //        throw new ArgumentException("RUs budget exceeded!");
            //    }

            //    continuationToken = response.ContinuationToken;

            //    items.AddRange(response.ToList());
            //};


            var response = await query.ReadNextAsync();

            totalRUsConsumed = totalRUsConsumed + response.RequestCharge;

            if (totalRUsConsumed > budgetRUs)
            {
                throw new ArgumentException("RUs budget exceeded!");
            }

            items.AddRange(response.ToList());
            pager.ContinuationToken = response.ContinuationToken.ToEncodeBase64();
            pager.Data = items;
            pager.totalRUsConsumed = totalRUsConsumed;

            return pager;
        }

        public async Task<TItem> UpdateAsync(string id, TItem item)
        {
            //var Id = item.GetType().GetProperty("Id").GetValue(item);
            var response = await _container.UpsertItemAsync(item);

            return response.Resource;
        }

        public async Task<string> GetPartitionKey(Container container)
        {
            ContainerProperties cproperties = await _container.ReadContainerAsync();

            return cproperties.PartitionKeyPath;
        }

        public async Task<TItem> InsertAsync(TItem item)
        {
            var response = await _container.CreateItemAsync(item);

            return response.Resource;
        }

        public async Task<List<TItem>> InsertManyAsync(List<TItem> items)
        {
            List<TItem> itemList = new List<TItem>();
            List<Task<ItemResponse<TItem>>> tasks = new List<Task<ItemResponse<TItem>>>(items.Count);
            foreach (TItem item in items)
            {
                tasks.Add(_container.CreateItemAsync(item));
                //tasks.Add(_container.CreateItemAsync(item)
                //    .ContinueWith(itemResponse =>
                //    {
                //        if (!itemResponse.IsCompletedSuccessfully)
                //        {
                //            AggregateException innerExceptions = itemResponse.Exception.Flatten();
                //            if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                //            {
                //                Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                //            }
                //            else
                //            {
                //                Console.WriteLine($"Exception {innerExceptions.InnerExceptions.FirstOrDefault()}.");
                //            }
                //        }
                //    }));
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var result = task.Result;
                itemList.Add(result.Resource);
            }

            return itemList;
        }

        public async Task<TItem> DeleteAsync(string id)
        {
            var response = await _container.DeleteItemAsync<TItem>(id, new PartitionKey());

            return response.Resource;
        }

        public async Task DeleteAsync(string dateTime, string logType)
        {
            string query = $"SELECT * FROM root r WHERE r.createdDateUtc <= '{dateTime}'";

            if (logType == LogType.Audit.ToString().ToLower())
                await _container.Scripts.ExecuteStoredProcedureAsync<TItem>("spDeleteAuditLogItems", new PartitionKey(logType), new[] { query });
            else if (logType == LogType.Error.ToString().ToLower())
                await _container.Scripts.ExecuteStoredProcedureAsync<TItem>("spDeleteErrorLogItems", new PartitionKey(logType), new[] { query });
            else if (logType == LogType.Request.ToString().ToLower())
                await _container.Scripts.ExecuteStoredProcedureAsync<TItem>("spDeleteRequestLogItems", new PartitionKey(logType), new[] { query });
            else
                await _container.Scripts.ExecuteStoredProcedureAsync<TItem>("spDeleteSqlLogItems", new PartitionKey(logType), new[] { query });

        }
    }
}

// https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-get-started
// https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/Queries/Program.cs
// https://github.com/damienaicheh/AzFunctionCosmosDbPagination
// https://devblogs.microsoft.com/cosmosdb/getting-started-end-to-end-example-2/
// https://billy-mumby-dev.com/paging-in-azure-cosmos-db