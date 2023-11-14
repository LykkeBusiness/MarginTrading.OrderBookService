// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Dapper;
using Lykke.Logs.MsSql.Extensions;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;
using MarginTrading.OrderBookService.Core.Repositories;
using Microsoft.Data.SqlClient;

namespace MarginTrading.OrderBookService.SqlRepositories
{
    public class ExecutionOrderBookRepository : IExecutionOrderBookRepository
    {
        private const string TableName = "ExecutionOrderBooks";

        private const string CreateTableScript = @"
create table ExecutionOrderBooks
(
    OID              bigint identity,
    OrderId          nvarchar(128)   not null,
    ExchangeName     nvarchar(64)    not null,
    AssetPairId      nvarchar(64)    not null,
    Timestamp        datetime        not null,
    Asks             nvarchar(max)   not null,
    Bids             nvarchar(max)   not null,
    Spread           float default 0 not null,
    ExternalOrderId  nvarchar(128),
    Volume           float default 0 not null,
    ReceiveTimestamp datetime2
)

create index IX_ExecutionOrderBooks_Base
    on ExecutionOrderBooks (OrderId) include (Spread)

create index IX_ExecutionOrderBooks_ExternalOrderId
    on ExecutionOrderBooks (ExternalOrderId) include (Spread)

create unique index IX_ExecutionOrderBooks_OrderId
    on ExecutionOrderBooks (OrderId)
";
        
        private readonly string _connectionString;
        private readonly ILog _log;

        private static readonly PropertyInfo[] Properties = typeof(OrderExecutionOrderBookEntity).GetProperties();

        private static readonly string GetColumns = string.Join(",", Properties.Select(x => x.Name));

        private static readonly string GetFields = string.Join(",", Properties.Select(x => "@" + x.Name));

        static ExecutionOrderBookRepository()
        {
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
        }

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
            await using var conn = new SqlConnection(_connectionString);
            var entity = OrderExecutionOrderBookEntity.Create(orderBook);
            var sql = $"insert into {TableName} ({GetColumns}) values ({GetFields})";
            try
            {
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                var msg = $"Error {ex.Message} \n" +
                          $"Entity <{nameof(OrderExecutionOrderBookEntity)}>: \n" +
                          orderBook.ToJson();
                    
                _log?.WriteWarning(nameof(ExecutionOrderBookRepository), nameof(AddAsync), msg);

                throw;
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

        public async Task<IOrderExecutionOrderBook> GetByExternalOrderAsync(string externalOrderId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<OrderExecutionOrderBookEntity>(
                    $"SELECT * FROM {TableName} WHERE ExternalOrderId=@externalOrderId", new {externalOrderId});
            }
        }
    }
}