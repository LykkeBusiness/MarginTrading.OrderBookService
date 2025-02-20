## 2.10.1 - Nova 2. Delivery 49. Hotfix 1 (February 17, 2025)
### What's changed
* LT-6054: Don't use settingsService explicitly in brokers


## 2.10.0 - Nova 2. Delivery 49 (February 07, 2025)
### What's changed
* LT-5993: Update rabbitmqbroker in margintrading.orderbookservice.

## 2.8.1 - Nova 2. Delivery 47. Hotfix 2 (January 15, 2025)
### What's changed
* LT-5991: Bump LykkeBiz.RabbitMqBroker to 8.11.1


## 2.9.0 - Nova 2. Delivery 48 (December 19, 2024)
### What's changed
* LT-5930: Update refit to 8.x version.
* LT-5881: Keep schema for appsettings.json up to date.
* LT-5874: Keep schema for appsettings.json up to date.
* LT-5873: Keep schema for appsettings.json up to date.


## 2.8.0 - Nova 2. Delivery 47 (November 15, 2024)
### What's changed
* LT-5827: Update messagepack to 2.x version.
* LT-5773: Add assembly load logger.
* LT-5763: Migrate to quorum queues.

### Deployment
In this release, all previously specified queues have been converted to quorum queues to enhance system reliability. The affected queues are:
- `dev.Gavel.events.exchange.MarginTrading.OrderBookService.ExecutionOrderBookBroker.DefaultEnv`
- `lykke.exchangeconnector.orderbooks.MarginTrading.OrderBookService.OrderBookBroker.DefaultEnv`

#### Automatic Conversion to Quorum Queues
The conversion to quorum queues will occur automatically upon service startup **if**:
* There are **no messages** in the existing queues.
* There are **no active** subscribers to the queues.

**Warning**: If messages or subscribers are present, the automatic conversion will fail. In such cases, please perform the following steps:
1. Run the previous version of the component associated with the queue.
1. Make sure all the messages are processed and the queue is empty.
1. Shut down the component associated with the queue.
1. Manually delete the existing classic queue from the RabbitMQ server.
1. Restart the component to allow it to create the quorum queue automatically.

#### Poison Queues
All the above is also applicable to the poison queues associated with the affected queues. Please ensure that the poison queues are also converted to quorum queues.

#### Disabling Mirroring Policies
Since quorum queues inherently provide data replication and reliability, server-side mirroring policies are no longer necessary for these queues. Please disable any existing mirroring policies applied to them to prevent redundant configurations and potential conflicts.

#### Environment and Instance Identifiers
Please note that the queue names may include environment-specific identifiers (e.g., dev, test, prod). Ensure you replace these placeholders with the actual environment names relevant to your deployment. The same applies to instance names embedded within the queue names (e.g., DefaultEnv, etc.).


## 2.7.0 - Nova 2. Delivery 46 (September 26, 2024)
### What's changed
* LT-5601: Migrate to net 8.


## 2.6.0 - Nova 2. Delivery 44 (August 19, 2024)
### What's changed
* LT-5623: Add api to fetch last non zero spread by assetid + save it in broker.
* LT-5516: Update rabbitmq broker library with new rabbitmq.client and templates.

### Deployment
Please ensure that the mirroring policy is configured on the RabbitMQ server side for the following queues:
- `dev.Gavel.events.exchange.MarginTrading.OrderBookService.ExecutionOrderBookBroker.DefaultEnv`
- `lykke.exchangeconnector.orderbooks.MarginTrading.OrderBookService.OrderBookBroker.DefaultEnv`

These queues require the mirroring policy to be enabled as part of our ongoing initiative to enhance system reliability. They are now classified as "no loss" queues, which necessitates proper configuration. The mirroring feature must be enabled on the RabbitMQ server side.

In some cases, you may encounter an error indicating that the server-side configuration of a queue differs from the client’s expected configuration. If this occurs, please delete the queue, allowing it to be automatically recreated by the client.

**Warning**: The "no loss" configuration is only valid if the mirroring policy is enabled on the server side.

Please be aware that the provided queue names may include environment-specific identifiers (e.g., dev, test, prod). Be sure to replace these with the actual environment name in use. The same applies to instance names embedded within the queue names (e.g., DefaultEnv, etc.).


## 2.5.0 - Nova 2. Delivery 41 (April 01, 2024)
### What's changed
* LT-5449: Update packages.


## 2.4.0 - Nova 2. Delivery 40 (February 28, 2024)
### What's changed
* LT-5213: Update lykke.httpclientgenerator to 5.6.2.


## 2.3.1 - Nova 2. Delivery 39. Hotfix 2 (February 7, 2024)
### What's changed
* LT-5245: Update vulnerable packages


## 2.3.0 - Nova 2. Delivery 39 (January 30, 2024)
### What's changed
* LT-5145: Changelog.md for orderbook service.


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
