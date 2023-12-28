## 2.2.0 - Nova 2. Delivery 38 (December 13, 2023)
### What's changed
* LT-5045: Create unique constraint for orderid field.

### Deployment
* Run the following SQL script to updated indexes:
    ```sql
    IF exists
    (SELECT name
     FROM sys.indexes
     WHERE name = N'IX_ExecutionOrderBooks_OrderId'
       AND object_id = object_id(N'[dbo].[ExecutionOrderBooks]', N'U'))
    RETURN

    CREATE UNIQUE INDEX IX_ExecutionOrderBooks_OrderId ON [dbo].[ExecutionOrderBooks] ([OrderId]);
    ```

## 2.1.1 - Nova 2. Delivery 36 (2023-08-31)
### What's changed
* LT-4899: Update nugets.


**Full change log**: https://github.com/lykkebusiness/margintrading.orderbookservice/compare/v2.0.8...v2.1.1


## 2.0.8 - Nova 2. Delivery 31. Hotfix 5 (2023-02-20)
## What's Changed
* LT-4529: ExecutionOrderBookBroker cannot write in the database by @tarurar in https://github.com/LykkeBusiness/MarginTrading.OrderBookService/pull/42

### Deployment
The following sql script has to be executed against database:

```sql
ALTER TABLE dbo.ExecutionOrderBooks
ALTER COLUMN ReceiveTimestamp datetime2
GO
```

_Please note_: `ExecutionOrderBookBroker` and `OrderBookBroker` services has to be updated as well cause they share the same code of repository to access the database.

**Full Changelog**: https://github.com/LykkeBusiness/MarginTrading.OrderBookService/compare/v2.0.7...v2.0.8


## 2.0.8 - Nova 2. Delivery 31. (2023-01-16)
## What's changed
* LT-4230: Issue with orderbook (fields - receivetimestamp and volume).

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


**Full change log**: https://github.com/lykkebusiness/margintrading.orderbookservice/compare/v2.0.6...v2.0.7
