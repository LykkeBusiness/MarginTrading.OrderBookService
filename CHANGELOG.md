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
