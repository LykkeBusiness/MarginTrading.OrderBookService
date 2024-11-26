# MarginTrading.OrderBookService API #

API for order book retrieval from cache.

## How to use in prod env? ##

1. Pull "mt-orderbook-service" docker image with a corresponding tag.
2. Configure environment variables according to "Environment variables" section.
3. Put secrets.json with endpoint data including the certificate:
```json
{
  "Kestrel": {
    "EndPoints": {
      "HttpsInlineCertFile": {
        "Url": "https://*:5190",
        "Certificate": {
          "Path": "<path to .pfx file>",
          "Password": "<certificate password>"
        }
      }
    }
  }
}
```
4. Initialize all dependencies.
5. Run.

## How to run for debug? ##

1. Clone repo to some directory.
2. In MarginTrading.OrderBookService root create a appsettings.dev.json with settings.
3. Add environment variable "SettingsUrl": "appsettings.dev.json".
4. VPN to a corresponding env must be connected and all dependencies must be initialized.
5. Run.

### Dependencies ###

TBD

### Configuration ###

Kestrel configuration may be passed through appsettings.json, secrets or environment.
All variables and value constraints are default. For instance, to set host URL the following env variable may be set:
```json
{
    "Kestrel__EndPoints__Http__Url": "http://*:5090"
}
```

### Environment variables ###

* *RESTART_ATTEMPTS_NUMBER* - number of restart attempts. If not set int.MaxValue is used.
* *RESTART_ATTEMPTS_INTERVAL_MS* - interval between restarts in milliseconds. If not set 10000 is used.
* *SettingsUrl* - defines URL of remote settings or path for local settings.

### Settings ###

OrderBookService settings schema is:
<!-- MARKDOWN-AUTO-DOCS:START (CODE:src=./service.json) -->
<!-- The below code snippet is automatically added from ./service.json -->
```json
{
  "APP_UID": "Integer",
  "ASPNETCORE_ENVIRONMENT": "String",
  "ENVIRONMENT": "String",
  "IsLive": "Boolean",
  "Kestrel": {
    "EndPoints": {
      "Http": {
        "Url": "String"
      }
    }
  },
  "OrderBookService": {
    "ChaosKitty": {
      "StateOfChaos": "Integer"
    },
    "Db": {
      "DataConnString": "String",
      "LogsConnString": "String",
      "OrderBooksCacheKeyPattern": "String",
      "RedisSettings": {
        "Configuration": "String"
      },
      "StorageMode": "String"
    },
    "UseSerilog": "Boolean"
  },
  "OrderBookServiceClient": {
    "ApiKey": "String",
    "ServiceUrl": "String",
    "UseSerilog": "Boolean"
  },
  "serilog": {
    "minimumLevel": {
      "default": "String"
    }
  },
  "TZ": "String"
}
```
<!-- MARKDOWN-AUTO-DOCS:END -->

ExecutionOrderBookBroker settings schema is:
<!-- MARKDOWN-AUTO-DOCS:START (CODE:src=./executionBroker.json) -->
<!-- The below code snippet is automatically added from ./executionBroker.json -->
```json
{
  "APP_UID": "Integer",
  "ASPNETCORE_ENVIRONMENT": "String",
  "ENVIRONMENT": "String",
  "Kestrel": {
    "EndPoints": {
      "Http": {
        "Url": "String"
      }
    }
  },
  "MtBrokerSettings": {
    "Db": {
      "ConnString": "String",
      "StorageMode": "String"
    },
    "MtRabbitMqConnString": "String",
    "RabbitMqQueues": {
      "ExecutionOrderBooks": {
        "ExchangeName": "String"
      }
    }
  },
  "MtBrokersLogs": {
    "LogsConnString": "String",
    "StorageMode": "String",
    "UseSerilog": "Boolean"
  },
  "serilog": {
    "Enrich": [
      "String"
    ],
    "minimumLevel": {
      "default": "String"
    },
    "Properties": {
      "Application": "String"
    },
    "Using": [
      "String"
    ],
    "writeTo": [
      {
        "Args": {
          "configure": [
            {
              "Args": {
                "outputTemplate": "String"
              },
              "Name": "String"
            }
          ]
        },
        "Name": "String"
      }
    ]
  },
  "TZ": "String"
}
```
<!-- MARKDOWN-AUTO-DOCS:END -->

OrderBookBroker settings schema is:
<!-- MARKDOWN-AUTO-DOCS:START (CODE:src=./broker.json) -->
<!-- The below code snippet is automatically added from ./broker.json -->
```json
{
  "APP_UID": "Integer",
  "ASPNETCORE_ENVIRONMENT": "String",
  "ENVIRONMENT": "String",
  "IsLive": "Boolean",
  "Kestrel": {
    "EndPoints": {
      "Http": {
        "Url": "String"
      }
    }
  },
  "MtBrokerSettings": {
    "Db": {
      "RedisConfiguration": "String"
    },
    "MtRabbitMqConnString": "String",
    "OrderBooksCacheKeyPattern": "String",
    "OrderBookThrottlingRateThreshold": "Integer",
    "RabbitMqQueues": {
      "OrderBooks": {
        "ExchangeName": "String"
      }
    }
  },
  "MtBrokersLogs": {
    "LogsConnString": "String",
    "StorageMode": "String",
    "UseSerilog": "Boolean"
  },
  "serilog": {
    "Enrich": [
      "String"
    ],
    "minimumLevel": {
      "default": "String"
    },
    "Properties": {
      "Application": "String"
    },
    "Using": [
      "String"
    ],
    "writeTo": [
      {
        "Args": {
          "configure": [
            {
              "Args": {
                "outputTemplate": "String"
              },
              "Name": "String"
            }
          ]
        },
        "Name": "String"
      }
    ]
  },
  "TZ": "String"
}
```
<!-- MARKDOWN-AUTO-DOCS:END -->
