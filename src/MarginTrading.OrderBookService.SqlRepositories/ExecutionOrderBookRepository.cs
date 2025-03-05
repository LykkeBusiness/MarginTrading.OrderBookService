// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Common;

using Dapper;

using Lykke.Logs.MsSql.Extensions;

using MarginTrading.OrderBookService.Core.Domain.Abstractions;
using MarginTrading.OrderBookService.Core.Repositories;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MarginTrading.OrderBookService.SqlRepositories
{
    public class ExecutionOrderBookRepository : IExecutionOrderBookRepository
    {
        private const string TableName = "ExecutionOrderBooks";

        private const string CreateTableScript = @"
create table {0}
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
";
        
        private const string CreateIndexesScript = @"
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_ExecutionOrderBooks_Base' AND object_id = OBJECT_ID('{0}'))
BEGIN
CREATE INDEX IX_ExecutionOrderBooks_Base ON {0} (OrderId) INCLUDE (Spread)
END;

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_ExecutionOrderBooks_ExternalOrderId' AND object_id = OBJECT_ID('{0}'))
BEGIN
CREATE INDEX IX_ExecutionOrderBooks_ExternalOrderId ON {0} (ExternalOrderId) INCLUDE (Spread)
END;

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_ExecutionOrderBooks_OrderId' AND object_id = OBJECT_ID('{0}'))
BEGIN
CREATE UNIQUE INDEX IX_ExecutionOrderBooks_OrderId ON {0} (ExternalOrderId) INCLUDE (Spread)
END;
";

        private readonly string _connectionString;
        private readonly ILogger<ExecutionOrderBookRepository> _logger;

        private static readonly PropertyInfo[] Properties = typeof(OrderExecutionOrderBookEntity).GetProperties();

        private static readonly string GetColumns = string.Join(",", Properties.Select(x => x.Name));

        private static readonly string GetFields = string.Join(",", Properties.Select(x => "@" + x.Name));

        static ExecutionOrderBookRepository()
        {
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
        }

        public ExecutionOrderBookRepository(string connectionString, ILogger<ExecutionOrderBookRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;

            using var conn = new SqlConnection(connectionString);
            try
            {
                conn.CreateTableIfDoesntExists(CreateTableScript, TableName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on table [{TableName}] creation", TableName);
                throw;
            }
            
            try
            {
                conn.ExecuteAsync(string.Format(CreateIndexesScript, TableName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on indexes [{TableName}] creation. Exception suppressed.", TableName);
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
                _logger.LogWarning(msg);
                throw;
            }
        }

        public async Task<IOrderExecutionOrderBook> GetAsync(string orderId)
        {
            await using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<OrderExecutionOrderBookEntity>(
                $"SELECT * FROM {TableName} WHERE OrderId=@orderId", new { orderId });
        }

        public async Task<IOrderExecutionOrderBook> GetByExternalOrderAsync(string externalOrderId)
        {
            await using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<OrderExecutionOrderBookEntity>(
                $"SELECT * FROM {TableName} WHERE ExternalOrderId=@externalOrderId", new { externalOrderId });
        }
    }
}