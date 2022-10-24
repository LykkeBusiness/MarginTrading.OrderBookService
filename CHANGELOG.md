## (TBD)

* LT-4230: Issue with orderbook (fields - ReceiveTimestamp and Volume)

### Deployment

The following sql script has to be executed against database:

```sql
ALTER TABLE dbo.ExecutionOrderBooks
    ADD Volume float NOT NULL default 0
GO

ALTER TABLE dbo.ExecutionOrderBooks
    ADD ReceiveTimestamp datetime
GO
```

## 1.4.5

* LT-2217: Save execution order book linked to external order id, not internal.

The following sql script has to be executed against database:

```sql
-- schema update
ALTER TABLE ExecutionOrderBooks ALTER COLUMN OrderId nvarchar(128) NOT NULL
GO

ALTER TABLE ExecutionOrderBooks
ADD ExternalOrderId nvarchar(128)
GO

CREATE INDEX IX_ExecutionOrderBooks_ExternalOrderId ON ExecutionOrderBooks (ExternalOrderId) include (Spread)
GO

-- data migration
UPDATE e
SET ExternalOrderId = t.ExternalOrderId
FROM ExecutionOrderBooks AS e
LEFT JOIN Trades AS t
  ON e.OrderId = t.Id
```