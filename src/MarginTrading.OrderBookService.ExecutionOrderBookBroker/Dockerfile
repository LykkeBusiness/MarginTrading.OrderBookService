FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY . .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENTRYPOINT ["dotnet", "MarginTrading.OrderBookService.ExecutionOrderBookBroker.dll"]
