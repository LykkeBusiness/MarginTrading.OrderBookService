// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Dapper;
using Lykke.Logs.MsSql.Extensions;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;
using MarginTrading.OrderBookService.Core.Repositories;

namespace MarginTrading.OrderBookService.SqlRepositories
{
    public class ExecutionOrderBookRepository : IExecutionOrderBookRepository
    {
        private const string TableName = "ExecutionOrderBooks";

        private const string CreateTableScript = "CREATE TABLE [{0}](" +
                                                 @"[OID] [bigint] NOT NULL IDENTITY (1,1),
[OrderId] [nvarchar](64) NOT NULL,
[Spread] [float] NOT NULL,
[ExchangeName] [nvarchar](64) NOT NULL,
[AssetPairId] [nvarchar](64) NOT NULL,
[Timestamp] [datetime] NOT NULL,
[Asks] [nvarchar](MAX) NOT NULL,
[Bids] [nvarchar](MAX) NOT NULL,
INDEX IX_{0}_Base (OrderId)
);";
        
        private readonly string _connectionString;
        private readonly ILog _log;

        private static readonly PropertyInfo[] Properties = typeof(OrderExecutionOrderBookEntity).GetProperties();

        private static readonly string GetColumns = string.Join(",", Properties.Select(x => x.Name));

        private static readonly string GetFields = string.Join(",", Properties.Select(x => "@" + x.Name));

        private static readonly string GetUpdateClause = string.Join(",",
            Properties.Select(x => "[" + x.Name + "]=@" + x.Name));

        public ExecutionOrderBookRepository(string connectionString, ILog log)
        {
            _connectionString = connectionString;
            _log = log;
            
            using (var conn = new SqlConnection(connectionString))
            {
                try { conn.CreateTableIfDoesntExists(CreateTableScript, TableName); }
                catch (Exception ex)
                {
                    _log?.WriteErrorAsync(TableName, "CreateTableIfDoesntExists", null, ex);
                    throw;
                }
            }
        }

        public async Task AddAsync(IOrderExecutionOrderBook orderBook)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    var entity = OrderExecutionOrderBookEntity.Create(orderBook);
                    var sql = $"insert into {TableName} ({GetColumns}) values ({GetFields})";
                    await conn.ExecuteAsync(sql, entity);
                }
                catch (Exception ex)
                {
                    var msg = $"Error {ex.Message} \n" +
                              $"Entity <{nameof(OrderExecutionOrderBookEntity)}>: \n" +
                              orderBook.ToJson();
                    
                    _log?.WriteWarning(nameof(ExecutionOrderBookRepository), nameof(AddAsync), msg);
                    
                    throw new Exception(msg);
                }
            }
        }

        public async Task<IOrderExecutionOrderBook> GetAsync(string orderId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<OrderExecutionOrderBookEntity>(
                    $"SELECT * FROM {TableName} WHERE OrderId=@orderId", new {orderId});
            }
        }
    }
}